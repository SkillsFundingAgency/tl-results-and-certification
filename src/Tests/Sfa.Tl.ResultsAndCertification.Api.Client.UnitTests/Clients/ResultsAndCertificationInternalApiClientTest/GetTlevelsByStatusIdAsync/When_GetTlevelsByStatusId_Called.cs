using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest.GetTlevelsByStatusIdAsync
{
    public class When_GetTlevelsByStatusId_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        protected ITokenServiceClient _tokenServiceClient;
        protected ResultsAndCertificationConfiguration _configuration;
        protected readonly long ukprn = 1024;
        protected Task<IEnumerable<AwardingOrganisationPathwayStatus>> Result;

        protected readonly string TlevelTitle = "Construction";
        protected readonly int StatusId = 1;

        protected ResultsAndCertificationInternalApiClient _apiClient;
        protected List<AwardingOrganisationPathwayStatus> _mockHttpResult;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new List<AwardingOrganisationPathwayStatus>
            {
                new AwardingOrganisationPathwayStatus { TlevelTitle = TlevelTitle, StatusId = StatusId }
            };            
            
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<IEnumerable<AwardingOrganisationPathwayStatus>>(_mockHttpResult, string.Format(ApiConstants.GetTlevelsByStatus, ukprn, StatusId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            Result = _apiClient.GetTlevelsByStatusIdAsync(ukprn, StatusId);
        }
    }
}
