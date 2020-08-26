using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.FindUlnAsync
{
    public class Then_No_Results_Are_Returned : When_FindUlnAsync_Is_Called
    {
        public override void Given()
        {
            expectedApiResult = null;
            InternalApiClient.FindUlnAsync(Ukprn, Uln).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_No_FindUln_Result_Is_Null()
        {
            ActualResult.Should().BeNull();
        }
    }
}
