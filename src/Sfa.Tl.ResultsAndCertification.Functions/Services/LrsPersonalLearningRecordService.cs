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
    public class LrsPersonalLearningRecordService : ILrsPersonalLearningRecordService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ILrsPersonalLearningRecordServiceApiClient _personalLearningRecordApiClient;
        private readonly ILrsService _learnerRecordService;

        public LrsPersonalLearningRecordService(IMapper mapper, ILogger<ILrsPersonalLearningRecordService> logger,
            ILrsPersonalLearningRecordServiceApiClient personalLearningRecordApiClient,
            ILrsService learnerRecordService)
        {
            _mapper = mapper;
            _logger = logger;
            _personalLearningRecordApiClient = personalLearningRecordApiClient;
            _learnerRecordService = learnerRecordService;
        }

        public async Task<LrsLearnerVerificationAndLearningEventsResponse> ProcessLearnerVerificationAndLearningEventsAsync()
        {
            // Step 1: Get Learners pending for either 'Verification' or 'Maths status' or 'English status' 
            var pendingLearners = await _learnerRecordService.GetPendingVerificationAndLearningEventsLearnersAsync();
            if (pendingLearners == null || !pendingLearners.Any())
            {
                var message = $"No pending learners found to process learner verification and learning events. Method: ProcessLearnerVerificationAndLearningEventsAsync()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new LrsLearnerVerificationAndLearningEventsResponse { IsSuccess = true, Message = message };
            }

            // Step 2: Call LrsApi get LearnerEvents
            var learnerRecordDetailsList = new List<LrsLearnerRecordDetails>();
            foreach (var pendingLearner in pendingLearners)
            {
                var plrResult = await _personalLearningRecordApiClient.GetLearnerEventsAsync(pendingLearner);

                if (plrResult != null)
                    learnerRecordDetailsList.Add(_mapper.Map<LrsLearnerRecordDetails>(plrResult, opt => opt.Items[Constants.LrsProfileId] = pendingLearner.ProfileId));
            }

            // Step 3: Process the Api results
            var response = await _learnerRecordService.ProcessLearnerRecordsAsync(learnerRecordDetailsList);
            response.TotalCount = pendingLearners.Count;
            return response;
        }
    }
}