using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.GetProviderLookupData
{
    public class When_ProviderName_InvalidLength : When_FindProviderAsync_Post_Action_Is_Called
    {
        public override void Given()
        {
            ProviderName = "Lo";
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(JsonResult));
            var expectedResult = Result as JsonResult;
            expectedResult.Value.Should().Be(string.Empty);
        }
    }
}
