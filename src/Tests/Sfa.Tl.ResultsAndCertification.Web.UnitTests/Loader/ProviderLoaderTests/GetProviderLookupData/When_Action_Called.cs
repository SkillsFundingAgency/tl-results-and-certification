using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetProviderLookupData
{
    public class When_Action_Called : TestSetup
    {
        private IEnumerable<ProviderMetadata> expectedaApiResult;

        public override void Given() 
        {
            expectedaApiResult = new List<ProviderMetadata> 
            {
                new ProviderMetadata { Id = 1, DisplayName = "Test provider 1", UkPrn = "10000001" },
                new ProviderMetadata { Id = 2, DisplayName = "Test provider 2", UkPrn = "10000002" },
                new ProviderMetadata { Id = 3, DisplayName = "Test provider 3", UkPrn = "10000003" },
            };

            InternalApiClient.FindProviderAsync(ProviderName, IsExactMatch)
                .Returns(expectedaApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).FindProviderAsync(ProviderName, IsExactMatch);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Count().Should().Be(3);

            var actualFirstResult = ActualResult.First();
            var expectedFirstResult = expectedaApiResult.First();

            expectedFirstResult.Id.Should().Be(actualFirstResult.Id);
            expectedFirstResult.DisplayName.Should().Be(actualFirstResult.DisplayName.ToProviderName());
        }
    }
}
