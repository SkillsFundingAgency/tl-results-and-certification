using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Index
{
    public class Then_Redirected_To_ViewAll_Action : When_Index_Action_Called
    {
        public override void Given()
        {
            TlevelLoader.GetTlevelsByStatusIdAsync(Arg.Any<long>(), Arg.Any<int>())
                .Returns(new List<YourTlevelsViewModel>());
        }

        [Fact]
        public void Then_GetTlevelsByStatusIdAsync_Is_Called()
        {
            TlevelLoader.Received().GetTlevelsByStatusIdAsync(Arg.Any<long>(), (int)TlevelReviewStatus.AwaitingConfirmation);
        }

        [Fact]
        public void Then_Redirected_To_Action_ViewAll()
        {
            var actualActionName = (Result.Result as RedirectToActionResult).ActionName;
            var expectedActionName = nameof(TlevelController.ViewAll);

            actualActionName.Should().Be(expectedActionName);
        }
    }
}
