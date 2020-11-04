using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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
        private readonly IMapper _mapper;
        private readonly ILogger<IAssessmentRepository> _logger;

        public AssessmentService(IAssessmentRepository assessmentRepository, IMapper mapper, ILogger<IAssessmentRepository> logger)
        {
            _assessmentRepository = assessmentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> CompareAndProcessAssessmentsAsync(IList<TqPathwayAssessment> pathwayAssessmentsToProcess, IList<TqSpecialismAssessment> specialismAssessmentsToProcess)
        {
            // Prepare Pathway Assessments
            var newOrAmendedPathwayAssessmentRecords = await PrepareNewAndAmendedPathwayAssessments(pathwayAssessmentsToProcess);

            // Prepare Specialism Assessments
            var newOrAmendedSpecialismAssessmentRecords = await PrepareNewAndAmendedSpecialismAssessments(specialismAssessmentsToProcess);

            // Process Assessments
            var result = await _assessmentRepository.BulkInsertOrUpdateAssessments(newOrAmendedPathwayAssessmentRecords, newOrAmendedSpecialismAssessmentRecords);
            return result;
        }

        public async Task<IList<AssessmentCsvRecordResponse>> ValidateAssessmentsAsync(long aoUkprn, IEnumerable<AssessmentCsvRecordResponse> csvAssessments)
        {
            var response = new List<AssessmentCsvRecordResponse>();
            var dbAssessments = await _assessmentRepository.GetBulkAssessmentsAsync(aoUkprn, csvAssessments.Select(x => x.Uln));

            foreach (var assessment in csvAssessments)
            {
                // 1. ULN not recognised with AO
                var dbAssessment = dbAssessments.SingleOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == assessment.Uln);
                if (dbAssessment == null)
                {
                    response.Add(AddStage3ValidationError(assessment.RowNum, assessment.Uln, ValidationMessages.UlnNotRegistered));
                    continue;
                }

                // 2. ULN is withdrawn
                var isWithdrawn = dbAssessment.Status == RegistrationPathwayStatus.Withdrawn;
                if (isWithdrawn)
                {
                    response.Add(AddStage3ValidationError(assessment.RowNum, assessment.Uln, ValidationMessages.CannotAddAssessmentToWithdrawnRegistration));
                    continue;
                }

                // 3. Core Code is incorrect
                var isValidCoreCode = dbAssessment.TqProvider.TqAwardingOrganisation.TlPathway.Name == assessment.CoreCode;
                if (!isValidCoreCode)
                    response.Add(AddStage3ValidationError(assessment.RowNum, assessment.Uln, ValidationMessages.InvalidCoreCode));

                // 4. Specialism Code is incorrect
                var isValidSpecialismCode = dbAssessment.TqProvider.TqAwardingOrganisation.TlPathway.Name == assessment.CoreCode;
                if (!isValidSpecialismCode)
                    response.Add(AddStage3ValidationError(assessment.RowNum, assessment.Uln, ValidationMessages.InvalidSpecialismCode));

                // 5. Core assessment entry must be no more than 4 years after the starting academic year
                // 6. Specialism assessment entry must be between one and 4 years after the starting academic year
                
                // 7. Only one assessment entry allowed per component - Not sure about this
            }

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
                            newAndAmendedPathwayAssessmentRecords.Add(amendedPathwayAssessment);
                        }
                    }
                });
            }

            if (newPathwayAssessments.Any())
            {
                newAndAmendedPathwayAssessmentRecords.AddRange(newPathwayAssessments);
            }

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
                            newOrAmendedSpecialismAssessmentRecords.Add(amendedSpecialismAssessment);
                        }
                    }
                });
            }

            if (newSpecialismAssessments.Any())
            {
                newOrAmendedSpecialismAssessmentRecords.AddRange(newSpecialismAssessments);
            }

            return newOrAmendedSpecialismAssessmentRecords;
        }
        private AssessmentCsvRecordResponse AddStage3ValidationError(int rowNum, long uln, string errorMessage)
        {
            return new AssessmentCsvRecordResponse()
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
