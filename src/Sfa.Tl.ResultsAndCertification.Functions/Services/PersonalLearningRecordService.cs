using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class PersonalLearningRecordService : IPersonalLearningRecordService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IPersonalLearningRecordServiceApiClient _personalLearningRecordApiClient;
        private readonly ILearnerRecordService _learnerRecordService;

        public PersonalLearningRecordService(IMapper mapper, ILogger<IPersonalLearningRecordService> logger,
            IPersonalLearningRecordServiceApiClient personalLearningRecordApiClient,
            ILearnerRecordService learnerRecordService)
        {
            _mapper = mapper;
            _logger = logger;
            _personalLearningRecordApiClient = personalLearningRecordApiClient;
            _learnerRecordService = learnerRecordService;
        }

        public async Task<LearnerVerificationAndLearningEventsResponse> ProcessLearnerVerificationAndLearningEventsAsync()
        {
            var pendingLearners = await _learnerRecordService.GetPendingVerificationAndLearningEventsLearnersAsync();

            if (pendingLearners == null || !pendingLearners.Any())
            {
                var message = $"No pending learners found to process learner verification and learning events. Method: ProcessLearnerVerificationAndLearningEventsAsync()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new LearnerVerificationAndLearningEventsResponse { IsSuccess = true, Message = message };
            }

            var learnerRecordDetailsList = new List<LearnerRecordDetails>();

            foreach (var pendingLearner in pendingLearners)
            {
                var plrResult = await _personalLearningRecordApiClient.GetLearnerEventsAsync(pendingLearner);

                if (plrResult != null)
                    learnerRecordDetailsList.Add(_mapper.Map<LearnerRecordDetails>(plrResult, opt => opt.Items[Constants.LrsProfileId] = pendingLearner.ProfileId));
            }

            // process learner records
            var response = await _learnerRecordService.ProcessLearnerRecordsAsync(learnerRecordDetailsList);
            response.TotalCount = pendingLearners.Count();
            return response;
        }
    }
}