using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.OrdnanceSurvey;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderAddressLoaderTests.GetAddressByUprn
{
    public class When_Called_With_NoBuildingNumber : TestSetup
    {
        private PostcodeLookupResult _expectedApiResult;

        public override void Given()
        {
            Uprn = 1234567895;

            _expectedApiResult = new PostcodeLookupResult
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
                           BuildingNumber = "Test line 1",
                           ThroughfareName = "Test line 2",
                           Town = "Test town",
                           Postcode = "xx1 1xx"
                       }
                   }
                }
            };
            OrdnanceSurveyApiClient.GetAddressByUprnAsync(Uprn).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            OrdnanceSurveyApiClient.Received(1).GetAddressByUprnAsync(Uprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            var actualResult = ActualResult;
            var expectedResult = _expectedApiResult.AddressResult[0].DeliveryPointAddress;

            actualResult.Udprn.Should().Be(expectedResult.Uprn);
            actualResult.FormattedAddress.Should().Be(expectedResult.FormattedAddress);
            actualResult.OrganisationName.Should().Be(expectedResult.OrganisationName);
            actualResult.AddressLine1.Should().Be($"{expectedResult.BuildingNumber}, {expectedResult.ThroughfareName}");
            actualResult.AddressLine2.Should().BeNullOrWhiteSpace();
            actualResult.Town.Should().Be(expectedResult.Town);
            actualResult.Postcode.Should().Be(expectedResult.Postcode);
        }
    }
}
