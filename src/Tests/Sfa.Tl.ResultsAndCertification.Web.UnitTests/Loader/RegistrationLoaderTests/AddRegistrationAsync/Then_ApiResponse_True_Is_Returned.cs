using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.AddRegistrationAsync
{
    public class Then_ApiResponse_True_Is_Returned : When_AddRegistrationAsync_Is_Called
    {
        [Fact]
        public void Then_ApiResponse_Is_True()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
