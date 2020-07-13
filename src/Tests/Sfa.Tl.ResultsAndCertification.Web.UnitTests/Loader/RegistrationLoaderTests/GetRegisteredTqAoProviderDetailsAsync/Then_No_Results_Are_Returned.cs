using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegisteredTqAoProviderDetailsAsync
{
    public class Then_No_Results_Are_Returned : When_GetRegisteredTqAoProviderDetailsAsync_Is_Called
    {
        public override void Given()
        {
            ApiClientResponse = null;
            InternalApiClient.GetTqAoProviderDetailsAsync(Ukprn).Returns(ApiClientResponse);
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_No_SelectProviderViewModel_Are_Returned()
        {
            ActualResult.Should().BeNull();
        }
    }
}
