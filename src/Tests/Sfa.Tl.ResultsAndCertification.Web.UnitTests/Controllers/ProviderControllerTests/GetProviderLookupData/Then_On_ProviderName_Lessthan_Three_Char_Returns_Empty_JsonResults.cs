using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderControllerTests.GetProviderLookupData
{
    public class Then_On_ProviderName_Lessthan_Three_Char_Returns_Empty_JsonResults : When_FindProviderAsync_Post_Action_Is_Called
    {
        public override void Given()
        {
            ProviderName = "Lo";
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
