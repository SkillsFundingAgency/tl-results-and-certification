using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderAddressLoaderTests.GetAddressesByPostcode
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private Models.Contracts.OrdnanceSurvey.PostcodeLookupResult _expectedApiResult;

        public override void Given()
        {
            _expectedApiResult = null;
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
            ActualResult.Should().BeNull();
        }
    }
}