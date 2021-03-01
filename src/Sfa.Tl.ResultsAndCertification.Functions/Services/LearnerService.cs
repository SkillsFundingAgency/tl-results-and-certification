using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Services
{
    public class LearnerService : ILearnerService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ILearnerServiceApiClient _learnerServiceApiClient;
        private readonly ILearnerRecordService _learnerRecordService;

        public LearnerService(IMapper mapper, ILogger<IPersonalLearningRecordService> logger, ILearnerServiceApiClient learnerServiceApiClient, ILearnerRecordService learnerRecordService)
        {
            _mapper = mapper;
            _logger = logger;
            _learnerServiceApiClient = learnerServiceApiClient;
            _learnerRecordService = learnerRecordService;
        }

        public async Task<LearnerGenderResponse> FetchLearnerGenderAsync()
        {
            var pendingGenderLearners = await _learnerRecordService.GetPendingGenderLearnersAsync();

            if (pendingGenderLearners == null)
            {
                var message = $"No pending learners found to process gender. Method: FetchLearnerGenderAsync()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new LearnerGenderResponse { IsSuccess = true, Message = message };
            }

            var learnerRecordDetailsList = new List<LearnerRecordDetails>();

            foreach (var learner in pendingGenderLearners)
            {
                var lrsResponse = await _learnerServiceApiClient.FetchLearnerDetailsAsync(learner);

                if (lrsResponse != null && lrsResponse.FindLearnerResponse?.Learner?.Length > 0)
                {
                    var learnerRecord = _mapper.Map<LearnerRecordDetails>(lrsResponse.FindLearnerResponse.Learner[0], opt => opt.Items["profileId"] = learner.ProfileId);
                    learnerRecord.IsLearnerVerified = !string.IsNullOrWhiteSpace(lrsResponse.FindLearnerResponse?.ResponseCode) && lrsResponse.FindLearnerResponse.ResponseCode.Equals("WSRC0004", StringComparison.InvariantCultureIgnoreCase);
                    learnerRecordDetailsList.Add(learnerRecord);
                }
            }

            // process learner gender
            var response = await _learnerRecordService.ProcessLearnerGenderAsync(learnerRecordDetailsList);
            response.TotalCount = pendingGenderLearners.Count();
            return response;
        }
    }
}
