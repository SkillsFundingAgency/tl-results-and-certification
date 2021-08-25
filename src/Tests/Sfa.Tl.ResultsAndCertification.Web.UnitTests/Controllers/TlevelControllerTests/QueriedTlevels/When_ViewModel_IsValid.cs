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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.QueriedTlevels
{
    public class When_ViewModel_IsValid : TestSetup
    {
        private YourTlevelViewModel _expectedConfirmedTlevel;

        public override void Given()
        {
            _expectedConfirmedTlevel = new YourTlevelViewModel { PathwayId = 10, TlevelTitle = "T Level in Education" };
            var mockresult = new QueriedTlevelsViewModel
            {
                Tlevels = new List<YourTlevelViewModel> { _expectedConfirmedTlevel }
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
            var model = viewResult.Model as QueriedTlevelsViewModel;

            model.Should().NotBeNull();
            model.Tlevels.Should().NotBeNull();
            model.Tlevels.Count.Should().Be(1);
            model.Tlevels.First().PathwayId.Should().Be(_expectedConfirmedTlevel.PathwayId);
            model.Tlevels.First().TlevelTitle.Should().Be(_expectedConfirmedTlevel.TlevelTitle);

            // Breadcrumb
            model.BreadCrumb.Should().NotBeNull();
            model.BreadCrumb.BreadcrumbItems.Count().Should().Be(3);
            model.BreadCrumb.BreadcrumbItems.ElementAt(0).DisplayName.Should().Be(BreadcrumbContent.Home);
            model.BreadCrumb.BreadcrumbItems.ElementAt(0).RouteName.Should().Be(RouteConstants.Home);
            model.BreadCrumb.BreadcrumbItems.ElementAt(1).DisplayName.Should().Be(BreadcrumbContent.Tlevels_Dashboard);
            model.BreadCrumb.BreadcrumbItems.ElementAt(1).RouteName.Should().Be(RouteConstants.TlevelsDashboard);
            model.BreadCrumb.BreadcrumbItems.ElementAt(2).DisplayName.Should().Be(BreadcrumbContent.Tlevels_Queried_List);
            model.BreadCrumb.BreadcrumbItems.ElementAt(2).RouteName.Should().BeNull();
        }
    }
}
