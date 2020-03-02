using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.GetTlevelDetailsByPathwayIdAsync
{
    public abstract class When_GetTlevelDetailsByPathwayIdAsync_Is_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private readonly long ukprn = 1024;
        private readonly int tlevelId = 99;

        //public HttpClient HttpClient { get; private set; }
        protected Task<TlevelPathwayDetails> Result;

        protected readonly string RouteName = "Construction";
        protected readonly string PathwayName = "Design";
        protected readonly List<string> Specialisms = new List<string> { "Civil Engineering", "Assisting teaching" };
        protected readonly int Status = 1;

        private ResultsAndCertificationInternalApiClient _apiClient;
        private TlevelPathwayDetails _mockHttpResult;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationApiSettings = new ResultsAndCertificationApiSettings { InternalApiUri = "http://xtz.com" }
            };

            _mockHttpResult = new TlevelPathwayDetails
            {
                PathwayName = PathwayName,
                RouteName = RouteName,
                Specialisms = Specialisms,
                PathwayStatusId = Status
            };
            HttpClient = new HttpClient(new MockHttpMessageHandler<TlevelPathwayDetails>(_mockHttpResult, string.Format(ApiConstants.TlevelDetailsUri, ukprn, tlevelId), HttpStatusCode.OK));
        }

        public override void Given()
        {            
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            Result = _apiClient.GetTlevelDetailsByPathwayIdAsync(ukprn, tlevelId);
        }
    }
}
