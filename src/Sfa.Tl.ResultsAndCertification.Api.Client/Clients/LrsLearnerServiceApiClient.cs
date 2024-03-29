﻿using Lrs.LearnerService.Api.Client;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Clients
{
    public class LrsLearnerServiceApiClient : ILrsLearnerServiceApiClient
    {
        private readonly ILogger<ILrsLearnerServiceApiClient> _logger;
        private readonly ILearnerPortTypeClient _learnerPortTypeClient;
        private readonly ResultsAndCertificationConfiguration _configuration;

        public LrsLearnerServiceApiClient(ILogger<ILrsLearnerServiceApiClient> logger, ILearnerPortTypeClient learnerQueryPortTypeClient, ResultsAndCertificationConfiguration configuration)
        {
            _logger = logger;
            _learnerPortTypeClient = learnerQueryPortTypeClient;
            _configuration = configuration;
        }

        public async Task<verifyLearnerResponse> VerifyLearnerAsync(RegisteredLearnerDetails learnerDetails)
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
                            ULN = learnerDetails.Uln.ToString(),
                            GivenName = learnerDetails.Firstname,
                            FamilyName = learnerDetails.Lastname,
                            DateOfBirth = learnerDetails.DateofBirth.ToString(Constants.LrsDateFormat)
                        }
                    }
                };

                return await _learnerPortTypeClient.verifyLearnerAsync(request);                
            }
            catch (Exception ex)
            {
                if (_learnerPortTypeClient.State == CommunicationState.Faulted)
                    _learnerPortTypeClient.Abort();

                _logger.LogError($"Error while executing VerifyLearnerAsnc. Exception = {ex}");
                return null;
            }
        }

        public async Task<findLearnerByULNResponse> FetchLearnerDetailsAsync(RegisteredLearnerDetails learnerDetails)
        {
            try
            {
                var learnerRequest = new learnerByULNRequest
                {
                    FindLearnerByULN = new LearnerByULNRqst
                    {
                        UKPRN = _configuration.LearningRecordServiceSettings.Ukprn,
                        UserName = _configuration.LearningRecordServiceSettings.Username,
                        OrgPassword = _configuration.LearningRecordServiceSettings.Password,
                        ULN = learnerDetails.Uln.ToString(),
                        GivenName = learnerDetails.Firstname,
                        FamilyName = learnerDetails.Lastname,
                        FindType = Constants.LearnerByULNFindType
                    }
                };                

                return await _learnerPortTypeClient.learnerByULNAsync(learnerRequest);                
            }
            catch (Exception ex)
            {
                if (_learnerPortTypeClient.State == CommunicationState.Faulted)
                    _learnerPortTypeClient.Abort();

                _logger.LogError($"Error while executing FetchLearnerDetailsAsync. Exception = {ex}");
                return null;
            }
        }
    }
}

namespace Lrs.LearnerService.Api.Client
{
    public interface ILearnerPortTypeClient : ICommunicationObject, LearnerPortType { }
    public partial class LearnerPortTypeClient : ILearnerPortTypeClient
    {
#pragma warning disable IDE0060 // Remove unused parameter
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            (serviceEndpoint.Binding as BasicHttpBinding).Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
        }
    }
}
