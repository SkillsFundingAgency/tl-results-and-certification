using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.ResultsAndCertification.Domain;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class WithdrawalService : IWithdrawalService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;
        private readonly ICommonService _commonService;
        private readonly IMapper _mapper;
        private readonly ILogger<WithdrawalService> _logger;

        public WithdrawalService(
            IProviderRepository providerRespository,
            IRegistrationRepository registrationRepository,
            IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository,
            ICommonService commonService,
            IMapper mapper,
            ILogger<WithdrawalService> logger)
        {
            _providerRepository = providerRespository;
            _registrationRepository = registrationRepository;
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
            _commonService = commonService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IList<WithdrawalRecordResponse>> ValidateWithdrawalLearnersAsync(long aoUkprn, IEnumerable<WithdrawalCsvRecordResponse> validWithdrawalsData)
        {
            var response = new List<WithdrawalRecordResponse>();
            int rowNum = 1;

            var registrationProfiles = await _registrationRepository.GetRegistrationProfilesAsync(validWithdrawalsData.Select(e => new TqRegistrationProfile
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

                bool isDobValid = ValidateDateOfBirth(currentRegistrationProfile, validWithdrawalsData);
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

                response.Add(new WithdrawalRecordResponse
                {
                    Uln = currentRegistrationProfile.UniqueLearnerNumber,
                    ProfileId = currentRegistrationProfile.Id
                });
            }

            return response;
        }

        public async Task<IList<WithdrawalRecordResponse>> ValidateWithdrawalTlevelsAsync(long aoUkprn, IEnumerable<WithdrawalCsvRecordResponse> validWithdrawalsData)
        {
            var response = new List<WithdrawalRecordResponse>();
            var aoProviderTlevels = await GetAllTLevelsByAoUkprnAsync(aoUkprn);
            var academicYears = await _commonService.GetAcademicYearsAsync();

            foreach (var withdrawalData in validWithdrawalsData)
            {
                var academicYear = academicYears.FirstOrDefault(x => x.Name.Equals(withdrawalData.AcademicYearName, StringComparison.InvariantCultureIgnoreCase));
                if (academicYear == null)
                {
                    response.Add(AddStage3ValidationError(withdrawalData.RowNum, withdrawalData.Uln, ValidationMessages.AcademicYearIsNotValid));
                    continue;
                }
                else
                    withdrawalData.AcademicYear = academicYear.Year;

                var isProviderRegisteredWithAwardingOrganisation = aoProviderTlevels.Any(t => t.ProviderUkprn == withdrawalData.ProviderUkprn);
                if (!isProviderRegisteredWithAwardingOrganisation)
                {
                    response.Add(AddStage3ValidationError(withdrawalData.RowNum, withdrawalData.Uln, ValidationMessages.ProviderNotRegisteredWithAo));
                    continue;
                }

                var technicalQualification = aoProviderTlevels.FirstOrDefault(tq => tq.ProviderUkprn == withdrawalData.ProviderUkprn && tq.PathwayLarId == withdrawalData.CoreCode);
                if (technicalQualification == null)
                {
                    response.Add(AddStage3ValidationError(withdrawalData.RowNum, withdrawalData.Uln, ValidationMessages.CoreNotRegisteredWithProvider));
                    continue;
                }

                if (withdrawalData.SpecialismCodes.Count() > 0)
                {
                    var specialismCodes = technicalQualification.TlSpecialismLarIds.Select(x => x.Value);
                    var invalidSpecialismCodes = withdrawalData.SpecialismCodes.Except(specialismCodes, StringComparer.InvariantCultureIgnoreCase);

                    if (invalidSpecialismCodes.Any())
                    {
                        response.Add(AddStage3ValidationError(withdrawalData.RowNum, withdrawalData.Uln, ValidationMessages.SpecialismNotValidWithCore));
                        continue;
                    }

                    var isSpecialismPartOfCouplets = IsSpecialismPartOfCouplet(technicalQualification.TlSpecialismCombinations, withdrawalData.SpecialismCodes);

                    if (isSpecialismPartOfCouplets)
                    {
                        if (!IsValidCouplet(technicalQualification.TlSpecialismCombinations, withdrawalData.SpecialismCodes))
                        {
                            response.Add(AddStage3ValidationError(withdrawalData.RowNum, withdrawalData.Uln, withdrawalData.SpecialismCodes.Count() == 1 ? ValidationMessages.SpecialismCannotBeSelectedAsSingleOption : ValidationMessages.SpecialismIsNotValid));
                            continue;
                        }
                    }
                }

                response.Add(new WithdrawalRecordResponse
                {
                    Uln = withdrawalData.Uln
                });
            };

            return response;
        }

        public IList<TqRegistrationProfile> TransformWithdrawalModel(IList<WithdrawalRecordResponse> withdrawalsData, string performedBy)
        {
            var registrationProfiles = new List<TqRegistrationProfile>();

            foreach (var (withdrawal, index) in withdrawalsData.Select((value, i) => (value, i)))
            {
                registrationProfiles.Add(new TqRegistrationProfile
                {
                    Id = index - Constants.RegistrationProfileStartIndex,
                    UniqueLearnerNumber = withdrawal.Uln
                });
            }
            return registrationProfiles;
        }

        public async Task<WithdrawalProcessResponse> ProcessWithdrawalsAsync(long AoUkprn, IList<TqRegistrationProfile> registrations, string performedBy)
        {
            WithdrawalProcessResponse response = new();
            int processed = 0;
            IList<TqRegistrationPathway> pathways = new List<TqRegistrationPathway>();

            var registrationProfiles = await _registrationRepository.GetRegistrationProfilesAsync(registrations);

            foreach (var profile in registrationProfiles)
            {
                var registration = await _registrationRepository.GetRegistrationLiteAsync(AoUkprn, profile.Id, false, includeOverallResults: false);

                if (registration == null || registration.Status != RegistrationPathwayStatus.Active)
                {
                    _logger.LogWarning(LogEvent.NoDataFound, $"No record found for ProfileId = {profile.Id}. Method: WithdrawRegistrationAsync({AoUkprn}, {profile.Id})");
                    response.IsSuccess = false;
                }

                EndRegistrationWithStatus(registration, RegistrationPathwayStatus.Withdrawn, performedBy);
                processed++;
                pathways.Add(registration);
            }

            response.IsSuccess = await _tqRegistrationPathwayRepository.UpdateManyAsync(pathways) > 0;

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

        private WithdrawalRecordResponse AddStage3ValidationError(int rowNum, long uln, string errorMessage)
        {
            return new WithdrawalRecordResponse
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

        private static void EndRegistrationWithStatus(TqRegistrationPathway pathway, RegistrationPathwayStatus status, string performedBy)
        {
            if (pathway == null)
                return;

            DateTime utcNow = DateTime.UtcNow;

            // Pathway
            pathway.Status = status;
            pathway.IsPendingWithdrawal = false;
            pathway.EndDate = utcNow;
            pathway.ModifiedBy = performedBy;
            pathway.ModifiedOn = utcNow;

            pathway.TqPathwayAssessments.Where(s => s.IsOptedin && s.EndDate == null).ToList().ForEach(pa =>
            {
                pa.EndDate = utcNow;
                pa.ModifiedBy = performedBy;
                pa.ModifiedOn = utcNow;

                pa.TqPathwayResults.Where(r => r.IsOptedin && r.EndDate == null).ToList().ForEach(pr =>
                {
                    pr.EndDate = utcNow;
                    pr.ModifiedBy = performedBy;
                    pr.ModifiedOn = utcNow;
                });
            });

            // Specialisms
            pathway.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null).ToList().ForEach(s =>
            {
                s.EndDate = utcNow;
                s.ModifiedBy = performedBy;
                s.ModifiedOn = utcNow;

                s.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.EndDate == null).ToList().ForEach(sa =>
                {
                    sa.EndDate = utcNow;
                    sa.ModifiedBy = performedBy;
                    sa.ModifiedOn = utcNow;

                    sa.TqSpecialismResults.Where(sr => sr.IsOptedin && sr.EndDate == null).ToList().ForEach(sr =>
                    {
                        sr.EndDate = utcNow;
                        sr.ModifiedBy = performedBy;
                        sr.ModifiedOn = utcNow;
                    });
                });
            });

            // Overall Results
            var overallResult = pathway.OverallResults.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);
            if (overallResult != null)
            {
                overallResult.EndDate = utcNow;
                overallResult.ModifiedBy = performedBy;
                overallResult.ModifiedOn = utcNow;
            }
        }

        private static bool IsRegistrationActive(TqRegistrationProfile profile)
            => profile.TqRegistrationPathways.Any(e => e.Status == RegistrationPathwayStatus.Active);

        private static bool ValidateDateOfBirth(TqRegistrationProfile profile, IEnumerable<WithdrawalCsvRecordResponse> validWithdrawalsData)
        {
            WithdrawalCsvRecordResponse withdrawalCsvRecord = validWithdrawalsData.FirstOrDefault(p => p.Uln == profile.UniqueLearnerNumber);
            return withdrawalCsvRecord != null && profile.DateofBirth.Date == withdrawalCsvRecord.DateOfBirth.Date;
        }

        private static bool ValidatePathwayResults(TqRegistrationProfile profile)
            => profile.TqRegistrationPathways.WhereActive()
                .SelectMany(rp => rp.TqPathwayAssessments.WhereActive())
                .SelectMany(pa => pa.TqPathwayResults.WhereActive())
                .All(res => !IsRommOrAppeal(res.PrsStatus));

        private static bool ValidateSpecialismResults(TqRegistrationProfile profile)
            => profile.TqRegistrationPathways.WhereActive()
                .SelectMany(p => p.TqRegistrationSpecialisms.WhereActive())
                .SelectMany(p => p.TqSpecialismAssessments.WhereActive())
                .SelectMany(p => p.TqSpecialismResults.WhereActive())
                .All(res => !IsRommOrAppeal(res.PrsStatus));

        private static bool IsRommOrAppeal(PrsStatus? prsStatus)
            => prsStatus.HasValue && (prsStatus.Value == PrsStatus.UnderReview || prsStatus.Value == PrsStatus.BeingAppealed);
    }
}