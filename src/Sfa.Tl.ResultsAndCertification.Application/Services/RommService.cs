using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Factory;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Models.PostResultsService.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class RommService : IRommService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;
        private readonly ICommonService _commonService;
        private readonly IMapper _mapper;
        private readonly ILogger<RommService> _logger;
        private readonly ISystemProvider _systemProvider;
        private readonly IRepositoryFactory _repositoryFactory;

        public RommService(
            IProviderRepository providerRespository,
            IRegistrationRepository registrationRepository,
            IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository,
            IRepositoryFactory repositoryFactory,
            ICommonService commonService,
            IMapper mapper,
            ISystemProvider systemProvider,
            ILogger<RommService> logger)
        {
            _providerRepository = providerRespository;
            _registrationRepository = registrationRepository;
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
            _repositoryFactory = repositoryFactory;
            _commonService = commonService;
            _mapper = mapper;
            _systemProvider = systemProvider;
            _logger = logger;
        }

        public async Task<IList<RommsRecordResponse>> ValidateRommLearnersAsync(long aoUkprn, IEnumerable<RommCsvRecordResponse> validRommsData)
        {
            var response = new List<RommsRecordResponse>();
            int rowNum = 1;

            var registrationProfiles = await _registrationRepository.GetRegistrationProfilesAsync(validRommsData.Select(e => new TqRegistrationProfile
            {
                UniqueLearnerNumber = e.Uln
            }).ToList());

            foreach (TqRegistrationProfile currentRegistrationProfile in registrationProfiles)
            {
                rowNum++;

                bool isRegistrationActive = IsRegistrationActive(currentRegistrationProfile);
                if (!isRegistrationActive)
                {
                    response.Add(AddStage3ValidationError(rowNum, currentRegistrationProfile.UniqueLearnerNumber, ValidationMessages.InactiveUln));
                    continue;
                }

                bool isDobValid = ValidateDateOfBirth(currentRegistrationProfile, validRommsData);
                if (!isDobValid)
                {
                    response.Add(AddStage3ValidationError(rowNum, currentRegistrationProfile.UniqueLearnerNumber, ValidationMessages.InvalidDateOfBirth));
                    continue;
                }

                bool arePathwayResultsValid = ValidatePathwayResults(currentRegistrationProfile);
                if (!arePathwayResultsValid)
                {
                    response.Add(AddStage3ValidationError(rowNum, currentRegistrationProfile.UniqueLearnerNumber, ValidationMessages.InvalidResultState));
                    continue;
                }

                bool areSpecialismResultsValid = ValidateSpecialismResults(currentRegistrationProfile);
                if (!areSpecialismResultsValid)
                {
                    response.Add(AddStage3ValidationError(rowNum, currentRegistrationProfile.UniqueLearnerNumber, ValidationMessages.InvalidResultState));
                    continue;
                }

                response.Add(new RommsRecordResponse
                {
                    Uln = currentRegistrationProfile.UniqueLearnerNumber,
                    ProfileId = currentRegistrationProfile.Id
                });
            }

            return response;
        }

        public async Task<IList<RommsRecordResponse>> ValidateRommTlevelsAsync(long aoUkprn, IEnumerable<RommCsvRecordResponse> validRommsData)
        {
            var response = new List<RommsRecordResponse>();
            var aoProviderTlevels = await GetAllTLevelsByAoUkprnAsync(aoUkprn);
            var academicYears = await _commonService.GetAcademicYearsAsync();

            foreach (var rommData in validRommsData)
            {
                var academicYear = academicYears.FirstOrDefault(x => x.Name.Equals(rommData.AcademicYearName, StringComparison.InvariantCultureIgnoreCase));
                if (academicYear == null)
                {
                    response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.AcademicYearIsNotValid));
                    continue;
                }
                else
                    rommData.AcademicYear = academicYear.Year;

                var isProviderRegisteredWithAwardingOrganisation = aoProviderTlevels.Any(t => t.ProviderUkprn == rommData.ProviderUkprn);
                if (!isProviderRegisteredWithAwardingOrganisation)
                {
                    response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.ProviderNotRegisteredWithAo));
                    continue;
                }

                var technicalQualification = aoProviderTlevels.FirstOrDefault(tq => tq.ProviderUkprn == rommData.ProviderUkprn && tq.PathwayLarId == rommData.CoreCode);
                if (technicalQualification == null)
                {
                    response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.CoreNotRegisteredWithProvider));
                    continue;
                }

                if (rommData.SpecialismCodes.Count() > 0)
                {
                    var specialismCodes = technicalQualification.TlSpecialismLarIds.Select(x => x.Value);
                    var invalidSpecialismCodes = rommData.SpecialismCodes.Except(specialismCodes, StringComparer.InvariantCultureIgnoreCase);

                    if (invalidSpecialismCodes.Any())
                    {
                        response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.SpecialismNotValidWithCore));
                        continue;
                    }

                    var isSpecialismPartOfCouplets = IsSpecialismPartOfCouplet(technicalQualification.TlSpecialismCombinations, rommData.SpecialismCodes);

                    if (isSpecialismPartOfCouplets)
                    {
                        if (!IsValidCouplet(technicalQualification.TlSpecialismCombinations, rommData.SpecialismCodes))
                        {
                            response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, rommData.SpecialismCodes.Count() == 1 ? ValidationMessages.SpecialismCannotBeSelectedAsSingleOption : ValidationMessages.SpecialismIsNotValid));
                            continue;
                        }
                    }
                }

                response.Add(new RommsRecordResponse
                {
                    Uln = rommData.Uln
                });
            };

            return response;
        }

        public IList<TqRegistrationProfile> TransformRommModel(IList<RommsRecordResponse> rommsData, string performedBy)
        {
            var registrationProfiles = new List<TqRegistrationProfile>();

            foreach (var (romm, index) in rommsData.Select((value, i) => (value, i)))
            {
                registrationProfiles.Add(new TqRegistrationProfile
                {
                    Id = index - Constants.RegistrationProfileStartIndex,
                    UniqueLearnerNumber = romm.Uln
                });
            }
            return registrationProfiles;
        }

        public async Task<RommsProcessResponse> ProcessRommsAsync(long AoUkprn, IList<TqRegistrationProfile> registrations, string performedBy)
        {
            RommsProcessResponse response = new();
            bool success = false;
            int processed = 0;
            IList<TqPathwayResult> results = new List<TqPathwayResult>();
            var resultsRepository = _repositoryFactory.GetRepository<TqPathwayResult>();

            var registrationProfiles = await _registrationRepository.GetRegistrationProfilesAsync(registrations);

            foreach (var profile in registrationProfiles)
            {
                var registration = await _registrationRepository.GetRegistrationLiteAsync(AoUkprn, profile.Id, false, includeOverallResults: false);

                if (registration == null || registration.Status != RegistrationPathwayStatus.Active)
                {
                    _logger.LogWarning(LogEvent.NoDataFound, $"No record found for ProfileId = {profile.Id}. Method: ProcessRommsAsync({AoUkprn}, {profile.Id})");
                    response.IsSuccess = false;
                }

                var pathwayResult = registration.TqPathwayAssessments
                    .FirstOrDefault(p => p.IsOptedin && p.EndDate is null)
                    .TqPathwayResults.FirstOrDefault();

                success = await OpenCoreRomm(pathwayResult, performedBy);

                processed++;
                results.Add(pathwayResult);
            }

            response.IsSuccess = success;

            if (response.IsValid && response.IsSuccess)
            {
                response.BulkUploadStats = new BulkUploadStats
                {
                    TotalRecordsCount = registrationProfiles.Count,
                    NewRecordsCount = 0,
                    AmendedRecordsCount = processed,
                    UnchangedRecordsCount = registrationProfiles.Count - processed
                };
            }
            return response;
        }

        private async Task<bool> OpenCoreRomm(TqPathwayResult pathwayResult, string createdBy)
        {
            var pathwayResultRepo = _repositoryFactory.GetRepository<TqPathwayResult>();

            TqPathwayResult existingPathwayResult = await pathwayResultRepo.GetFirstOrDefaultAsync(p => p.Id == pathwayResult.Id);
            if (existingPathwayResult == null)
            {
                return false;
            }

            DateTime utcNow = _systemProvider.UtcNow;

            var updated = await UpdatePathwayResultAsync(pathwayResultRepo, existingPathwayResult, createdBy);

            if (!updated)
            {
                return false;
            }


            var newPathwayResult = CreatePathwayRequest(existingPathwayResult.TlLookupId, existingPathwayResult.TqPathwayAssessmentId, PrsStatus.UnderReview, createdBy);

            bool created = await pathwayResultRepo.CreateAsync(newPathwayResult) > 0;
            if (!created)
            {
                return false;
            }

            return true;
        }

        private TqPathwayResult CreatePathwayRequest(int tlLookUpId, int TqPathwayAssessmentId, PrsStatus prsStatus, string createdBy)
        {
            DateTime utcNow = _systemProvider.UtcNow;

            return new TqPathwayResult
            {
                TqPathwayAssessmentId = TqPathwayAssessmentId,
                TlLookupId = tlLookUpId,
                PrsStatus = prsStatus,
                IsOptedin = true,
                StartDate = utcNow,
                IsBulkUpload = false,
                CreatedBy = createdBy,
                CreatedOn = utcNow
            };
        }

        private async Task<bool> UpdatePathwayResultAsync(IRepository<TqPathwayResult> pathwayResultRepo, TqPathwayResult existingPathwayResult, string createdBy)
        {
            DateTime utcNow = _systemProvider.UtcNow;

            existingPathwayResult.IsOptedin = false;
            existingPathwayResult.EndDate = utcNow;
            existingPathwayResult.ModifiedBy = createdBy;
            existingPathwayResult.ModifiedOn = utcNow;

            return await pathwayResultRepo.UpdateWithSpecifedColumnsOnlyAsync(existingPathwayResult,
                p => p.IsOptedin,
                p => p.EndDate,
                p => p.ModifiedBy,
                p => p.ModifiedOn) > 0;
        }

        private async Task<IList<TechnicalQualificationDetails>> GetAllTLevelsByAoUkprnAsync(long ukprn)
        {
            var result = await _providerRepository.GetManyAsync(p => p.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == ukprn
                                                                    && p.TqAwardingOrganisation.TlAwardingOrganisaton.IsActive
                                                                    && p.TqAwardingOrganisation.TlPathway.IsActive,
               p => p.TlProvider, p => p.TqAwardingOrganisation, p => p.TqAwardingOrganisation.TlAwardingOrganisaton,
               p => p.TqAwardingOrganisation.TlPathway, p => p.TqAwardingOrganisation.TlPathway.TlPathwaySpecialismCombinations.Where(p => p.IsActive),
               p => p.TqAwardingOrganisation.TlPathway.TlSpecialisms.Where(p => p.IsActive)).ToListAsync();

            return _mapper.Map<IList<TechnicalQualificationDetails>>(result);
        }

        private RommsRecordResponse AddStage3ValidationError(int rowNum, long uln, string errorMessage)
        {
            return new RommsRecordResponse
            {
                ValidationErrors = new List<BulkProcessValidationError>()
                {
                    new()
                    {
                        RowNum = rowNum.ToString(),
                        Uln = uln.ToString(),
                        ErrorMessage = errorMessage
                    }
                }
            };
        }

        private bool IsSpecialismPartOfCouplet(IEnumerable<KeyValuePair<int, string>> coupletPairs, IEnumerable<string> specialismCodesToRegister)
        {
            if (!coupletPairs.Any() || !specialismCodesToRegister.Any()) return false;

            var coupletSpecialismCodes = new List<string>();

            coupletPairs.Select(x => x.Value).ToList().ForEach(c => { coupletSpecialismCodes.AddRange(c.Split(Constants.PipeSeperator)); });

            var result = specialismCodesToRegister.Any(s => coupletSpecialismCodes.Any(c => c.Equals(s, StringComparison.InvariantCultureIgnoreCase)));
            return result;
        }

        private bool IsValidCouplet(IEnumerable<KeyValuePair<int, string>> coupletPairs, IEnumerable<string> specialismCodesToRegister)
        {
            if (!coupletPairs.Any() || !specialismCodesToRegister.Any()) return false;

            var coupletSpecialismCodes = coupletPairs.Select(x => x.Value).ToList();

            var hasValidSpecialismCodes = coupletSpecialismCodes.Any(cs => specialismCodesToRegister.Except(cs.Split(Constants.PipeSeperator), StringComparer.InvariantCultureIgnoreCase).Count() == 0);
            var hasValidCoupletSpecialismCodes = coupletSpecialismCodes.Any(cs => cs.Split(Constants.PipeSeperator).Except(specialismCodesToRegister, StringComparer.InvariantCultureIgnoreCase).Count() == 0);
            return hasValidSpecialismCodes && hasValidCoupletSpecialismCodes;
        }

        private static bool IsRegistrationActive(TqRegistrationProfile profile)
            => profile.TqRegistrationPathways.Any(e => e.Status == RegistrationPathwayStatus.Active);

        private static bool ValidateDateOfBirth(TqRegistrationProfile profile, IEnumerable<RommCsvRecordResponse> validRommsData)
        {
            RommCsvRecordResponse rommCsvRecord = validRommsData.FirstOrDefault(p => p.Uln == profile.UniqueLearnerNumber);
            return rommCsvRecord != null && profile.DateofBirth.Date == rommCsvRecord.DateOfBirth.Date;
        }

        private static bool ValidatePathwayResults(TqRegistrationProfile profile)
            => profile.TqRegistrationPathways.WhereActive()
                .SelectMany(rp => rp.TqPathwayAssessments.WhereActive())
                .SelectMany(pa => pa.TqPathwayResults.WhereActive())
                .Any(res => IsResult(res));

        private static bool ValidateSpecialismResults(TqRegistrationProfile profile)
            => profile.TqRegistrationPathways.WhereActive()
                .SelectMany(p => p.TqRegistrationSpecialisms.WhereActive())
                .SelectMany(p => p.TqSpecialismAssessments.WhereActive())
                .SelectMany(p => p.TqSpecialismResults.WhereActive())
                .All(res => !IsRommOrAppeal(res.PrsStatus));

        private static bool IsRommOrAppeal(PrsStatus? prsStatus)
            => prsStatus.HasValue && (prsStatus.Value == PrsStatus.UnderReview || prsStatus.Value == PrsStatus.BeingAppealed);

        private static bool IsResult(TqPathwayResult res) => res.PrsStatus is null && res.IsOptedin == true && res.EndDate is null;
    }
}