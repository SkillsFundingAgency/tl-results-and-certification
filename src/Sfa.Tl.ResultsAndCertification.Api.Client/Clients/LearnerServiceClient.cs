using Lrs.LearnerService.Api.Client;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class LearnerServiceClient : ILearnerServiceApiClient
    {
        private readonly ILogger<LearnerServiceClient> _logger;
        private readonly ILearnerPortTypeClient _learnerPortTypeClient;
        private readonly ResultsAndCertificationConfiguration _configuration;

        public LearnerServiceClient(ILogger<LearnerServiceClient> logger, ILearnerPortTypeClient learnerQueryPortTypeClient, ResultsAndCertificationConfiguration configuration)
        {
            _logger = logger;
            _learnerPortTypeClient = learnerQueryPortTypeClient;
            _configuration = configuration;
        }

        public async Task<bool> VerifyLearnerAsync(string uln, string firstName, string lastName, string dateOfBirth)
        {
            try
            {
                var request = new verifyLearnerRequest
                {
                    VerifyLearner = new VerifyLearnerRqst
                    {
                        UKPRN = _configuration.LearningRecordServiceSettings.Ukprn,
                        UserName = _configuration.LearningRecordServiceSettings.Username,
                        OrgPassword = _configuration.LearningRecordServiceSettings.Password,
                        LearnerToVerify = new MIAPLearnerToVerify
                        {
                            ULN = uln,
                            GivenName = firstName,
                            FamilyName = lastName,
                            DateOfBirth = dateOfBirth
                        }
                    }
                };

                var response = await _learnerPortTypeClient.verifyLearnerAsync(request);
                var responseCode = response.VerifyLearnerResponse.VerifiedLearner.ResponseCode;
                //_learnerPortTypeClient.Close();

                if (string.IsNullOrWhiteSpace(responseCode) || responseCode.Equals("WSVRC005", StringComparison.InvariantCultureIgnoreCase) || responseCode.Equals("WSVRC006", StringComparison.InvariantCultureIgnoreCase))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                if (_learnerPortTypeClient.State == CommunicationState.Faulted)
                    _learnerPortTypeClient.Abort();

                _logger.LogError($"Error while executing VerifyLearnerAsnc. Exception = {ex}");
                //_learnerPortTypeClient.Close();
                return false;
            }
        }
    }
}

namespace Lrs.LearnerService.Api.Client
{
    public interface ILearnerPortTypeClient : ICommunicationObject, LearnerPortType { }
    public partial class LearnerPortTypeClient : ILearnerPortTypeClient
    {
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials)
        {
            (serviceEndpoint.Binding as BasicHttpBinding).Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
        }
    }
}
