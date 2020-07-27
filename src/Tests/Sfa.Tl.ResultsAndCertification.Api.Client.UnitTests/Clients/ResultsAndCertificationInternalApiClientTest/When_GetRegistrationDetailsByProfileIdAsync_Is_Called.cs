using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetRegistrationDetailsByProfileIdAsync_Is_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly long _ukprn = 12345678;
        private readonly int _profileId = 1;

        protected RegistrationDetails _mockHttpResult;

        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private Task<RegistrationDetails> _result;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };

            _mockHttpResult = new RegistrationDetails
            {
                ProfileId = 1,
                Uln = 1234567890,
                Name = "Test",
                DateofBirth = DateTime.UtcNow,
                ProviderDisplayName = "Test Provider (1234567)",
                PathwayDisplayName = "Pathway (7654321)",
                SpecialismsDisplayName = new List<string> { "Specialism1 (2345678)", "Specialism2 (555678)" },
                AcademicYear = 2020
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<RegistrationDetails>(_mockHttpResult, string.Format(ApiConstants.GetRegistrationDetailsUri, _ukprn, _profileId), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            _result = _apiClient.GetRegistrationDetailsByProfileIdAsync(_ukprn, _profileId);
        }

        [Fact]
        public void Then_Expected_Result_Returned()
        {
            var actualResult = _result.Result;

            actualResult.Should().NotBeNull();

            actualResult.Uln.Should().Be(_mockHttpResult.Uln);
            actualResult.Name.Should().Be(_mockHttpResult.Name);
            actualResult.DateofBirth.Should().Be(_mockHttpResult.DateofBirth);
            actualResult.ProviderDisplayName.Should().Be(_mockHttpResult.ProviderDisplayName);
            actualResult.PathwayDisplayName.Should().Be(_mockHttpResult.PathwayDisplayName);
            actualResult.SpecialismsDisplayName.Should().BeEquivalentTo(_mockHttpResult.SpecialismsDisplayName);
            actualResult.AcademicYear.Should().Be(_mockHttpResult.AcademicYear);
        }
    }
}
