using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.AllTlevelsReviewed
{
    public class When_Awaiting_Tlevels_Are_Zero : TestSetup
    {
        public override void Given()
        {
            TlevelLoader.GetTlevelsByStatusIdAsync(Arg.Any<long>(), Arg.Any<int>())
                .Returns(new List<YourTlevelViewModel>());
        }

        [Fact]
        public void Then_Called_Expected_Methods()
        {
            TlevelLoader.Received(1).GetTlevelsByStatusIdAsync(Arg.Any<long>(), (int)TlevelReviewStatus.AwaitingConfirmation);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AllTlevelsReviewedViewModel));

            var model = viewResult.Model as AllTlevelsReviewedViewModel;
            model.Should().NotBeNull();

            // Breadcrum
            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count.Should().Be(3);

            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.TlevelsDashboard);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Tlevels_Dashboard);
            model.Breadcrumb.BreadcrumbItems[2].RouteName.Should().BeNull();
            model.Breadcrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Tlevel_Review_Select);
        }
    }
}
