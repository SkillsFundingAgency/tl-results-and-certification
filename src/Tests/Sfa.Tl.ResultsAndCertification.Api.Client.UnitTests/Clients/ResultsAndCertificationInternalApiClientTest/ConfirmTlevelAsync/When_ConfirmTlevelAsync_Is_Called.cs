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
using Newtonsoft.Json;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest.ConfirmTlevelAsync
{
    public class When_ConfirmTlevelAsync_Is_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        protected ITokenServiceClient _tokenServiceClient;
        protected ResultsAndCertificationConfiguration _configuration;
        protected readonly long ukprn = 1024;
        protected Task<bool> Result;

        protected readonly string RouteName = "Construction";
        protected readonly string PathwayName = "Design, Surveying and Planning";
        protected readonly int StatusId = 1;

        protected ResultsAndCertificationInternalApiClient _apiClient;
        protected bool _mockHttpResult;

        private ConfirmTlevelDetails _model;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationApiSettings = new ResultsAndCertificationApiSettings { InternalApiUri = "http://tlevel.api.com" }
            };

            _mockHttpResult = true;
            _model = new ConfirmTlevelDetails
            {
                TqAwardingOrganisationId = 1,
                PathwayStatusId = 2,
                ModifiedBy = "Test User"
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<bool>(_mockHttpResult, ApiConstants.ConfirmTlevelUri, HttpStatusCode.OK, JsonConvert.SerializeObject(_model)));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            Result = _apiClient.ConfirmTlevelAsync(_model);
        }
    }
}
