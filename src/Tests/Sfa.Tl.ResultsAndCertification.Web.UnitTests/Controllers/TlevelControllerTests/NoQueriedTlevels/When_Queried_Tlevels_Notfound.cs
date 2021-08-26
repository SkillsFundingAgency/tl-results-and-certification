using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.NoQueriedTlevels
{
    public class When_Queried_Tlevels_Notfound : TestSetup
    {
        public override void Given()
        {
            var mockresult = new QueriedTlevelsViewModel
            {
                Tlevels = new List<YourTlevelViewModel>()
            };

            TlevelLoader.GetQueriedTlevelsViewModelAsync(AoUkprn)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Called_Expected_Method()
        {
            TlevelLoader.Received(1).GetQueriedTlevelsViewModelAsync(AoUkprn);
        }

        [Fact]
        public void Then_Returns_Expected_ViewModel()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as NoQueriedTlevelsViewModel;

            model.Should().NotBeNull();

            // Breadcrumb
            model.BreadCrumb.Should().NotBeNull();
            model.BreadCrumb.BreadcrumbItems.Count().Should().Be(3);
            model.BreadCrumb.BreadcrumbItems.ElementAt(0).DisplayName.Should().Be(BreadcrumbContent.Home);
            model.BreadCrumb.BreadcrumbItems.ElementAt(0).RouteName.Should().Be(RouteConstants.Home);
            model.BreadCrumb.BreadcrumbItems.ElementAt(1).DisplayName.Should().Be(BreadcrumbContent.Tlevels_Dashboard);
            model.BreadCrumb.BreadcrumbItems.ElementAt(1).RouteName.Should().Be(RouteConstants.TlevelsDashboard);
            model.BreadCrumb.BreadcrumbItems.ElementAt(2).DisplayName.Should().Be(BreadcrumbContent.Tlevels_None_Queried);
            model.BreadCrumb.BreadcrumbItems.ElementAt(2).RouteName.Should().BeNull();
        }
    }
}
