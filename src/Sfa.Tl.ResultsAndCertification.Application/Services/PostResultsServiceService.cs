using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class PostResultsServiceService : IPostResultsServiceService
    {
        public readonly IPostResultsServiceRepository _postResultsServiceRepository;

        public PostResultsServiceService(IPostResultsServiceRepository postResultsServiceRepository)
        {
            _postResultsServiceRepository = postResultsServiceRepository;
        }

        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long uln)
        {
            return await _postResultsServiceRepository.FindPrsLearnerRecordAsync(aoUkprn, uln);
        }
    }
}
