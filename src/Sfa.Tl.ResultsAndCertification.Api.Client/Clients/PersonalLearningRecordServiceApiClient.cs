using Lrs.PersonalLearningRecordService.Api.Client;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class PersonalLearningRecordServiceApiClient : IPersonalLearningRecordApiClient
    {
        private readonly ILogger<ILearnerServiceR9Client> _logger;
        private readonly ILearnerServiceR9Client _learnerServiceR9Client;
        private readonly ResultsAndCertificationConfiguration _configuration;        

        public PersonalLearningRecordServiceApiClient(ILogger<ILearnerServiceR9Client> logger, ILearnerServiceR9Client learnerServiceR9Client, ResultsAndCertificationConfiguration configuration)
        {
            _logger = logger;
            _learnerServiceR9Client = learnerServiceR9Client;
            _configuration = configuration;
        }

        public async Task<GetLearnerLearningEventsResponse> GetLearnerEventsAsync(string uln, string firstName, string lastName, DateTime dateOfBirth)
        {
            try
            {
                var invokingOrganisation = new InvokingOrganisationR10
                {
                    OrganisationRef = _configuration.LearningRecordServiceSettings.Ukprn,
                    Ukprn = _configuration.LearningRecordServiceSettings.Ukprn,
                    Username = _configuration.LearningRecordServiceSettings.Username,
                    Password = _configuration.LearningRecordServiceSettings.Password
                };

                var response = await _learnerServiceR9Client.GetLearnerLearningEventsAsync(invokingOrganisation, "ORG", _configuration.LearningRecordServiceSettings.VendorId, "ENG", uln, firstName, lastName, dateOfBirth.ToString("yyyy-MM-dd"), null, "FULL");
                return response;
            }
            catch (Exception ex)
            {
                if (_learnerServiceR9Client.State == CommunicationState.Faulted)
                    _learnerServiceR9Client.Abort();

                _logger.LogError($"Error while executing GetLearnerEventsAsync({uln}, {firstName}, {lastName}, {dateOfBirth.ToShortDateString()}). Exception = {ex}");
                return null;
            }
        }
    }
}

namespace Lrs.PersonalLearningRecordService.Api.Client
{
    public interface ILearnerServiceR9Client : ICommunicationObject, ILearnerServiceR9 { }
    public partial class LearnerServiceR9Client : ILearnerServiceR9Client
    {
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials)
        {
            (serviceEndpoint.Binding as BasicHttpBinding).Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
        }
    }
}
