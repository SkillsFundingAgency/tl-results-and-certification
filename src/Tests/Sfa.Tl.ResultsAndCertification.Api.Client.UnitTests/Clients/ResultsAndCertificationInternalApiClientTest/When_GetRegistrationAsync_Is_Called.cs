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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetRegistrationAsync_Is_Called : BaseTest<ResultsAndCertificationInternalApiClient>
    {
        private ITokenServiceClient _tokenServiceClient;
        private ResultsAndCertificationConfiguration _configuration;
        private readonly long _ukprn = 1024;
        private readonly int _profileId = 1;
        private Task<ManageRegistration> _result;

        private ResultsAndCertificationInternalApiClient _apiClient;
        private ManageRegistration _expectedResult;

        public override void Setup()
        {
            _tokenServiceClient = Substitute.For<ITokenServiceClient>();

            _configuration = new ResultsAndCertificationConfiguration
            {
                ResultsAndCertificationInternalApiSettings = new ResultsAndCertificationInternalApiSettings { Uri = "http://tlevel.api.com" }
            };
            _expectedResult = new ManageRegistration
            {
                ProfileId = 1,
                Uln = 1234567890,
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = DateTime.Now,
                ProviderUkprn = 34567890,
                CoreCode = "12345678",
                SpecialismCodes = new List<string> { "sp1", "sp2" },
                AcademicYear = 2001,
                AoUkprn = 98765432,
                PerformedBy = "User Create"
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<ManageRegistration>(
                                            _expectedResult, 
                                            string.Format(ApiConstants.GetRegistrationUri, _ukprn, _profileId), 
                                            HttpStatusCode.OK));

            _apiClient = new ResultsAndCertificationInternalApiClient(HttpClient, _tokenServiceClient, _configuration);
        }

        public override void When()
        {
            _result = _apiClient.GetRegistrationAsync(_ukprn, _profileId);
        }

        [Fact]
        public void Then_Expected_Result_Returned()
        {
            _result.Result.Should().NotBeNull();

            _result.Result.ProfileId.Should().Be(_expectedResult.ProfileId);
            _result.Result.FirstName.Should().Be(_expectedResult.FirstName);
            _result.Result.LastName.Should().Be(_expectedResult.LastName);
            _result.Result.DateOfBirth.Should().Be(_expectedResult.DateOfBirth);
            _result.Result.ProviderUkprn.Should().Be(_expectedResult.ProviderUkprn);
            _result.Result.CoreCode.Should().Be(_expectedResult.CoreCode);
            _result.Result.AcademicYear.Should().Be(_expectedResult.AcademicYear);
            _result.Result.AoUkprn.Should().Be(_expectedResult.AoUkprn);
            _result.Result.PerformedBy.Should().Be(_expectedResult.PerformedBy);

            _result.Result.SpecialismCodes.Count().Should().Be(_expectedResult.SpecialismCodes.Count());
            _result.Result.SpecialismCodes.ElementAt(0).Should().Be(_expectedResult.SpecialismCodes.ElementAt(0));
            _result.Result.SpecialismCodes.ElementAt(1).Should().Be(_expectedResult.SpecialismCodes.ElementAt(1));
        }
    }
}
