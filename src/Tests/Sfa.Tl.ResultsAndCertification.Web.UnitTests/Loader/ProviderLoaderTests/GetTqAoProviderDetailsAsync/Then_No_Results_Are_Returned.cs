using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetTqAoProviderDetailsAsync
{
    public class Then_No_Results_Are_Returned : When_GetTqAoProviderDetailsAsync_Is_Called
    {
        public override void Given()
        {
            ApiClientResponse = new List<ProviderDetails>();

            InternalApiClient.GetTqAoProviderDetailsAsync(Ukprn).Returns(ApiClientResponse);

            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_No_ProviderDetails_Are_Returned()
        {
            ActualResult.Should().BeNullOrEmpty();
        }
    }
}
