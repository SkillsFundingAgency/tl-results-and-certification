using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.GetProviderLookupData
{
    public class Then_OnEmpty_ProviderName_Returns_Empty_JsonResults : When_FindProviderAsync_Post_Action_Is_Called
    {
        public override void Given()
        {
            ProviderName = string.Empty;
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            Result.Result.Should().BeOfType(typeof(JsonResult));
            var expectedResult = Result.Result as JsonResult;

            expectedResult.Value.Should().Be(string.Empty);
        }
    }
}
