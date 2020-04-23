using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.AddProviderTlevelsAsync
{
    public class Then_ApiResponse_False_Is_Returned : When_Called_Method_AddProviderTlevelsAsync
    {
        public override void Given()
        {
            ExpectedResult = false;
            InternalApiClient.AddProviderTlevelsAsync(Arg.Any<List<ProviderTlevel>>())
                .Returns(ExpectedResult);
            Loader = new ProviderLoader(InternalApiClient, Mapper);            
        }

        [Fact]
        public void Then_ApiResponse_Is_False()
        {
            ActualResult.Should().BeFalse();
        }
    }
}
