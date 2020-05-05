using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncGet
{
    public class Then_On_IsBackToVerifyPage_SessionKey_True_Viewmodel_IsBackToVerifyPage_Set_To_True : When_ReportIssueAsync_Is_Called
    {
        public override void Given()
        {
            TempData[Common.Helpers.Constants.IsBackToVerifyPage] = "true";
            TlevelLoader.GetQueryTlevelViewModelAsync(ukprn, pathwayId)
                .Returns(expectedResult);
        }

        [Fact]
        public void Then_IsBackToVerifyPage_Set_To_True()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as TlevelQueryViewModel;

            model.IsBackToVerifyPage.Should().BeTrue();
        }
    }
}
