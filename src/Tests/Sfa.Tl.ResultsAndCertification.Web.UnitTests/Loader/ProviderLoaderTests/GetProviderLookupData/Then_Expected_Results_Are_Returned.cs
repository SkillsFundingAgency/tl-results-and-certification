using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetProviderLookupData
{
    public class Then_Expected_Results_Are_Returned : When_GetProviderLookupDataAsync_Is_Called
    {
        private IEnumerable<ProviderMetadata> expectedaApiResult;

        public override void Given() 
        {
            expectedaApiResult = new List<ProviderMetadata> 
            {
                new ProviderMetadata { Id = 1, DisplayName = "Test provider 1" },
                new ProviderMetadata { Id = 2, DisplayName = "Test provider 2" },
                new ProviderMetadata { Id = 3, DisplayName = "Test provider 3" },
            };

            InternalApiClient.FindProviderAsync(ProviderName, IsExactMatch)
                .Returns(expectedaApiResult);
        }

        [Fact]
        public void Then_FindProviderAsync_Method_Is_Called()
        {
            InternalApiClient.Received(1).FindProviderAsync(ProviderName, IsExactMatch);
        }

        [Fact]
        public void Then_Expected_LookupData_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Count().Should().Be(3);

            var actualFirstResult = ActualResult.First();
            var expectedFirstResult = expectedaApiResult.First();

            expectedFirstResult.Id.Should().Be(actualFirstResult.Id);
            expectedFirstResult.DisplayName.Should().Be(actualFirstResult.DisplayName);
        }
    }
}
