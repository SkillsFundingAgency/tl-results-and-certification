using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ReportIssueAsyncGet
{
    public class When_IsBackToConfirmed_IsTrue : TestSetup
    {
        public override void Given()
        {
            isBackToConfirmed = true;
            TlevelLoader.GetQueryTlevelViewModelAsync(AoUkprn, pathwayId)
                .Returns(expectedResult);
        }

        [Fact]
        public void Then_Returns_Expected_BackLinkModel()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as TlevelQueryViewModel;

            model.IsBackToConfirmed.Should().BeTrue();
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.TlevelConfirmedDetails);
            model.BackLink.RouteAttributes.Count().Should().Be(1);
            model.BackLink.RouteAttributes["id"].Should().Be(model.PathwayId.ToString());
        }
    }
}
