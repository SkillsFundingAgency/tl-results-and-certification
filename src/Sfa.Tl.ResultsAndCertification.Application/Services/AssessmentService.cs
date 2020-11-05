using AutoMapper;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IRepository<AssessmentSeries> _assessmentSeries;
        private readonly IMapper _mapper;
        private readonly ILogger<IAssessmentRepository> _logger;

        public AssessmentService(IAssessmentRepository assessmentRepository,
            IRepository<AssessmentSeries> assessmentSeries, IMapper mapper, ILogger<IAssessmentRepository> logger)
        {
            _assessmentRepository = assessmentRepository;
            _assessmentSeries = assessmentSeries;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IList<AssessmentRecordResponse>> ValidateAssessmentsAsync(long aoUkprn, IEnumerable<AssessmentCsvRecordResponse> csvAssessments)
        {
            var response = new List<AssessmentRecordResponse>();
            var dbRegistrations = await _assessmentRepository.GetBulkAssessmentsAsync(aoUkprn, csvAssessments.Select(x => x.Uln));

            foreach (var assessment in csvAssessments)
            {
                // 1. ULN not recognised with AO
                var dbRegistration = dbRegistrations.SingleOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == assessment.Uln);
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
                var isValidCoreCode = dbRegistration.TqProvider.TqAwardingOrganisation.TlPathway.LarId.Equals(assessment.CoreCode, StringComparison.InvariantCultureIgnoreCase);
                if (!isValidCoreCode)
                    validationErrors.Add(new BulkProcessValidationError { RowNum = assessment.RowNum.ToString(), Uln = assessment.Uln.ToString(), ErrorMessage = ValidationMessages.InvalidCoreCode });

                // 4. Specialism Code is incorrect
                var isValidSpecialismCode = dbRegistration.TqRegistrationSpecialisms.Any(x => x.TlSpecialism.LarId.Equals(assessment.SpecialismCode, StringComparison.InvariantCultureIgnoreCase));
                if (!isValidSpecialismCode)
                    validationErrors.Add(new BulkProcessValidationError { RowNum = assessment.RowNum.ToString(), Uln = assessment.Uln.ToString(), ErrorMessage = ValidationMessages.InvalidSpecialismCode });

                // 5. Core assessment entry must be no more than 4 years after the starting academic year
                var regYear = dbRegistration.AcademicYear;
                var csvCoreSeries = await _assessmentSeries.GetFirstOrDefaultAsync(x => x.Name.Equals(assessment.CoreAssessmentEntry));  // TODO: case check required? or it works bydefault?
                var isCoreEntryValid = csvCoreSeries?.Year > regYear && csvCoreSeries?.Year <= regYear + 2;
                if (csvCoreSeries == null && !isCoreEntryValid)
                    validationErrors.Add(new BulkProcessValidationError { RowNum = assessment.RowNum.ToString(), Uln = assessment.Uln.ToString(), ErrorMessage = ValidationMessages.CoreEntryOutOfRange });

                // 6. Specialism assessment entry must be between one and 4 years after the starting academic year
                var csvSpecialismSeries = await _assessmentSeries.GetFirstOrDefaultAsync(x => x.Name.Equals(assessment.SpecialismAssessmentEntry));
                var isSpecialismEntryValid = csvSpecialismSeries?.Year > regYear + 1 && csvSpecialismSeries?.Year <= regYear + 2;
                if (csvSpecialismSeries == null && !isSpecialismEntryValid)
                    validationErrors.Add(new BulkProcessValidationError { RowNum = assessment.RowNum.ToString(), Uln = assessment.Uln.ToString(), ErrorMessage = ValidationMessages.SpecialismEntryOutOfRange });

                if (validationErrors.Count == 0)
                {
                    var specialismAssessment = dbRegistration.TqRegistrationSpecialisms.FirstOrDefault();
                    response.Add(new AssessmentRecordResponse
                    {
                        TqRegistrationPathwayId = dbRegistration.Id,
                        PathwayAssessmentSeriesId = dbRegistration.TqPathwayAssessments.SingleOrDefault()?.AssessmentSeriesId,

                        TqRegistrationSpecialismId = specialismAssessment?.Id,
                        SpecialismAssessmentSeriesId = specialismAssessment?.TqSpecialismAssessments.SingleOrDefault()?.AssessmentSeriesId,
                    });
                }
                else
                    response.Add(new AssessmentRecordResponse { ValidationErrors = validationErrors });
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

            var existingPathwayAssessmentsFromDb = await _assessmentRepository.GetPathwayAssessmentsAsync(pathwayAssessmentsToProcess);
            //existingPathwayAssessmentsFromDb = existingPathwayAssessmentsFromDb.Where(x => x.IsOptedin && x.EndDate == null).ToList();

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

                            if (amendedPathwayAssessment.AssessmentSeriesId > 0)
                                newAndAmendedPathwayAssessmentRecords.Add(amendedPathwayAssessment);
                        }
                    }
                });
            }

            if (newPathwayAssessments.Any())
                newAndAmendedPathwayAssessmentRecords.AddRange(newPathwayAssessments.Where(p => p.TqRegistrationPathwayId != 0 && p.AssessmentSeriesId > 0));

            return newAndAmendedPathwayAssessmentRecords;
        }

        private async Task<List<TqSpecialismAssessment>> PrepareNewAndAmendedSpecialismAssessments(IList<TqSpecialismAssessment> specialismAssessmentsToProcess)
        {
            var specialismAssessmentComparer = new TqSpecialismAssessmentEqualityComparer();
            var amendedSpecialismAssessments = new List<TqSpecialismAssessment>();
            var newOrAmendedSpecialismAssessmentRecords = new List<TqSpecialismAssessment>();

            var existingSpecialismAssessmentsFromDb = await _assessmentRepository.GetSpecialismAssessmentsAsync(specialismAssessmentsToProcess);
            //existingSpecialismAssessmentsFromDb = existingSpecialismAssessmentsFromDb.Where(x => x.IsOptedin && x.EndDate == null).ToList();

            var newSpecialismAssessments = specialismAssessmentsToProcess.Except(existingSpecialismAssessmentsFromDb, specialismAssessmentComparer).ToList();
            var matchedSpecialismAssessments = specialismAssessmentsToProcess.Intersect(existingSpecialismAssessmentsFromDb, specialismAssessmentComparer).ToList();
            var unchangedSpecialismAssessments = matchedSpecialismAssessments.Intersect(existingSpecialismAssessmentsFromDb, new TqSpecialismAssessmentRecordEqualityComparer()).ToList();
            var hasAnyMatchedSpecialismAssessmentsToProcess = matchedSpecialismAssessments.Count != unchangedSpecialismAssessments.Count;

            if (hasAnyMatchedSpecialismAssessmentsToProcess)
            {
                amendedSpecialismAssessments = matchedSpecialismAssessments.Except(unchangedSpecialismAssessments, specialismAssessmentComparer).ToList();

                amendedSpecialismAssessments.ForEach(amendedSpecialismAssessment =>
                {
                    var existingSpecialismAssessment = existingSpecialismAssessmentsFromDb.SingleOrDefault(existingSpecialismAssessment => existingSpecialismAssessment.TqRegistrationSpecialismId == amendedSpecialismAssessment.TqRegistrationSpecialismId);

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

                            if (amendedSpecialismAssessment.AssessmentSeriesId > 0)
                                newOrAmendedSpecialismAssessmentRecords.Add(amendedSpecialismAssessment);
                        }
                    }
                });
            }

            if (newSpecialismAssessments.Any())
                newOrAmendedSpecialismAssessmentRecords.AddRange(newSpecialismAssessments.Where(p => p.TqRegistrationSpecialismId != 0 && p.AssessmentSeriesId > 0));

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
    }
}