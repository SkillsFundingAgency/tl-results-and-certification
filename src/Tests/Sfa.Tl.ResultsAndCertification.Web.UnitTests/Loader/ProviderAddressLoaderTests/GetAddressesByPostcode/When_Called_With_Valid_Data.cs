using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.OrdnanceSurvey;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderAddressLoaderTests.GetAddressesByPostcode
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private PostcodeLookupResult _expectedApiResult;

        public override void Given()
        {
            Postcode = "xx1 1yy";
            
            _expectedApiResult = new PostcodeLookupResult
            {
                AddressResult = new List<AddressResult>
                {
                    new AddressResult
                   {
                       DeliveryPointAddress = new DeliveryPointAddress
                       {
                           Uprn = "1234567895",
                           FormattedAddress = "Test org name, Test building name, Test line 1, Test line 2, Test town, xx1 1xx",
                           OrganisationName = "Test org name",
                           BuildingName = "Test building name",
                           BuildingNumber = "Test line 1",
                           ThroughfareName = "Test line 2",
                           Town = "Test town",
                           Postcode = "xx1 1xx"
                       }
                   }
                }
            };
            OrdnanceSurveyApiClient.GetAddressesByPostcodeAsync(Postcode).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            OrdnanceSurveyApiClient.Received(1).GetAddressesByPostcodeAsync(Postcode);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.AddressSelectList.Should().NotBeNullOrEmpty();
            ActualResult.AddressSelectList.Count.Should().Be(1);
            ActualResult.SelectedAddressUprn.Should().BeNull();
            ActualResult.SelectedAddress.Should().BeNull();
            ActualResult.DepartmentName.Should().BeNull();
            ActualResult.Postcode.Should().BeNull();

            var actualResult = ActualResult.AddressSelectList[0];
            var expectedResult = _expectedApiResult.AddressResult[0].DeliveryPointAddress;

            actualResult.Text.Should().Be(expectedResult.FormattedAddress);
            actualResult.Value.Should().Be(expectedResult.Uprn);
        }
    }
}
