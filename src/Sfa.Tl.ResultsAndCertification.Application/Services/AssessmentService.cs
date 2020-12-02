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
using Sfa.Tl.ResultsAndCertification.Models.Assessment;
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
                if (!string.IsNullOrEmpty(assessment.CoreCode))
                {
                    var isValidCoreCode = dbRegistration.TqProvider.TqAwardingOrganisation.TlPathway.LarId.Equals(assessment.CoreCode, StringComparison.InvariantCultureIgnoreCase);
                    if (!isValidCoreCode)
                        validationErrors.Add(BuildValidationError(assessment, ValidationMessages.InvalidCoreCode));
                }

                // 4. Specialism Code is incorrect
                if (!string.IsNullOrEmpty(assessment.SpecialismCode))
                {
                    var isValidSpecialismCode = dbRegistration.TqRegistrationSpecialisms.Any(x => x.TlSpecialism.LarId.Equals(assessment.SpecialismCode, StringComparison.InvariantCultureIgnoreCase));
                    if (!isValidSpecialismCode)
                        validationErrors.Add(BuildValidationError(assessment, ValidationMessages.InvalidSpecialismCode));
                }

                // 5. Core assessment entry must be no more than 4 years after the starting academic year
                AssessmentSeries csvCoreSeries = null;
                if (!string.IsNullOrEmpty(assessment.CoreAssessmentEntry))
                {
                    csvCoreSeries = dbAssessmentSeries.FirstOrDefault(x => x.Name.Equals(assessment.CoreAssessmentEntry));
                    var isValidCoreSeries = csvCoreSeries != null && IsValidAssessmentEntry(dbRegistration.AcademicYear, csvCoreSeries.Year, AssessmentEntryType.Core);
                    if (!isValidCoreSeries)
                        validationErrors.Add(BuildValidationError(assessment, ValidationMessages.CoreEntryOutOfRange));
                }

                // 6. Specialism assessment entry must be between one and 4 years after the starting academic year
                AssessmentSeries csvSpecialismSeries = null;
                if (!string.IsNullOrEmpty(assessment.SpecialismAssessmentEntry))
                {
                    csvSpecialismSeries = dbAssessmentSeries.FirstOrDefault(x => x.Name.Equals(assessment.SpecialismAssessmentEntry));
                    var isValidAssessmentSeries = csvSpecialismSeries != null && IsValidAssessmentEntry(dbRegistration.AcademicYear, csvSpecialismSeries.Year, AssessmentEntryType.Specialism);
                    if (!isValidAssessmentSeries)
                        validationErrors.Add(BuildValidationError(assessment, ValidationMessages.SpecialismEntryOutOfRange));
                }

                if (validationErrors.Any())
                    response.Add(new AssessmentRecordResponse { ValidationErrors = validationErrors });
                else
                {
                    var registrationSpecialism = dbRegistration.TqRegistrationSpecialisms
                                                    .FirstOrDefault(x => x.TlSpecialism.LarId.Equals(assessment.SpecialismCode, StringComparison.InvariantCultureIgnoreCase));

                    response.Add(new AssessmentRecordResponse
                    {
                        TqRegistrationPathwayId = !string.IsNullOrWhiteSpace(assessment.CoreCode) ? dbRegistration?.Id : (int?)null,
                        PathwayAssessmentSeriesId = !string.IsNullOrWhiteSpace(assessment.CoreAssessmentEntry) ? csvCoreSeries?.Id : (int?)null,

                        TqRegistrationSpecialismId = !string.IsNullOrWhiteSpace(assessment.SpecialismCode) ? registrationSpecialism?.Id : (int?)null,
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

                if (assessment.TqRegistrationSpecialismId.HasValue && assessment.TqRegistrationSpecialismId.Value > 0)
                {
                    specialismAssessments.Add(new TqSpecialismAssessment
                    {
                        Id = index - Constants.SpecialismAssessmentsStartIndex,
                        TqRegistrationSpecialismId = assessment.TqRegistrationSpecialismId.Value,
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
            var newOrAmendedPathwayAssessmentRecords = await PrepareNewAndAmendedPathwayAssessments(pathwayAssessmentsToProcess);

            // Prepare Specialism Assessments
            var newOrAmendedSpecialismAssessmentRecords = await PrepareNewAndAmendedSpecialismAssessments(specialismAssessmentsToProcess);

            // Process Assessments
            response.IsSuccess = await _assessmentRepository.BulkInsertOrUpdateAssessments(newOrAmendedPathwayAssessmentRecords, newOrAmendedSpecialismAssessmentRecords);

            return response;
        }

        private async Task<List<TqPathwayAssessment>> PrepareNewAndAmendedPathwayAssessments(IList<TqPathwayAssessment> pathwayAssessmentsToProcess)
        {
            var pathwayAssessmentComparer = new TqPathwayAssessmentEqualityComparer();
            var amendedPathwayAssessments = new List<TqPathwayAssessment>();
            var newAndAmendedPathwayAssessmentRecords = new List<TqPathwayAssessment>();

            var existingPathwayAssessmentsFromDb = await _assessmentRepository.GetBulkPathwayAssessmentsAsync(pathwayAssessmentsToProcess);
            var newPathwayAssessments = pathwayAssessmentsToProcess.Except(existingPathwayAssessmentsFromDb, pathwayAssessmentComparer).ToList();
            var matchedPathwayAssessments = pathwayAssessmentsToProcess.Intersect(existingPathwayAssessmentsFromDb, pathwayAssessmentComparer).ToList();
            var unchangedPathwayAssessments = matchedPathwayAssessments.Intersect(existingPathwayAssessmentsFromDb, new TqPathwayAssessmentRecordEqualityComparer()).ToList();
            var hasAnyMatchedPathwayAssessmentsToProcess = matchedPathwayAssessments.Count != unchangedPathwayAssessments.Count;

            if (hasAnyMatchedPathwayAssessmentsToProcess)
            {
                amendedPathwayAssessments = matchedPathwayAssessments.Except(unchangedPathwayAssessments, pathwayAssessmentComparer).ToList();

                amendedPathwayAssessments.ForEach(amendedPathwayAssessment =>
                {
                    var existingPathwayAssessment = existingPathwayAssessmentsFromDb.FirstOrDefault(existingPathwayAssessment => existingPathwayAssessment.TqRegistrationPathwayId == amendedPathwayAssessment.TqRegistrationPathwayId);

                    if (existingPathwayAssessment != null)
                    {
                        var hasPathwayAssessmentChanged = amendedPathwayAssessment.AssessmentSeriesId != existingPathwayAssessment.AssessmentSeriesId;

                        if (hasPathwayAssessmentChanged)
                        {
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

            if (newPathwayAssessments.Any())
                newAndAmendedPathwayAssessmentRecords.AddRange(newPathwayAssessments.Where(p => p.TqRegistrationPathwayId > 0 && p.AssessmentSeriesId > 0));

            return newAndAmendedPathwayAssessmentRecords;
        }

        private async Task<List<TqSpecialismAssessment>> PrepareNewAndAmendedSpecialismAssessments(IList<TqSpecialismAssessment> specialismAssessmentsToProcess)
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
        
        private bool IsValidAssessmentEntry(int registrationYear, int assessmentYear, AssessmentEntryType entryType)
        {
            var startYear = entryType == AssessmentEntryType.Specialism ? Constants.SpecialismAssessmentStartInYears : Constants.CoreAssessmentStartInYears;

            var isValidRange = assessmentYear > (registrationYear + startYear) &&
                               assessmentYear <= (registrationYear + Constants.AssessmentEndInYears);

            return isValidRange;
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

            return _mapper.Map<AssessmentDetails>(tqRegistration);
        }

        public async Task<AvailableAssessmentSeries> GetAvailableAssessmentSeriesAsync(long aoUkprn, int profileId, AssessmentEntryType assessmentEntryType)
        {
            // Validate
            var tqRegistration = await _assessmentRepository.GetAssessmentsAsync(aoUkprn, profileId);
            var isValid = IsValidAddAssessmentRequestAsync(tqRegistration, assessmentEntryType);
            if (!isValid) return null;

            var startInYear = assessmentEntryType == AssessmentEntryType.Specialism ? Constants.SpecialismAssessmentStartInYears : Constants.CoreAssessmentStartInYears;
            var series = await _assessmentRepository.GetAvailableAssessmentSeriesAsync(aoUkprn, profileId, startInYear);
            if (series == null)
                return null;

            return _mapper.Map<AvailableAssessmentSeries>(series, opt => opt.Items["profileId"] = profileId);
        }

        public async Task<AddAssessmentEntryResponse> AddAssessmentEntryAsync(AddAssessmentEntryRequest request)
        {
            // Validate
            var tqRegistrationPathway = await _assessmentRepository.GetAssessmentsAsync(request.AoUkprn, request.ProfileId);
            var isValid = IsValidAddAssessmentRequestAsync(tqRegistrationPathway, request.AssessmentEntryType);
            if (!isValid)
                return new AddAssessmentEntryResponse { Status = false };

            var status = 0;
            if (request.AssessmentEntryType == AssessmentEntryType.Core)
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

            return new AddAssessmentEntryResponse { UniqueLearnerNumber = tqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber, Status = status > 0 };
        }

        public async Task<AssessmentEntryDetails> GetActivePathwayAssessmentEntryDetailsAsync(long aoUkprn, int pathwayAssessmentId)
        {
            var pathwayAssessment = await _assessmentRepository.GetPathwayAssessmentDetailsAsync(aoUkprn, pathwayAssessmentId);
            
            if (!IsValidActivePathwayAssessment(pathwayAssessment))
                return null;

            return _mapper.Map<AssessmentEntryDetails>(pathwayAssessment);
        }

        private bool IsValidActivePathwayAssessment(TqPathwayAssessment pathwayAssessment)
        {
            // 1. Must be an active registration.
            if (pathwayAssessment == null || pathwayAssessment.TqRegistrationPathway.Status != RegistrationPathwayStatus.Active)
                return false;

            // 2. Must have an active assessment.
            return pathwayAssessment.IsOptedin && pathwayAssessment.EndDate == null;
        }

        private bool IsValidAddAssessmentRequestAsync(TqRegistrationPathway registrationPathway, AssessmentEntryType assessmentEntryType)
        {
            // 1. Must be an active registration.
            if (registrationPathway == null || registrationPathway.Status != RegistrationPathwayStatus.Active)
                return false;

            // 2. Must not have an active assessment.
            var anyActiveAssessment = assessmentEntryType == AssessmentEntryType.Core ?
                        registrationPathway.TqPathwayAssessments.Any(x => x.IsOptedin && x.EndDate == null) :
                        registrationPathway.TqRegistrationSpecialisms.Any(x => x.TqSpecialismAssessments.Any(x => x.IsOptedin && x.EndDate == null));

            return !anyActiveAssessment;
        }
    }
}
