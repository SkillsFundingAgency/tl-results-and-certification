using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TimeoutControllerTests.TimeoutConfirmation
{
    public class Then_TimeoutConfirmation_View_IsCalled : When_TimeoutConfirmation_Action_Called
    {
        public override void Given() { }

        [Fact]
        public void Then_Timeout_Confirmation_View_Page_IsCalled()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType<ViewResult>();
            var viewResult = Result as ViewResult;
            viewResult.Should().NotBeNull();
        }
    }
}
