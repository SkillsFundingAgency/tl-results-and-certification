﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.OrdnanceSurvey;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.OrdnanceSurveyApiClientTest
{
    public class When_GetAddressesByPostcode_Called : BaseTest<OrdnanceSurveyApiClient>
    {
        private readonly string _postcode = "xx1 1xx";
        protected PostcodeLookupResult _mockHttpResult;

        private ResultsAndCertificationConfiguration _configuration;
        private OrdnanceSurveyApiClient _apiClient;
        private PostcodeLookupResult _result;

        public override void Setup()
        {
            _configuration = new ResultsAndCertificationConfiguration
            {
                OrdnanceSurveyApiSettings = new OrdnanceSurveyApiSettings { PlacesUri = "http://os.api.com", PlacesKey = "test" }
            };

            _mockHttpResult = new PostcodeLookupResult
            {
                AddressResult = new List<AddressResult>
                {
                   new AddressResult 
                   { 
                       DeliveryPointAddress = new DeliveryPointAddress
                       {
                           Uprn = "1234567895",
                           FormattedAddress = "Test line 1, Test line 2, Test town, xx1 1xx",
                           OrganisationName = "Test org name",
                           SubBuildingName = "Sub building",
                           BuildingName = "Test building name",
                           BuildingNumber = "Test line 1",
                           ThroughfareName = "Test line 2",
                           Town = "Test town",
                           Postcode = "xx1 1xx"
                       }
                   }
                }
            };
        }

        public override void Given()
        {
            HttpClient = new HttpClient(new MockHttpMessageHandler<PostcodeLookupResult>(_mockHttpResult, string.Format(ApiConstants.SearchAddressByPostcodeUri, _postcode, _configuration.OrdnanceSurveyApiSettings.PlacesKey ), HttpStatusCode.OK));
            _apiClient = new OrdnanceSurveyApiClient(HttpClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.GetAddressesByPostcodeAsync(_postcode);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.AddressResult.Should().NotBeNullOrEmpty();
            _result.AddressResult.Count.Should().Be(1);

            var actualResult = _result.AddressResult[0].DeliveryPointAddress;
            var expectedResult = _mockHttpResult.AddressResult[0].DeliveryPointAddress;

            actualResult.Uprn.Should().Be(expectedResult.Uprn);
            actualResult.FormattedAddress.Should().Be(expectedResult.FormattedAddress);
            actualResult.OrganisationName.Should().Be(expectedResult.OrganisationName);
            actualResult.BuildingName.Should().Be(expectedResult.BuildingName);
            actualResult.SubBuildingName.Should().Be(expectedResult.SubBuildingName);
            actualResult.FormattedBuildingName.Should().Be($"{expectedResult.SubBuildingName}, {expectedResult.BuildingName}");
            actualResult.BuildingNumber.Should().Be(expectedResult.BuildingNumber);
            actualResult.ThroughfareName.Should().Be(expectedResult.ThroughfareName);
            actualResult.FormattedBuildingNumberAndThroughfare.Should().Be($"{expectedResult.BuildingNumber}, {expectedResult.ThroughfareName}");
            actualResult.Town.Should().Be(expectedResult.Town);
            actualResult.Postcode.Should().Be(expectedResult.Postcode);
        }
    }
}
