using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetTqAoProviderDetails
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            ApiClientResponse = new List<ProviderDetails>();
            InternalApiClient.GetTqAoProviderDetailsAsync(Ukprn).Returns(ApiClientResponse);
            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNullOrEmpty();
        }
    }
}
