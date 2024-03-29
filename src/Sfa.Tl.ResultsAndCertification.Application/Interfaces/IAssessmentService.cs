﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAssessmentService
    {
        Task<IList<AssessmentRecordResponse>> ValidateAssessmentsAsync(long aoUkprn, IEnumerable<AssessmentCsvRecordResponse> enumerable);
        (IList<TqPathwayAssessment>, IList<TqSpecialismAssessment>) TransformAssessmentsModel(IList<AssessmentRecordResponse> assessmentsData, string performedBy);
        Task<AssessmentProcessResponse> CompareAndProcessAssessmentsAsync(IList<TqPathwayAssessment> pathwayAssessments, IList<TqSpecialismAssessment> specialismAssessments);        
        Task<AvailableAssessmentSeries> GetAvailableAssessmentSeriesAsync(long aoUkprn, int profileId, ComponentType componentType, IList<int> componentIds);
        Task<AddAssessmentEntryResponse> AddAssessmentEntryAsync(AddAssessmentEntryRequest request);
        Task<AssessmentEntryDetails> GetActivePathwayAssessmentEntryDetailsAsync(long aoUkprn, int pathwayAssessmentId);
        Task<IEnumerable<AssessmentEntryDetails>> GetActiveSpecialismAssessmentEntriesAsync(long aoUkprn, IList<int> assessmentIds);
        Task<bool> RemovePathwayAssessmentEntryAsync(RemoveAssessmentEntryRequest model);
        Task<bool> RemoveSpecialismAssessmentEntryAsync(RemoveAssessmentEntryRequest model);
        Task<IList<AssessmentSeriesDetails>> GetAssessmentSeriesAsync();
    }
}
