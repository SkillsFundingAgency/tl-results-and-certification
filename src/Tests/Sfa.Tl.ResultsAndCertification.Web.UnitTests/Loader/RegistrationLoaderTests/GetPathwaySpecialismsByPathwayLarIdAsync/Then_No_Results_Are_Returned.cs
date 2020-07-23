using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetPathwaySpecialismsByPathwayLarIdAsync
{
    public class Then_No_Results_Are_Returned : When_GetPathwaySpecialismsByPathwayLarIdAsync_Is_Called
    {
        public override void Given()
        {
            ApiClientResponse = null;
            InternalApiClient.GetPathwaySpecialismsByPathwayLarIdAsync(Ukprn, PathwayLarId).Returns(ApiClientResponse);
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_No_PathwaySpecialismViewModel_Are_Returned()
        {
            ActualResult.Should().BeNull();
        }
    }
}
