using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SelectToReview;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.SelectToReviewGet
{
    public class When_ViewModel_IsValid : TestSetup
    {
        private SelectToReviewPageViewModel mockresult;

        public override void Given()
        {
            mockresult = new SelectToReviewPageViewModel 
            { 
                TlevelsToReview  = new List<TlevelToReviewViewModel> 
                {
                    new TlevelToReviewViewModel { PathwayId = 1, TlevelTitle = "Route1: Pathway1"},
                    new TlevelToReviewViewModel { PathwayId = 2, TlevelTitle = "Route2: Pathway2"}
                } 
            };

            TlevelLoader.GetTlevelsToReviewByUkprnAsync(Arg.Any<long>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Called_Expeccted_Methods()
        {
            TlevelLoader.Received().GetTlevelsToReviewByUkprnAsync(ukprn);
        }

        [Fact]
        public void Then_Reeturns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as SelectToReviewPageViewModel;

            model.Should().NotBeNull();
            model.SelectedPathwayId.Should().Be(mockresult.SelectedPathwayId);
            model.TlevelsToReview.Should().NotBeNull();
            model.TlevelsToReview.Count().Should().Be(2);
        }

        [Fact]
        public void Then_Reeturns_Expected_ViewModel()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as SelectToReviewPageViewModel;

            model.TlevelsToReview.Should().NotBeNull();
            model.SelectedPathwayId.Should().Be(0);

            var expectedFirstItemModel = mockresult.TlevelsToReview.FirstOrDefault();
            var actualFirstItemModel = model.TlevelsToReview.FirstOrDefault();
            
            expectedFirstItemModel.PathwayId.Should().Be(actualFirstItemModel.PathwayId);
            expectedFirstItemModel.TlevelTitle.Should().Be(actualFirstItemModel.TlevelTitle);

            // Breadcrumb
            model.BreadCrumb.Should().NotBeNull();
            model.BreadCrumb.BreadcrumbItems.Count().Should().Be(3);
            model.BreadCrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.BreadCrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.BreadCrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Tlevels_Dashboard);
            model.BreadCrumb.BreadcrumbItems[1].RouteName.Should().Be(RouteConstants.TlevelsDashboard);
            model.BreadCrumb.BreadcrumbItems[2].DisplayName.Should().Be(BreadcrumbContent.Tlevel_Review_Select);
            model.BreadCrumb.BreadcrumbItems[2].RouteName.Should().BeNull();
        }
    }
}
