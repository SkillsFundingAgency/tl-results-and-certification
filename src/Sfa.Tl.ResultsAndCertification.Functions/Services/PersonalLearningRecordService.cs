using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class PersonalLearningRecordService : IPersonalLearningRecordService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IPersonalLearningRecordApiClient _personalLearningRecordApiClient;
        private readonly ILearnerRecordService _learnerRecordService;

        public PersonalLearningRecordService(IMapper mapper, ILogger<IPersonalLearningRecordService> logger, IPersonalLearningRecordApiClient personalLearningRecordApiClient, ILearnerRecordService learnerRecordService)
        {
            _mapper = mapper;
            _logger = logger;
            _personalLearningRecordApiClient = personalLearningRecordApiClient;
            _learnerRecordService = learnerRecordService;
        }

        public async Task ProcessLearnerVerificationAndLearningEvents()
        {
            var learners = await _learnerRecordService.GetValidRegistrationLearners();

            if(learners == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No learners found to process learner verification and learning events. Method: ProcessLearnerVerificationAndLearningEvents()");
                return;
            }

            foreach(var learner in learners)
            {
                var plrResult = await _personalLearningRecordApiClient.GetLearnerEventsAsync(learner.Uln.ToString(), learner.Firstname, learner.Lastname, learner.DateofBirth);
            }
        }
    }
}