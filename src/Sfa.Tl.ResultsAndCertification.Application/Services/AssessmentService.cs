using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Comparer;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IRepository<TqPathwayAssessment> _pathwayAssessmentRepository;
        private readonly IRepository<TqSpecialismAssessment> _specialismAssessmentRepository;
        private readonly IRepository<AssessmentSeries> _assessmentSeriesRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IAssessmentRepository> _logger;

        public AssessmentService(IAssessmentRepository assessmentRepository,
            IRepository<TqPathwayAssessment> pathwayAssessmentRepository, 
            IRepository<TqSpecialismAssessment> specialismAssessmentRepository,
            IRepository<AssessmentSeries> assessmentSeries, IMapper mapper, ILogger<IAssessmentRepository> logger)
        {
            _assessmentRepository = assessmentRepository;
            _pathwayAssessmentRepository = pathwayAssessmentRepository;
            _specialismAssessmentRepository = specialismAssessmentRepository;
            _assessmentSeriesRepository = assessmentSeries;
            _mapper = mapper;
            _logger = logger;
        }

        #region Bulk Assessments

        public async Task<IList<AssessmentRecordResponse>> ValidateAssessmentsAsync(long aoUkprn, IEnumerable<AssessmentCsvRecordResponse> csvAssessments)
        {
            var response = new List<AssessmentRecordResponse>();
            var dbRegistrations = await _assessmentRepository.GetBulkAssessmentsAsync(aoUkprn, csvAssessments.Select(x => x.Uln));
            var dbAssessmentSeries = await _assessmentSeriesRepository.GetManyAsync().ToListAsync();

            foreach (var assessment in csvAssessments)
            {
                // 1. ULN not recognised with AO
                var dbRegistration = dbRegistrations.FirstOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == assessment.Uln);
                if (dbRegistration == null)
                {
                    response.Add(AddStage3ValidationError(assessment.RowNum, assessment.Uln, ValidationMessages.UlnNotRegistered));
                    continue;
                }

                // 2. ULN is withdrawn
                var isWithdrawn = dbRegistration.Status == RegistrationPathwayStatus.Withdrawn;
                if (isWithdrawn)
                {
                    response.Add(AddStage3ValidationError(assessment.RowNum, assessment.Uln, ValidationMessages.CannotAddAssessmentToWithdrawnRegistration));
                    continue;
                }

                var validationErrors = new List<BulkProcessValidationError>();
                // 3. Core Code is incorrect
                if (!string.IsNullOrWhiteSpace(assessment.CoreCode))
                {
                    var isValidCoreCode = dbRegistration.TqProvider.TqAwardingOrganisation.TlPathway.LarId.Equals(assessment.CoreCode, StringComparison.InvariantCultureIgnoreCase);
                    if (!isValidCoreCode)
                        validationErrors.Add(BuildValidationError(assessment, ValidationMessages.InvalidCoreCode));
                }

                // 4. Specialism Code is incorrect
                if (assessment.SpecialismCodes.Any())
                {
                    var registeredSpecialismCodes = dbRegistration.TqRegistrationSpecialisms.Select(x => x.TlSpecialism.LarId);
                    var invalidSpecialismCodes = assessment.SpecialismCodes.Except(registeredSpecialismCodes, StringComparer.InvariantCultureIgnoreCase);

                    if (invalidSpecialismCodes.Any())
                    {
                        validationErrors.Add(BuildValidationError(assessment, ValidationMessages.InvalidSpecialismCode));
                    }
                    else
                    {
                        var registeredAssessments = dbRegistration.TqRegistrationSpecialisms.SelectMany(x => x.TqSpecialismAssessments.Where(x => x.EndDate == null && x.IsOptedin)).Select(x => x.AssessmentSeriesId);
                        var availableAssessmentSeries = GetValidAssessmentSeries(dbAssessmentSeries, dbRegistration, ComponentType.Specialism);

                        if (availableAssessmentSeries != null)
                        {
                            var isResit = registeredAssessments.Except(availableAssessmentSeries.Select(x => x.Id)).Any();

                            if (!isResit && assessment.SpecialismCodes.Count < registeredSpecialismCodes.Count())
                            {
                                validationErrors.Add(BuildValidationError(assessment, ValidationMessages.SpecialismCodeMustBePair));
                            }
                        }
                    }
                }

                // 5. Core assessment entry must be no more than 4 years after the starting academic year
                if (!string.IsNullOrWhiteSpace(assessment.CoreAssessmentEntry))
                {
                    var isSeriesFound = dbAssessmentSeries.Any(x => x.Name.Equals(assessment.CoreAssessmentEntry, StringComparison.InvariantCultureIgnoreCase));
                    if (!isSeriesFound)
                        validationErrors.Add(BuildValidationError(assessment, ValidationMessages.InvalidCoreAssessmentEntry));
                    else
                    {
                        var isValidNextAssessmentSeries = IsValidNextAssessmentSeries(dbAssessmentSeries, dbRegistration.AcademicYear, assessment.CoreAssessmentEntry, Constants.CoreAssessmentStartInYears, ComponentType.Core);
                        if (!isValidNextAssessmentSeries)
                            validationErrors.Add(BuildValidationError(assessment, ValidationMessages.InvalidNextCoreAssessmentEntry));
                    }
                }

                // 6. Specialism assessment entry must be between one and 4 years after the starting academic year
                if (!string.IsNullOrWhiteSpace(assessment.SpecialismAssessmentEntry))
                {
                    var isSeriesFound = dbAssessmentSeries.Any(x => x.Name.Equals(assessment.SpecialismAssessmentEntry, StringComparison.InvariantCultureIgnoreCase));
                    if (!isSeriesFound)
                        validationErrors.Add(BuildValidationError(assessment, ValidationMessages.InvalidSpecialismAssessmentEntry));
                    else
                    {
                        var isValidNextAssessmentSeries = IsValidNextAssessmentSeries(dbAssessmentSeries, dbRegistration.AcademicYear, assessment.SpecialismAssessmentEntry, Constants.SpecialismAssessmentStartInYears, ComponentType.Specialism);
                        if (!isValidNextAssessmentSeries)
                            validationErrors.Add(BuildValidationError(assessment, ValidationMessages.InvalidNextSpecialismAssessmentEntry));
                    }
                }

                if (validationErrors.Any())
                    response.Add(new AssessmentRecordResponse { ValidationErrors = validationErrors });
                else
                {
                    var registrationSpecialisms = dbRegistration.TqRegistrationSpecialisms.Where(x => assessment.SpecialismCodes.Any(sc => sc.Equals(x.TlSpecialism.LarId, StringComparison.InvariantCultureIgnoreCase)));

                    var csvCoreSeries = dbAssessmentSeries.FirstOrDefault(x => x.Name.Equals(assessment.CoreAssessmentEntry, StringComparison.InvariantCultureIgnoreCase));
                    var csvSpecialismSeries = dbAssessmentSeries.FirstOrDefault(x => x.Name.Equals(assessment.SpecialismAssessmentEntry, StringComparison.InvariantCultureIgnoreCase));

                    response.Add(new AssessmentRecordResponse
                    {
                        TqRegistrationPathwayId = !string.IsNullOrWhiteSpace(assessment.CoreCode) ? dbRegistration?.Id : (int?)null,
                        PathwayAssessmentSeriesId = !string.IsNullOrWhiteSpace(assessment.CoreAssessmentEntry) ? csvCoreSeries?.Id : (int?)null,

                        TqRegistrationSpecialismIds = registrationSpecialisms.Select(x => x.TlSpecialismId),
                        SpecialismAssessmentSeriesId = !string.IsNullOrWhiteSpace(assessment.SpecialismAssessmentEntry) ? csvSpecialismSeries?.Id : (int?)null,
                    });
                }
            }
            
            return response;
        }
        
        public (IList<TqPathwayAssessment>, IList<TqSpecialismAssessment>) TransformAssessmentsModel(IList<AssessmentRecordResponse> assessmentsData, string performedBy)
        {
            var pathwayAssessments = new List<TqPathwayAssessment>();
            var specialismAssessments = new List<TqSpecialismAssessment>();

            foreach (var (assessment, index) in assessmentsData.Select((value, i) => (value, i)))
            {
                if (assessment.TqRegistrationPathwayId.HasValue && assessment.TqRegistrationPathwayId.Value > 0)
                {
                    pathwayAssessments.Add(new TqPathwayAssessment
                    {
                        Id = index - Constants.PathwayAssessmentsStartIndex,
                        TqRegistrationPathwayId = assessment.TqRegistrationPathwayId.Value,
                        AssessmentSeriesId = assessment.PathwayAssessmentSeriesId ?? 0,
                        StartDate = DateTime.UtcNow,
                        IsOptedin = true,
                        IsBulkUpload = true,
                        CreatedBy = performedBy,
                        CreatedOn = DateTime.UtcNow
                    });
                }

                foreach (var specialismId in assessment.TqRegistrationSpecialismIds)
                {
                    specialismAssessments.Add(new TqSpecialismAssessment
                    {
                        Id = index - Constants.SpecialismAssessmentsStartIndex,
                        TqRegistrationSpecialismId = specialismId,
                        AssessmentSeriesId = assessment.SpecialismAssessmentSeriesId ?? 0,
                        StartDate = DateTime.UtcNow,
                        IsOptedin = true,
                        IsBulkUpload = true,
                        CreatedBy = performedBy,
                        CreatedOn = DateTime.UtcNow
                    });
                }
            }
            return (pathwayAssessments, specialismAssessments);
        }

        public async Task<AssessmentProcessResponse> CompareAndProcessAssessmentsAsync(IList<TqPathwayAssessment> pathwayAssessmentsToProcess, IList<TqSpecialismAssessment> specialismAssessmentsToProcess)
        {
            var response = new AssessmentProcessResponse();

            // Prepare Pathway Assessments
            var newOrAmendedPathwayAssessmentRecords = await PrepareNewAndAmendedPathwayAssessments(pathwayAssessmentsToProcess, response);

            // Prepare Specialism Assessments
            var newOrAmendedSpecialismAssessmentRecords = await PrepareNewAndAmendedSpecialismAssessments(specialismAssessmentsToProcess, response);

            // Process Assessments
            if (response.IsValid)
                response.IsSuccess = await _assessmentRepository.BulkInsertOrUpdateAssessments(newOrAmendedPathwayAssessmentRecords, newOrAmendedSpecialismAssessmentRecords);

            return response;
        }

        private async Task<List<TqPathwayAssessment>> PrepareNewAndAmendedPathwayAssessments(IList<TqPathwayAssessment> pathwayAssessmentsToProcess, AssessmentProcessResponse response)
        {
            var pathwayAssessmentComparer = new TqPathwayAssessmentEqualityComparer();
            var amendedPathwayAssessments = new List<TqPathwayAssessment>();
            var newAndAmendedPathwayAssessmentRecords = new List<TqPathwayAssessment>();

            var existingPathwayAssessmentsFromDb = await _assessmentRepository.GetBulkPathwayAssessmentsAsync(pathwayAssessmentsToProcess);
            var newPathwayAssessments = pathwayAssessmentsToProcess.Except(existingPathwayAssessmentsFromDb, pathwayAssessmentComparer).ToList();
            var matchedPathwayAssessments = pathwayAssessmentsToProcess.Intersect(existingPathwayAssessmentsFromDb, pathwayAssessmentComparer).ToList();
            var unchangedPathwayAssessments = matchedPathwayAssessments.Intersect(existingPathwayAssessmentsFromDb, new TqPathwayAssessmentRecordEqualityComparer()).ToList();
            var hasAnyMatchedPathwayAssessmentsToProcess = matchedPathwayAssessments.Count != unchangedPathwayAssessments.Count;

            // Stage 4 Rule For new assessment entry: To add new Assessment entry previous assessment entry should have result
            if (newPathwayAssessments.Any())
            {
                newPathwayAssessments.ForEach(newPathwayAssessment =>
                {
                    var existingPathwayAssessment = GetExistingPathwayAssessment(existingPathwayAssessmentsFromDb, newPathwayAssessment);

                    if (existingPathwayAssessment != null)
                    {
                        var hasResultForExistingAssessment = existingPathwayAssessment.TqPathwayResults.Any(x => x.EndDate == null && x.IsOptedin);
                        if (!hasResultForExistingAssessment)
                            response.ValidationErrors.Add(GetAssessmentValidationError(existingPathwayAssessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber, ValidationMessages.AssessmentEntryCannotBeAddedUntilResultRecordedForExistingEntry));
                    }
                });
            }

            if (hasAnyMatchedPathwayAssessmentsToProcess)
            {
                amendedPathwayAssessments = matchedPathwayAssessments.Except(unchangedPathwayAssessments, pathwayAssessmentComparer).ToList();

                amendedPathwayAssessments.ForEach(amendedPathwayAssessment =>
                {
                    var existingPathwayAssessment = GetExistingPathwayAssessment(existingPathwayAssessmentsFromDb, amendedPathwayAssessment);

                    if (existingPathwayAssessment != null)
                    {
                        var hasPathwayAssessmentChanged = amendedPathwayAssessment.AssessmentSeriesId != existingPathwayAssessment.AssessmentSeriesId;

                        if (hasPathwayAssessmentChanged)
                        {
                            response = ValidateStage4Rules(existingPathwayAssessment, amendedPathwayAssessment, response);
                            if (!response.IsValid)
                                return;

                            existingPathwayAssessment.IsOptedin = false;
                            existingPathwayAssessment.EndDate = DateTime.UtcNow;
                            existingPathwayAssessment.ModifiedBy = amendedPathwayAssessment.CreatedBy;
                            existingPathwayAssessment.ModifiedOn = DateTime.UtcNow;

                            newAndAmendedPathwayAssessmentRecords.Add(existingPathwayAssessment);

                            if (amendedPathwayAssessment.TqRegistrationPathwayId > 0 && amendedPathwayAssessment.AssessmentSeriesId > 0)
                                newAndAmendedPathwayAssessmentRecords.Add(amendedPathwayAssessment);
                        }
                    }
                });
            }

            if (response.IsValid)
            {
                if (newPathwayAssessments.Any())
                    newAndAmendedPathwayAssessmentRecords.AddRange(newPathwayAssessments.Where(p => p.TqRegistrationPathwayId > 0 && p.AssessmentSeriesId > 0));
            }

            return newAndAmendedPathwayAssessmentRecords;
        }

        private static TqPathwayAssessment GetExistingPathwayAssessment(IList<TqPathwayAssessment> existingPathwayAssessmentsFromDb, TqPathwayAssessment amendedOrNewPathwayAssessment)
        {
            return existingPathwayAssessmentsFromDb.OrderByDescending(x => x.AssessmentSeries.StartDate)
                                                   .FirstOrDefault(existingPathwayAssessment => existingPathwayAssessment.TqRegistrationPathwayId == amendedOrNewPathwayAssessment.TqRegistrationPathwayId);
        }

        private AssessmentProcessResponse ValidateStage4Rules(TqPathwayAssessment existingPathwayAssessment, TqPathwayAssessment amendedOrNewPathwayAssessment, AssessmentProcessResponse response)
        {
            // Rule: Assessment entry can not be removed when results are associated to it. 
            var hasResult = existingPathwayAssessment.TqPathwayResults.Any(x => x.EndDate == null && x.IsOptedin);
            if (hasResult && amendedOrNewPathwayAssessment.AssessmentSeriesId == 0)
                response.ValidationErrors.Add(GetAssessmentValidationError(existingPathwayAssessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber, ValidationMessages.AssessmentEntryCannotBeRemovedHasResult));

            return response;
        }

        private static BulkProcessValidationError GetAssessmentValidationError(long uln, string errorMessage)
        {
            return new BulkProcessValidationError
            {
                Uln = uln.ToString(),
                ErrorMessage = errorMessage
            };
        }

        private async Task<List<TqSpecialismAssessment>> PrepareNewAndAmendedSpecialismAssessments(IList<TqSpecialismAssessment> specialismAssessmentsToProcess, AssessmentProcessResponse response)
        {
            var specialismAssessmentComparer = new TqSpecialismAssessmentEqualityComparer();
            var amendedSpecialismAssessments = new List<TqSpecialismAssessment>();
            var newOrAmendedSpecialismAssessmentRecords = new List<TqSpecialismAssessment>();

            var existingSpecialismAssessmentsFromDb = await _assessmentRepository.GetBulkSpecialismAssessmentsAsync(specialismAssessmentsToProcess);
            var newSpecialismAssessments = specialismAssessmentsToProcess.Except(existingSpecialismAssessmentsFromDb, specialismAssessmentComparer).ToList();
            var matchedSpecialismAssessments = specialismAssessmentsToProcess.Intersect(existingSpecialismAssessmentsFromDb, specialismAssessmentComparer).ToList();
            var unchangedSpecialismAssessments = matchedSpecialismAssessments.Intersect(existingSpecialismAssessmentsFromDb, new TqSpecialismAssessmentRecordEqualityComparer()).ToList();
            var hasAnyMatchedSpecialismAssessmentsToProcess = matchedSpecialismAssessments.Count != unchangedSpecialismAssessments.Count;

            if (hasAnyMatchedSpecialismAssessmentsToProcess)
            {
                amendedSpecialismAssessments = matchedSpecialismAssessments.Except(unchangedSpecialismAssessments, specialismAssessmentComparer).ToList();

                amendedSpecialismAssessments.ForEach(amendedSpecialismAssessment =>
                {
                    var existingSpecialismAssessment = existingSpecialismAssessmentsFromDb.FirstOrDefault(existingSpecialismAssessment => existingSpecialismAssessment.TqRegistrationSpecialismId == amendedSpecialismAssessment.TqRegistrationSpecialismId);

                    if (existingSpecialismAssessment != null)
                    {
                        var hasSpecialismAssessmentChanged = amendedSpecialismAssessment.AssessmentSeriesId != existingSpecialismAssessment.AssessmentSeriesId;

                        if (hasSpecialismAssessmentChanged)
                        {
                            existingSpecialismAssessment.IsOptedin = false;
                            existingSpecialismAssessment.EndDate = DateTime.UtcNow;
                            existingSpecialismAssessment.ModifiedBy = amendedSpecialismAssessment.CreatedBy;
                            existingSpecialismAssessment.ModifiedOn = DateTime.UtcNow;

                            newOrAmendedSpecialismAssessmentRecords.Add(existingSpecialismAssessment);

                            if (amendedSpecialismAssessment.TqRegistrationSpecialismId > 0 && amendedSpecialismAssessment.AssessmentSeriesId > 0)
                                newOrAmendedSpecialismAssessmentRecords.Add(amendedSpecialismAssessment);
                        }
                    }
                });
            }

            if (newSpecialismAssessments.Any())
                newOrAmendedSpecialismAssessmentRecords.AddRange(newSpecialismAssessments.Where(p => p.TqRegistrationSpecialismId > 0 && p.AssessmentSeriesId > 0));

            return newOrAmendedSpecialismAssessmentRecords;
        }

        private AssessmentRecordResponse AddStage3ValidationError(int rowNum, long uln, string errorMessage)
        {
            return new AssessmentRecordResponse()
            {
                ValidationErrors = new List<BulkProcessValidationError>()
                {
                    new BulkProcessValidationError
                    {
                        RowNum = rowNum.ToString(),
                        Uln = uln.ToString(),
                        ErrorMessage = errorMessage
                    }
                }
            };
        }
        
        private BulkProcessValidationError BuildValidationError(AssessmentCsvRecordResponse assessment, string message)
        {
            return new BulkProcessValidationError { RowNum = assessment.RowNum.ToString(), Uln = assessment.Uln.ToString(), ErrorMessage = message };
        }

        #endregion

        public async Task<AssessmentDetails> GetAssessmentDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var tqRegistration = await _assessmentRepository.GetAssessmentsAsync(aoUkprn, profileId);
            if (tqRegistration == null || (status != null && tqRegistration.Status != status)) return null;

            var assessmentDetails = _mapper.Map<AssessmentDetails>(tqRegistration);

            //var assessmentSeriesOld = await _assessmentRepository.GetAvailableAssessmentSeriesAsync(aoUkprn, profileId, Constants.CoreAssessmentStartInYears);

            var allAssessmentSeries = await GetAllAssessmentSeriesAsync();
            var coreAssessmentSeries = GetValidAssessmentSeries(allAssessmentSeries, tqRegistration, ComponentType.Core);
            assessmentDetails.IsCoreEntryEligible = tqRegistration.Status == RegistrationPathwayStatus.Active && coreAssessmentSeries != null && coreAssessmentSeries.Any();
            assessmentDetails.NextAvailableCoreSeries = GetNextAvailableAssessmentSeries(allAssessmentSeries, tqRegistration, ComponentType.Core)?.Name;

            var specialismAssessmentSeries = GetValidAssessmentSeries(allAssessmentSeries, tqRegistration, ComponentType.Specialism);
            assessmentDetails.IsSpecialismEntryEligible = tqRegistration.Status == RegistrationPathwayStatus.Active && specialismAssessmentSeries != null && specialismAssessmentSeries.Any();
            assessmentDetails.NextAvailableSpecialismSeries = GetNextAvailableAssessmentSeries(allAssessmentSeries, tqRegistration, ComponentType.Specialism)?.Name;
            return assessmentDetails;
        }

        public async Task<AvailableAssessmentSeries> GetAvailableAssessmentSeriesAsync(long aoUkprn, int profileId, ComponentType componentType)
        {
            // Validate
            var tqRegistration = await _assessmentRepository.GetAssessmentsAsync(aoUkprn, profileId);
            
            var startInYear = componentType == ComponentType.Specialism ? Constants.SpecialismAssessmentStartInYears : Constants.CoreAssessmentStartInYears;
            var assessmentSeries = await _assessmentRepository.GetAvailableAssessmentSeriesAsync(aoUkprn, profileId, startInYear);

            var currentOpenSeries = assessmentSeries?.FirstOrDefault(a => a.ComponentType == componentType);
            if (currentOpenSeries == null)
                return null;

            var isValid = IsValidAddAssessmentRequestAsync(tqRegistration, currentOpenSeries.Id, componentType);
            if (!isValid) return null;

            return _mapper.Map<AvailableAssessmentSeries>(currentOpenSeries, opt => opt.Items["profileId"] = profileId);
        }

        public async Task<AddAssessmentEntryResponse> AddAssessmentEntryAsync(AddAssessmentEntryRequest request)
        {
            // Validate
            var tqRegistrationPathway = await _assessmentRepository.GetAssessmentsAsync(request.AoUkprn, request.ProfileId);

            var startInYear = request.ComponentType == ComponentType.Specialism ? Constants.SpecialismAssessmentStartInYears : Constants.CoreAssessmentStartInYears;
            var assessmentSeries = await _assessmentRepository.GetAvailableAssessmentSeriesAsync(request.AoUkprn, request.ProfileId, startInYear);
            var currrentOpenSeries = assessmentSeries?.FirstOrDefault(a => a.ComponentType == request.ComponentType);

            if (currrentOpenSeries == null)
                return new AddAssessmentEntryResponse { IsSuccess = false };

            var isValid = IsValidAddAssessmentRequestAsync(tqRegistrationPathway, currrentOpenSeries.Id, request.ComponentType);
            if (!isValid)
                return new AddAssessmentEntryResponse { IsSuccess = false };

            int status = 0;
            if (request.ComponentType == ComponentType.Core)
            {
                status = await _pathwayAssessmentRepository.CreateAsync(new TqPathwayAssessment
                {
                    TqRegistrationPathwayId = tqRegistrationPathway.Id,
                    AssessmentSeriesId = request.AssessmentSeriesId,
                    IsOptedin = true,
                    StartDate = DateTime.UtcNow,
                    EndDate = null,
                    IsBulkUpload = false,
                    CreatedBy = request.PerformedBy
                });
            }
            else if (request.ComponentType == ComponentType.Specialism)
            {
                var specialismAssessmentsToAdd = new List<TqSpecialismAssessment>();

                foreach(var specialismId in request.SpecialismIds)
                {
                    specialismAssessmentsToAdd.Add(new TqSpecialismAssessment
                    {
                        TqRegistrationSpecialismId = specialismId.Value,
                        AssessmentSeriesId = request.AssessmentSeriesId,
                        IsOptedin = true,
                        StartDate = DateTime.UtcNow,
                        EndDate = null,
                        IsBulkUpload = false,
                        CreatedBy = request.PerformedBy
                    });
                }

                if(specialismAssessmentsToAdd.Any())
                status = await _specialismAssessmentRepository.CreateManyAsync(specialismAssessmentsToAdd);
            }

            return new AddAssessmentEntryResponse { Uln = tqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber, IsSuccess = status > 0 };
        }

        public async Task<AssessmentEntryDetails> GetActivePathwayAssessmentEntryDetailsAsync(long aoUkprn, int pathwayAssessmentId)
        {
            var pathwayAssessment = await _assessmentRepository.GetPathwayAssessmentDetailsAsync(aoUkprn, pathwayAssessmentId);
            
            if (!IsValidActivePathwayAssessment(pathwayAssessment))
                return null;

            return _mapper.Map<AssessmentEntryDetails>(pathwayAssessment);
        }

        public async Task<bool> RemovePathwayAssessmentEntryAsync(RemoveAssessmentEntryRequest model)
        {
            var pathwayAssessment = await _pathwayAssessmentRepository.GetFirstOrDefaultAsync(pa => pa.Id == model.AssessmentId && pa.IsOptedin
                                                                                              && pa.EndDate == null && pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                                                                                              && pa.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == model.AoUkprn
                                                                                              && !pa.TqPathwayResults.Any(x => x.IsOptedin && x.EndDate == null));
            if (pathwayAssessment == null) return false;

            pathwayAssessment.IsOptedin = false;
            pathwayAssessment.EndDate = DateTime.UtcNow;
            pathwayAssessment.ModifiedOn = DateTime.UtcNow;
            pathwayAssessment.ModifiedBy = model.PerformedBy;

            return await _pathwayAssessmentRepository.UpdateAsync(pathwayAssessment) > 0;
        }

        public async Task<IList<AssessmentSeriesDetails>> GetAssessmentSeriesAsync()
        {
            var assessmentSeries =  await _assessmentSeriesRepository.GetManyAsync().ToListAsync();
            return _mapper.Map<IList<AssessmentSeriesDetails>>(assessmentSeries);
        }

        private bool IsValidActivePathwayAssessment(TqPathwayAssessment pathwayAssessment)
        {
            // 1. Must be an active registration.
            if (pathwayAssessment == null || pathwayAssessment.TqRegistrationPathway.Status != RegistrationPathwayStatus.Active)
                return false;

            // 2. Must have an active assessment.
            var isActiveAssessment = pathwayAssessment.IsOptedin && pathwayAssessment.EndDate == null;
            if (!isActiveAssessment)
                return false;

            // 3. Must not have results
            var hasActiveResult = pathwayAssessment.TqPathwayResults.Any(x => x.IsOptedin && x.EndDate == null);
            return !hasActiveResult;
        }

        private bool IsValidAddAssessmentRequestAsync(TqRegistrationPathway registrationPathway, int currentOpenSeriesId, ComponentType componentType)
        {
            // 1. Must be an active registration.
            if (registrationPathway == null || registrationPathway.Status != RegistrationPathwayStatus.Active)
                return false;

            // 2. Must not have an active assessment.
            var anyActiveAssessment = componentType == ComponentType.Core ?
                        registrationPathway.TqPathwayAssessments.Any(x => x.IsOptedin && x.EndDate == null && x.AssessmentSeriesId == currentOpenSeriesId) :
                        registrationPathway.TqRegistrationSpecialisms.Any(x => x.TqSpecialismAssessments.Any(x => x.IsOptedin && x.EndDate == null && x.AssessmentSeriesId == currentOpenSeriesId));

            return !anyActiveAssessment;
        }

        private static bool IsValidNextAssessmentSeries(IList<AssessmentSeries> dbAssessmentSeries, int regAcademicYear, string assessmentEntryName, int startYearOffset, ComponentType componentType)
        {
            var currentDate = DateTime.UtcNow.Date;

            var isValidNextAssessmentSeries = dbAssessmentSeries.Any(s => s.ComponentType == componentType &&
                s.Name.Equals(assessmentEntryName, StringComparison.InvariantCultureIgnoreCase) &&
                currentDate >= s.StartDate && currentDate <= s.EndDate &&
                s.Year > regAcademicYear + startYearOffset && s.Year <= regAcademicYear + Constants.AssessmentEndInYears);

            return isValidNextAssessmentSeries;
        }

        private async Task<IList<AssessmentSeries>> GetAllAssessmentSeriesAsync()
        {
            return await _assessmentSeriesRepository.GetManyAsync().ToListAsync();
        }

        public IList<AssessmentSeries> GetValidAssessmentSeries(IList<AssessmentSeries> assessmentSeries, TqRegistrationPathway tqRegistrationPathway, ComponentType componentType)
        {
            var currentDate = DateTime.UtcNow.Date;
            var startInYear = componentType == ComponentType.Specialism ? Constants.SpecialismAssessmentStartInYears : Constants.CoreAssessmentStartInYears;

            var series = assessmentSeries?.Where(s => s.ComponentType == componentType && s.Year > tqRegistrationPathway.AcademicYear + startInYear &&
                                        s.Year <= tqRegistrationPathway.AcademicYear + Constants.AssessmentEndInYears &&
                                        currentDate >= s.StartDate && currentDate <= s.EndDate)?.OrderBy(a => a.Id)?.ToList();

            return series;
        }

        public AssessmentSeries GetNextAvailableAssessmentSeries(IList<AssessmentSeries> assessmentSeries, TqRegistrationPathway tqRegistrationPathway, ComponentType componentType)
        {
            var startInYear = componentType == ComponentType.Specialism ? Constants.SpecialismAssessmentStartInYears : Constants.CoreAssessmentStartInYears;
            var series = assessmentSeries?.OrderBy(a => a.Id)?.FirstOrDefault(s => s.ComponentType == componentType && s.Year > tqRegistrationPathway.AcademicYear + startInYear &&
                                        s.Year <= tqRegistrationPathway.AcademicYear + Constants.AssessmentEndInYears && DateTime.UtcNow.Date <= s.EndDate);
            return series;
        }
    }
}
