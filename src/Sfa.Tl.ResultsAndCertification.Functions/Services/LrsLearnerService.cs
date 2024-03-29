﻿using AutoMapper;
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
    public class LrsLearnerService : ILrsLearnerService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ILrsLearnerServiceApiClient _learnerServiceApiClient;
        private readonly ILrsService _learnerRecordService;

        public LrsLearnerService(IMapper mapper, ILogger<ILrsLearnerService> logger, ILrsLearnerServiceApiClient learnerServiceApiClient, ILrsService learnerRecordService)
        {
            _mapper = mapper;
            _logger = logger;
            _learnerServiceApiClient = learnerServiceApiClient;
            _learnerRecordService = learnerRecordService;
        }

        public async Task<LrsLearnerGenderResponse> FetchLearnerGenderAsync()
        {
            var pendingGenderLearners = await _learnerRecordService.GetPendingGenderLearnersAsync();

            if (pendingGenderLearners == null || !pendingGenderLearners.Any())
            {
                var message = $"No pending learners found to process gender. Method: FetchLearnerGenderAsync()";
                _logger.LogWarning(LogEvent.NoDataFound, message);
                return new LrsLearnerGenderResponse { IsSuccess = true, Message = message };
            }

            var learnerRecordDetailsList = new List<LrsLearnerRecordDetails>();

            foreach (var learner in pendingGenderLearners)
            {
                var lrsResponse = await _learnerServiceApiClient.FetchLearnerDetailsAsync(learner);

                if (lrsResponse != null && lrsResponse.FindLearnerResponse?.Learner?.Length > 0)
                    learnerRecordDetailsList.Add(_mapper.Map<LrsLearnerRecordDetails>(lrsResponse.FindLearnerResponse.Learner[0], opt => { opt.Items[Constants.LrsProfileId] = learner.ProfileId; opt.Items[Constants.LrsResponseCode] = lrsResponse.FindLearnerResponse.ResponseCode; }));
            }

            // process learner gender
            var response = await _learnerRecordService.ProcessLearnerGenderAsync(learnerRecordDetailsList);
            response.TotalCount = pendingGenderLearners.Count();
            return response;
        }
    }
}
