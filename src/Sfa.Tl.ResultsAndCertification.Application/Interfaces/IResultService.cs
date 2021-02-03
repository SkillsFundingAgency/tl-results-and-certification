using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IResultService
    {
        Task<IList<ResultRecordResponse>> ValidateResultsAsync(long aoUkprn, IEnumerable<ResultCsvRecordResponse> csvResults);        
        IList<TqPathwayResult> TransformResultsModel(IList<ResultRecordResponse> resultsData, string performedBy);
        Task<ResultProcessResponse> CompareAndProcessResultsAsync(IList<TqPathwayResult> pathwayResultsToProcess);

        Task<ResultDetails> GetResultDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null);
        Task<AddResultResponse> AddResultAsync(AddResultRequest request);
        Task<UpdateResultResponse> UpdateResultAsync(UpdateResultRequest request);
    }
}