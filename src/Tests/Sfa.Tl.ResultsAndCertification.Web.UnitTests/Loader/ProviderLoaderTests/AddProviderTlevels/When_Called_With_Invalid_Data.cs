using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.AddProviderTlevels
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            ExpectedResult = false;
            InternalApiClient.AddProviderTlevelsAsync(Arg.Any<List<ProviderTlevel>>()).Returns(ExpectedResult);
            Loader = new ProviderLoader(InternalApiClient, Mapper);            
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeFalse();
        }
    }
}
