using Lrs.PersonalLearningRecordService.Api.Client;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface IPersonalLearningRecordApiClient
    {
        Task<GetLearnerLearningEventsResponse> GetLearnerEventsAsync(string uln, string firstName, string lastName, DateTime dateOfBirth);
    }
}
