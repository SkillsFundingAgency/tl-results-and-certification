using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncGet
{
    public class Then_On_IsBackToVerifyPage_SessionKey_True_Viewmodel_IsBackToVerifyPage_Set_To_True : When_ReportIssueAsync_Is_Called
    {
        public override void Given()
        {
            TempData[Constants.IsBackToVerifyPage] = "true";
            TlevelLoader.GetQueryTlevelViewModelAsync(ukprn, pathwayId)
                .Returns(expectedResult);
        }

        [Fact]
        public void Then_IsBackToVerifyPage_Set_To_True()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as TlevelQueryViewModel;

            model.IsBackToVerifyPage.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AreDetailsCorrect);
            model.BackLink.RouteAttributes.Count().Should().Be(2);
            model.BackLink.RouteAttributes["id"].Should().Be(model.PathwayId.ToString());
            model.BackLink.RouteAttributes["isback"].Should().Be("true");
        }
    }
}
