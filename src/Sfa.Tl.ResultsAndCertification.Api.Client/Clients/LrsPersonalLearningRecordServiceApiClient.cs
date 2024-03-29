﻿using Lrs.PersonalLearningRecordService.Api.Client;
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
    public class LrsPersonalLearningRecordServiceApiClient : ILrsPersonalLearningRecordServiceApiClient
    {
        private readonly ILogger<ILrsPersonalLearningRecordServiceApiClient> _logger;
        private readonly ILearnerServiceR9Client _learnerServiceR9Client;
        private readonly ResultsAndCertificationConfiguration _configuration;

        public LrsPersonalLearningRecordServiceApiClient(ILogger<ILrsPersonalLearningRecordServiceApiClient> logger,
            ILearnerServiceR9Client learnerServiceR9Client,
            ResultsAndCertificationConfiguration configuration)
        {
            _logger = logger;
            _learnerServiceR9Client = learnerServiceR9Client;
            _configuration = configuration;
        }

        public async Task<GetLearnerLearningEventsResponse> GetLearnerEventsAsync(RegisteredLearnerDetails learnerDetails)
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

                return await _learnerServiceR9Client.GetLearnerLearningEventsAsync(invokingOrganisation, Constants.LearnerLearningEventsUserType,
                    _configuration.LearningRecordServiceSettings.VendorId, Constants.LrsLanguage, learnerDetails.Uln.ToString(),
                    learnerDetails.Firstname, learnerDetails.Lastname, learnerDetails.DateofBirth.ToString(Constants.LrsDateFormat), null,
                    Constants.LearnerLearningEventsGetType);
            }
            catch (Exception ex)
            {
                if (_learnerServiceR9Client.State == CommunicationState.Faulted)
                    _learnerServiceR9Client.Abort();

                _logger.LogError($"Error while executing GetLearnerEventsAsync({learnerDetails}). Exception = {ex}");
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
#pragma warning disable IDE0060 // Remove unused parameter
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            (serviceEndpoint.Binding as BasicHttpBinding).Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
        }
    }
}
