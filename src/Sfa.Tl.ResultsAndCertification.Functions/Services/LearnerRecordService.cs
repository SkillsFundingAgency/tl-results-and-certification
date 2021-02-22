using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class LearnerRecordService : ILearnerRecordService
    {
        private readonly IMapper _mapper;
        private readonly IPersonalLearningRecordApiClient _personalLearningRecordApiClient;

        public LearnerRecordService(IMapper mapper, IPersonalLearningRecordApiClient personalLearningRecordApiClient)
        {
            _mapper = mapper;
            _personalLearningRecordApiClient = personalLearningRecordApiClient;
        }

        public bool ProcessLearnerVerificationAndLearningEvents()
        {
            return true;
        }
    }
}