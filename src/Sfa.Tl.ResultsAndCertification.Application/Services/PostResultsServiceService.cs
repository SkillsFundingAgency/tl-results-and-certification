using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class PostResultsServiceService : IPostResultsServiceService
    {
        public readonly IPostResultsServiceRepository _postResultsServiceRepository;
        private readonly IMapper _mapper;

        public PostResultsServiceService(IPostResultsServiceRepository postResultsServiceRepository, IMapper mapper)
        {
            _postResultsServiceRepository = postResultsServiceRepository;
            _mapper = mapper;
        }

        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long uln)
        {
            var registrationPathway = await _postResultsServiceRepository.FindPrsLearnerRecordAsync(aoUkprn, uln);
            return _mapper.Map<FindPrsLearnerRecord>(registrationPathway);
        }
    }
}
