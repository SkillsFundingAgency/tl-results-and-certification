using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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
    public class When_GetRegistrationDetailsByProfileId_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private readonly long _ukprn = 12345678;
        private readonly int _profileId = 1;
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Active;
        protected RegistrationDetails _mockHttpResult;

        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private ResultsAndCertificationInternalApiClient _apiClient;
        private RegistrationDetails _result;

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
                ProviderName = "Test Provider (1234567)",
                PathwayName = "Pathway (7654321)",
                SpecialismsDisplayName = new List<string> { "Specialism1 (2345678)", "Specialism2 (555678)" },
                AcademicYear = 2020,
                Status = RegistrationPathwayStatus.Active
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<RegistrationDetails>(_mockHttpResult, string.Format(ApiConstants.GetRegistrationDetailsUri, _ukprn, _profileId, (int)_registrationPathwayStatus), HttpStatusCode.OK));
            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetRegistrationDetailsAsync(_ukprn, _profileId, _registrationPathwayStatus);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Uln.Should().Be(_mockHttpResult.Uln);
            _result.Name.Should().Be(_mockHttpResult.Name);
            _result.DateofBirth.Should().Be(_mockHttpResult.DateofBirth);
            _result.ProviderName.Should().Be(_mockHttpResult.ProviderName);
            _result.PathwayName.Should().Be(_mockHttpResult.PathwayName);
            _result.SpecialismsDisplayName.Should().BeEquivalentTo(_mockHttpResult.SpecialismsDisplayName);
            _result.AcademicYear.Should().Be(_mockHttpResult.AcademicYear);
            _result.Status.Should().Be(_mockHttpResult.Status);
        }
    }
}
