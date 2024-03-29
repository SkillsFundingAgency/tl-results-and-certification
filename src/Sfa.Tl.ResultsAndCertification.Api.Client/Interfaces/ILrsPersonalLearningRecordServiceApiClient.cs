﻿using Lrs.PersonalLearningRecordService.Api.Client;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface ILrsPersonalLearningRecordServiceApiClient
    {
        Task<GetLearnerLearningEventsResponse> GetLearnerEventsAsync(RegisteredLearnerDetails learnerDetails);
    }
}
