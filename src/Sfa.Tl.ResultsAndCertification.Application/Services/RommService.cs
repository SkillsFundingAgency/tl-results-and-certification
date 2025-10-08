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

            foreach (RommCsvRecordResponse rommRecord in validRommsData)
            {
                var profile = registrationProfiles.FirstOrDefault(p => p.UniqueLearnerNumber == rommRecord.Uln);
                if (profile == null)
                {
                    response.Add(AddStage3ValidationError(rowNum, rommRecord.Uln, ValidationMessages.UlnNotRegistered));
                    continue;
                }

                bool isRegistrationActive = IsRegistrationActive(profile);
                if (!isRegistrationActive)
                {
                    response.Add(AddStage3ValidationError(rowNum, rommRecord.Uln, ValidationMessages.InactiveUln));
                    continue;
                }

                bool isDobValid = ValidateDateOfBirth(profile, validRommsData);
                if (!isDobValid)
                {
                    response.Add(AddStage3ValidationError(rowNum, rommRecord.Uln, ValidationMessages.InvalidDateOfBirth));
                    continue;
                }

                bool isLastNameValid = ValidateLastName(profile, validRommsData);
                if (!isLastNameValid)
                {
                    response.Add(AddStage3ValidationError(rowNum, rommRecord.Uln, ValidationMessages.InvalidLastName));
                    continue;
                }

                response.Add(new RommsRecordResponse
                {
                    Uln = profile.UniqueLearnerNumber,
                    ProfileId = profile.Id
                });
            }

            return response;
        }

        public async Task<IList<RommsRecordResponse>> ValidateRommTlevelsAsync(long aoUkprn, IEnumerable<RommCsvRecordResponse> validRommsData)
        {
            var response = new List<RommsRecordResponse>();
            var aoProviderTlevels = await GetAllTLevelsByAoUkprnAsync(aoUkprn);
            var academicYears = await _commonService.GetAcademicYearsAsync();
            var assessmentSeries = await _commonService.GetAssessmentSeriesAsync();
            TqSpecialismAssessment activeSpecialismAssessmentEntry = new();

            var registrationProfiles = await _registrationRepository.GetRegistrationProfilesAsync(validRommsData.Select(e => new TqRegistrationProfile
            {
                UniqueLearnerNumber = e.Uln
            }).ToList());

            foreach (var rommData in validRommsData)
            {
                var profile = registrationProfiles.FirstOrDefault(p => p.UniqueLearnerNumber == rommData.Uln);

                // 1. Academic year
                var academicYear = academicYears.FirstOrDefault(x => x.Name.Equals(rommData.AcademicYearName, StringComparison.InvariantCultureIgnoreCase));
                if (academicYear == null)
                {
                    response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.AcademicYearIsNotValid));
                    continue;
                }
                else
                    rommData.AcademicYear = academicYear.Year;

                var isAcademicYearValid = profile.TqRegistrationPathways.Any(r => r.AcademicYear == rommData.AcademicYear);
                if (!isAcademicYearValid)
                {
                    response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.AcademicYearIsNotValid));
                    continue;
                }

                // 2. Core Assessment Series
                var coreAssessmentSeries = assessmentSeries.FirstOrDefault(x => x.ComponentType == ComponentType.Core && x.SeriesName.Equals(rommData.CoreAssessmentSeries, StringComparison.InvariantCultureIgnoreCase));
                if (coreAssessmentSeries == null)
                {
                    response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.InvalidCoreAssessmentSeriesEntry));
                    continue;
                }

                // 3. Learner Active Assessments
                var activeCoreAssessmentEntry = profile.TqRegistrationPathways
                        .Where(p => p.Status == RegistrationPathwayStatus.Active && p.EndDate == null)
                        .SelectMany(p => p.TqPathwayAssessments)
                        .FirstOrDefault(a => a.IsOptedin
                            && a.EndDate is null
                            && a.AssessmentSeriesId == coreAssessmentSeries.Id);

                if (activeCoreAssessmentEntry == null)
                {
                    response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.NoCoreAssessmentEntryCurrentlyActive));
                    continue;
                }

                // 4. Validate Core RoMM Window Active
                bool isValidCoreRommWindow = _systemProvider.Today <= coreAssessmentSeries.RommEndDate;
                if (!isValidCoreRommWindow)
                {
                    response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.CoreRommWindowExpired));
                    continue;
                }

                // 5. Specialism Assessment Series
                if (!string.IsNullOrEmpty(rommData.SpecialismAssessmentSeries) && rommData.SpecialismRommOpen)
                {
                    var specialismAssessmentSeries = assessmentSeries.FirstOrDefault(x => x.ComponentType == ComponentType.Specialism && x.SeriesName.Equals(rommData.SpecialismAssessmentSeries, StringComparison.InvariantCultureIgnoreCase));
                    if (coreAssessmentSeries == null)
                    {
                        response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.InvalidSpecialismAssessmentSeriesEntry));
                        continue;
                    }

                    // 6. Learner's Active Specialism Assessments
                    activeSpecialismAssessmentEntry = profile.TqRegistrationPathways
                        .Select(rp => rp.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin && rs.EndDate == null)
                            .FirstOrDefault(rs => rs.IsOptedin && rs.EndDate is null && rs.TlSpecialism.LarId == rommData.SpecialismCode))
                        .Select(sa => sa.TqSpecialismAssessments
                            .FirstOrDefault(a => a.AssessmentSeriesId == specialismAssessmentSeries.Id && a.IsOptedin && a.EndDate is null))
                        .FirstOrDefault();

                    // 7. Active Assessment Series matches Assessment to change
                    if (activeSpecialismAssessmentEntry == null)
                    {
                        response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.NoSpecialismAssessmentEntryCurrentlyActive));
                        continue;
                    }

                    // 8. Validate Specialism RoMM Window Active
                    bool isValidSpecialismRommWindow = _systemProvider.Today <= specialismAssessmentSeries.RommEndDate;
                    if (!isValidSpecialismRommWindow)
                    {
                        response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.SpecialismRommWindowExpired));
                        continue;
                    }
                }

                // 9. Awarding Organisation
                var isProviderRegisteredWithAwardingOrganisation = aoProviderTlevels.Any(t => t.ProviderUkprn == rommData.ProviderUkprn);
                if (!isProviderRegisteredWithAwardingOrganisation)
                {
                    response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.ProviderNotRegisteredWithAo));
                    continue;
                }

                // 10. Core Code Registered with AO
                var technicalQualification = aoProviderTlevels.FirstOrDefault(tq => tq.ProviderUkprn == rommData.ProviderUkprn && tq.PathwayLarId == rommData.CoreCode);
                if (technicalQualification == null)
                {
                    response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.CoreNotRegisteredWithProvider));
                    continue;
                }

                // 11. Specialism Code REgistered
                var isSpecialismCodeProvided = !string.IsNullOrEmpty(rommData.SpecialismCode);
                if (isSpecialismCodeProvided)
                {
                    var specialismCode = technicalQualification.TlSpecialismLarIds.FirstOrDefault(x => x.Value == rommData.SpecialismCode).Value;
                    if (specialismCode == null)
                    {
                        response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.SpecialismNotValidWithCore));
                        continue;
                    }
                }

                bool openCoreRomm = rommData.CoreRommOpen;
                bool openCoreRommWithOutcome = !string.IsNullOrEmpty(rommData.CoreRommOutcome);

                bool openCoreRommOnly = openCoreRomm && !openCoreRommWithOutcome;
                bool openCoreRommAndAddOutcome = openCoreRomm && openCoreRommWithOutcome;

                if (openCoreRommOnly)
                {
                    bool arePathwayResultsValid = ValidatePathwayResultStatus<TqPathwayResult>(profile.TqRegistrationPathways, activeCoreAssessmentEntry.Id, p => !p.PrsStatus.HasValue);
                    if (!arePathwayResultsValid)
                    {
                        response.Add(AddStage3ValidationError(rommData.RowNum, profile.UniqueLearnerNumber, ValidationMessages.InvalidCoreResultState));
                        continue;
                    }
                }

                if (openCoreRommAndAddOutcome)
                {
                    var gradesLookup = await _commonService.GetLookupDataAsync(LookupCategory.PathwayComponentGrade
                        , new() { Constants.PathwayComponentGradeQpendingResultCode, Constants.PathwayComponentGradeXNoResultCode });

                    var isPathwayComponentGrade = gradesLookup.FirstOrDefault(g => g.Value.Equals(rommData.CoreRommOutcome));
                    if (isPathwayComponentGrade == null)
                    {
                        response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.InvalidCoreRommComponentGrade));
                        continue;
                    }

                    bool arePathwayResultsValid = ValidatePathwayResultStatus<TqPathwayResult>(profile.TqRegistrationPathways, activeCoreAssessmentEntry.Id, p => !p.PrsStatus.HasValue || p.PrsStatus == PrsStatus.UnderReview);
                    if (!arePathwayResultsValid)
                    {
                        response.Add(AddStage3ValidationError(rommData.RowNum, profile.UniqueLearnerNumber, ValidationMessages.InvalidCoreResultState));
                        continue;
                    }
                }

                bool openSpecialismRomm = rommData.SpecialismRommOpen && isSpecialismCodeProvided;
                bool openSpecialismRommWithOutcome = !string.IsNullOrEmpty(rommData.SpecialismRommOutcome);

                bool openSpecialismRommOnly = openSpecialismRomm && !openSpecialismRommWithOutcome;
                bool openSpecialismRommAndAddOutcome = openSpecialismRomm && openSpecialismRommWithOutcome;

                if (openSpecialismRommOnly)
                {
                    bool areSpecialismResultsValid = ValidateSpecialismResultStatus<TqSpecialismResult>(profile.TqRegistrationPathways, activeSpecialismAssessmentEntry.Id, rommData.SpecialismCode, p => !p.PrsStatus.HasValue);
                    if (!areSpecialismResultsValid)
                    {
                        response.Add(AddStage3ValidationError(rommData.RowNum, profile.UniqueLearnerNumber, ValidationMessages.InvalidSpecialismResultState));
                        continue;
                    }
                }

                if (openSpecialismRommAndAddOutcome)
                {
                    var gradesLookup = await _commonService.GetLookupDataAsync(LookupCategory.SpecialismComponentGrade,
                        new() { Constants.SpecialismComponentGradeQpendingResultCode, Constants.SpecialismComponentGradeXNoResultCode });

                    var isSpecialismComponentGrade = gradesLookup.FirstOrDefault(g => g.Value.Equals(rommData.SpecialismRommOutcome));
                    if (isSpecialismComponentGrade == null)
                    {
                        response.Add(AddStage3ValidationError(rommData.RowNum, rommData.Uln, ValidationMessages.InvalidSpecialismRommComponentGrade));
                        continue;
                    }

                    bool areSpecialismResultsValid = ValidateSpecialismResultStatus<TqSpecialismResult>(profile.TqRegistrationPathways, activeSpecialismAssessmentEntry.Id, rommData.SpecialismCode, p => !p.PrsStatus.HasValue || p.PrsStatus == PrsStatus.UnderReview);
                    if (!areSpecialismResultsValid)
                    {
                        response.Add(AddStage3ValidationError(rommData.RowNum, profile.UniqueLearnerNumber, ValidationMessages.InvalidSpecialismResultState));
                        continue;
                    }
                }

                response.Add(new RommsRecordResponse
                {
                    Uln = rommData.Uln,
                    OpenCoreRomm = openCoreRomm,
                    AddCoreRommOutcome = openCoreRommWithOutcome,
                    CoreRommOutcome = rommData.CoreRommOutcome,
                    CoreAssessmentSeriesId = activeCoreAssessmentEntry.AssessmentSeriesId,
                    OpenSpecialismRomm = openSpecialismRomm,
                    AddSpecialismRommOutcome = openSpecialismRommWithOutcome,
                    SpecialismRommOutcome = rommData.SpecialismRommOutcome,
                    SpecialismCode = rommData.SpecialismCode,
                    SpecialismAssessmentSeriesId = activeSpecialismAssessmentEntry.AssessmentSeriesId
                });
            }
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

        public async Task<RommsProcessResponse> ProcessRommsAsync(long AoUkprn, IList<TqRegistrationProfile> registrations, IEnumerable<RommsRecordResponse> rommData, string performedBy)
        {
            RommsProcessResponse response = new();
            bool success = false;
            int processed = 0;

            var registrationProfiles = await _registrationRepository.GetRegistrationProfilesAsync(registrations);

            foreach (var profile in registrationProfiles)
            {
                var registration = await _registrationRepository.GetRegistrationLiteAsync(AoUkprn, profile.Id, false, includeOverallResults: false);

                if (registration == null || registration.Status != RegistrationPathwayStatus.Active)
                {
                    _logger.LogWarning(LogEvent.NoDataFound, $"No record found for ProfileId = {profile.Id}. Method: ProcessRommsAsync({AoUkprn}, {profile.Id})");
                    response.IsSuccess = false;
                }

                var romm = rommData.FirstOrDefault(r => r.Uln == profile.UniqueLearnerNumber);
                if (romm.OpenCoreRomm)
                    success = await OpenCoreRomm(registration, performedBy, romm.CoreAssessmentSeriesId, romm.AddCoreRommOutcome, romm.CoreRommOutcome);

                if (romm.OpenSpecialismRomm)
                    success = await OpenSpecialismRomm(registration, performedBy, romm.SpecialismAssessmentSeriesId, romm.SpecialismCode, romm.AddSpecialismRommOutcome, romm.SpecialismRommOutcome);

                processed++;
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

        private async Task<bool> OpenSpecialismRomm(TqRegistrationPathway pathway, string createdBy, int assessmentSeriesId, string specilismCode, bool addOutcome = false, string grade = null)
        {
            var specialismResultRepo = _repositoryFactory.GetRepository<TqSpecialismResult>();

            var result = pathway.TqRegistrationSpecialisms
                .Where(p => p.TlSpecialism.LarId == specilismCode)
                .SelectMany(p => p.TqSpecialismAssessments.WhereActive())
                .SelectMany(p => p.TqSpecialismResults.WhereActive())
                .FirstOrDefault(p => p.TqSpecialismAssessment.AssessmentSeriesId == assessmentSeriesId);

            TqSpecialismResult existingSpecialismResult = await specialismResultRepo.GetFirstOrDefaultAsync(p => p.Id == result.Id);

            if (existingSpecialismResult.PrsStatus == PrsStatus.UnderReview)
                return await AddSpecialismRommOutcome(existingSpecialismResult.Id, grade, createdBy);

            if (existingSpecialismResult == null)
            {
                return false;
            }

            DateTime utcNow = _systemProvider.UtcNow;

            existingSpecialismResult.IsOptedin = false;
            existingSpecialismResult.EndDate = utcNow;
            existingSpecialismResult.ModifiedBy = createdBy;
            existingSpecialismResult.ModifiedOn = utcNow;

            var updated = await UpdateSpecialismResultAsync(specialismResultRepo, existingSpecialismResult, createdBy);

            if (!updated)
            {
                return false;
            }

            var newSpecialismResult = CreateSpecialismRequest(existingSpecialismResult.TlLookupId, existingSpecialismResult.TqSpecialismAssessmentId, utcNow, PrsStatus.UnderReview, createdBy);

            var newSpecialismResultId = await specialismResultRepo.CreateAsync(newSpecialismResult);

            bool created = newSpecialismResultId > 0;

            if (!created)
            {
                return false;
            }

            if (addOutcome)
                return await AddSpecialismRommOutcome(newSpecialismResultId, grade, createdBy);

            return created;
        }

        private async Task<bool> AddSpecialismRommOutcome(int specialismResultId, string rommOutcome, string createdBy)
        {
            var specialismResultRepo = _repositoryFactory.GetRepository<TqSpecialismResult>();
            var lookupRepo = _repositoryFactory.GetRepository<TlLookup>();

            var grade = await lookupRepo.GetFirstOrDefaultAsync(e => e.Value.Equals(rommOutcome)
                                    && e.Category.Equals(LookupCategory.SpecialismComponentGrade.ToString()));


            TqSpecialismResult existingSpecialismResult = await specialismResultRepo.GetFirstOrDefaultAsync(p => p.Id == specialismResultId);
            if (existingSpecialismResult == null)
            {
                return false;
            }

            DateTime utcNow = _systemProvider.UtcNow;

            var updated = await UpdateSpecialismResultAsync(specialismResultRepo, existingSpecialismResult, createdBy);

            if (!updated)
            {
                return false;
            }

            var newSpecialismResult = CreateSpecialismRequest(grade.Id, existingSpecialismResult.TqSpecialismAssessmentId, utcNow, PrsStatus.Reviewed, createdBy);

            bool created = await specialismResultRepo.CreateAsync(newSpecialismResult) > 0;

            if (!created)
            {
                return false;
            }

            return created;
        }

        private async Task<bool> OpenCoreRomm(TqRegistrationPathway pathway, string createdBy, int assessmentSeriesId, bool addOutcome = false, string grade = null)
        {
            var pathwayResultRepo = _repositoryFactory.GetRepository<TqPathwayResult>();

            var result = pathway.TqPathwayAssessments.WhereActive().FirstOrDefault(a => a.AssessmentSeriesId == assessmentSeriesId).TqPathwayResults.WhereActive().FirstOrDefault();

            TqPathwayResult existingPathwayResult = await pathwayResultRepo.GetFirstOrDefaultAsync(p => p.Id == result.Id);

            if (existingPathwayResult == null)
            {
                return false;
            }

            if (existingPathwayResult.PrsStatus == PrsStatus.UnderReview)
                return await AddCoreRommOutcome(existingPathwayResult.Id, grade, createdBy);

            DateTime utcNow = _systemProvider.UtcNow;

            var updated = await UpdatePathwayResultAsync(pathwayResultRepo, existingPathwayResult, createdBy);

            if (!updated)
            {
                return false;
            }

            var newPathwayResult = CreatePathwayRequest(existingPathwayResult.TlLookupId, existingPathwayResult.TqPathwayAssessmentId, PrsStatus.UnderReview, createdBy);

            var newPathwayResultId = await pathwayResultRepo.CreateAsync(newPathwayResult);

            bool created = (newPathwayResultId > 0);
            if (!created)
            {
                return false;
            }

            if (addOutcome)
                return await AddCoreRommOutcome(newPathwayResultId, grade, createdBy);

            return created;
        }

        private async Task<bool> AddCoreRommOutcome(int pathwayResultId, string rommOutcome, string createdBy)
        {
            var pathwayResultRepo = _repositoryFactory.GetRepository<TqPathwayResult>();
            var lookupRepo = _repositoryFactory.GetRepository<TlLookup>();

            var grade = await lookupRepo.GetFirstOrDefaultAsync(e => e.Value.Equals(rommOutcome)
                                && e.Category.Equals(LookupCategory.PathwayComponentGrade.ToString()));

            TqPathwayResult existingPathwayResult = await pathwayResultRepo.GetFirstOrDefaultAsync(p => p.Id == pathwayResultId);
            if (existingPathwayResult == null)
            {
                return false;
            }

            var updated = await UpdatePathwayResultAsync(pathwayResultRepo, existingPathwayResult, createdBy);

            if (!updated)
            {
                return false;
            }

            var newPathwayResult = CreatePathwayRequest(grade.Id, existingPathwayResult.TqPathwayAssessmentId, PrsStatus.Reviewed, createdBy);

            bool created = await pathwayResultRepo.CreateAsync(newPathwayResult) > 0;
            if (!created)
            {
                return false;
            }

            return created;
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

        private async Task<bool> OpenSpecialismRomm(TqSpecialismResult specialismResult, string createdBy)
        {
            var specialismResultRepo = _repositoryFactory.GetRepository<TqSpecialismResult>();

            TqSpecialismResult existingSpecialismResult = await specialismResultRepo.GetFirstOrDefaultAsync(p => p.Id == specialismResult.Id);
            if (existingSpecialismResult == null)
            {
                return false;
            }

            DateTime utcNow = _systemProvider.UtcNow;

            existingSpecialismResult.IsOptedin = false;
            existingSpecialismResult.EndDate = utcNow;
            existingSpecialismResult.ModifiedBy = createdBy;
            existingSpecialismResult.ModifiedOn = utcNow;

            var updated = await UpdateSpecialismResultAsync(specialismResultRepo, existingSpecialismResult, createdBy);

            if (!updated)
            {
                return false;
            }

            var newSpecialismResult = CreateSpecialismRequest(existingSpecialismResult.TlLookupId, existingSpecialismResult.TqSpecialismAssessmentId, utcNow, PrsStatus.UnderReview, createdBy);

            bool created = await specialismResultRepo.CreateAsync(newSpecialismResult) > 0;

            if (!created)
            {
                return false;
            }

            return true;
        }

        private static TqSpecialismResult CreateSpecialismRequest(int tlLookUpId, int TqSpecialismAssessmentId, DateTime utcNow, PrsStatus prsStatus, string createdBy)
        {
            return new TqSpecialismResult
            {
                TqSpecialismAssessmentId = TqSpecialismAssessmentId,
                TlLookupId = tlLookUpId,
                PrsStatus = prsStatus,
                IsOptedin = true,
                StartDate = utcNow,
                IsBulkUpload = false,
                CreatedBy = createdBy,
                CreatedOn = utcNow
            };
        }

        private async Task<bool> UpdateSpecialismResultAsync(IRepository<TqSpecialismResult> specialismResultRepo, TqSpecialismResult existingSpecialismResult, string createdBy)
        {
            DateTime utcNow = _systemProvider.UtcNow;

            existingSpecialismResult.IsOptedin = false;
            existingSpecialismResult.EndDate = utcNow;
            existingSpecialismResult.ModifiedBy = createdBy;
            existingSpecialismResult.ModifiedOn = utcNow;

            return await specialismResultRepo.UpdateWithSpecifedColumnsOnlyAsync(existingSpecialismResult,
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
               p => p.TqAwardingOrganisation.TlPathway,
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

        private static bool ValidateLastName(TqRegistrationProfile profile, IEnumerable<RommCsvRecordResponse> validRommsData)
        {
            RommCsvRecordResponse rommCsvRecord = validRommsData.FirstOrDefault(p => p.Uln == profile.UniqueLearnerNumber);
            return rommCsvRecord != null && rommCsvRecord.LastName.Equals(profile.Lastname, StringComparison.InvariantCultureIgnoreCase);
        }

        private bool ValidatePathwayResultStatus<TResult>(IEnumerable<TqRegistrationPathway> pathway, int assessmentId, Func<TqPathwayResult, bool> getValue)
        {
            var result = pathway
                    .SelectMany(p => p.TqPathwayAssessments.WhereActive())
                    .SelectMany(p => p.TqPathwayResults.WhereActive())
                .FirstOrDefault(p => p.TqPathwayAssessmentId == assessmentId);

            return result == null ? false : getValue(result);
        }

        private bool ValidateSpecialismResultStatus<TResult>(IEnumerable<TqRegistrationPathway> pathway, int assessmentId, string specialismCode, Func<TqSpecialismResult, bool> getValue)
        {
            var result = pathway
                .SelectMany(p => p.TqRegistrationSpecialisms.WhereActive())
                .Where(p => p.TlSpecialism.LarId == specialismCode)
                    .SelectMany(p => p.TqSpecialismAssessments.WhereActive())
                    .SelectMany(p => p.TqSpecialismResults.WhereActive())
                .FirstOrDefault(p => p.TqSpecialismAssessmentId == assessmentId);

            return result is null ? false : getValue(result);
        }

        private static bool ValidatePathwayResults(TqRegistrationProfile profile)
            => profile.TqRegistrationPathways.WhereActive()
                .SelectMany(rp => rp.TqPathwayAssessments.WhereActive())
                .SelectMany(pa => pa.TqPathwayResults.WhereActive())
                .Any(res => HasCoreResult(res));

        private static bool ValidateSpecialismResults(TqRegistrationProfile profile)
            => profile.TqRegistrationPathways.WhereActive()
                .SelectMany(p => p.TqRegistrationSpecialisms.WhereActive())
                .SelectMany(p => p.TqSpecialismAssessments.WhereActive())
                .SelectMany(p => p.TqSpecialismResults.WhereActive())
                .Any(res => HasSpecialismResult(res));

        private static bool HasCoreResult(TqPathwayResult res) => res.PrsStatus is null && res.IsOptedin == true && res.EndDate is null;
        private static bool HasSpecialismResult(TqSpecialismResult res) => res.PrsStatus is null && res.IsOptedin == true && res.EndDate is null;
    }
}