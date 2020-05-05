using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.VerifyAsync
{
    public class Then_On_IsBack_Is_True_IsEverythingCorrect_Set_To_False : When_VerifyAsync_Get_Action_Is_Called
    {
        private ConfirmTlevelViewModel expectedModel;

        public override void Given()
        {
            pathwayId = 10;
            isBack = true; 

            expectedModel = new ConfirmTlevelViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation };

            TlevelLoader.GetVerifyTlevelDetailsByPathwayIdAsync(ukprn, pathwayId)
                .Returns(expectedModel);
        }

        [Fact]
        public void Then_IsEverythingCorrect_Set_To_False()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as ConfirmTlevelViewModel;

            model.Should().NotBeNull();
            model.IsEverythingCorrect.Should().Be(false);
        }
    }
}
