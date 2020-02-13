using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.GetAllTlevelsByAwardingOrganisationAsync
{
    public abstract class When_GetAllTlevelsByAwardingOrganisationAsync_Is_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        protected HttpClient MockHttpClient;
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;

        public HttpClient HttpClient { get; private set; }
        protected Task<IEnumerable<AwardingOrganisationPathwayStatus>> Result;

        protected readonly string RouteName = "Construction";
        protected readonly string PathwayName = "Design";
        protected readonly int Status = 1;

        private ResultsAndCertificationInternalApiClient _apiClient;
        private IEnumerable<AwardingOrganisationPathwayStatus> _mockHttpResult; 
        
        public override void Setup()
        {
            MockHttpClient = Substitute.For<HttpClient>();

            _tokenServiceClient = Substitute.For<ITokenServiceClient>();
            _configuration = new ResultsAndCertificationConfiguration
            { ResultsAndCertificationApiSettings = new ResultsAndCertificationApiSettings { InternalApiUri = "https://localhost:5001" } };

            _mockHttpResult = new List<AwardingOrganisationPathwayStatus> 
            { 
                new AwardingOrganisationPathwayStatus { PathwayName = PathwayName, RouteName = RouteName, StatusId = Status }
            };
            HttpClient = new HttpClient(new MockHttpMessageHandler<IEnumerable<AwardingOrganisationPathwayStatus>>(_mockHttpResult, HttpStatusCode.OK));
            
        }
        public override void Given() 
        {
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }
        
        public override void When()
        {
            Result = _apiClient.GetAllTlevelsByAwardingOrganisationAsync();
        }
    }
}
