using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ViewAll
{
    public class Then_Return_Expected_ViewModel : When_ViewAll_Action_Called
    {
        int expectedPathwayId = 11;
        string expectedTlevelTitle = "Route: Pathway";

        public override void Given()
        {
            var mockresult = new YourTlevelsViewModel 
            {
                IsAnyReviewPending = true,
                ConfirmedTlevels = new List<YourTlevelViewModel> 
                {
                    new YourTlevelViewModel { PathwayId = expectedPathwayId, TlevelTitle = expectedTlevelTitle }
                },
                QueriedTlevels = new List<YourTlevelViewModel>()
            };

            TlevelLoader.GetYourTlevelsViewModel(Arg.Any<long>())
                .Returns(mockresult);
        }

        [Fact]
        public void Then_GetAllTlevelsByUkprnAsync_Is_Called()
        {
            TlevelLoader.Received().GetYourTlevelsViewModel(Arg.Any<long>());
        }

        [Fact]
        public void Then_GetAllTlevelsByUkprnAsync_ViewModel_Return_ExpectedResults()
        {
            var viewResult = Result.Result as ViewResult;
            var model = viewResult.Model as YourTlevelsViewModel;

            model.IsAnyReviewPending.Should().BeTrue();
            model.ConfirmedTlevels.Should().NotBeNull();
            model.ConfirmedTlevels.Count().Should().Be(1);
            model.ConfirmedTlevels.First().PathwayId.Should().Be(expectedPathwayId);
            model.ConfirmedTlevels.First().TlevelTitle.Should().Be(expectedTlevelTitle);
            model.QueriedTlevels.Should().NotBeNull();
            model.QueriedTlevels.Should().BeEmpty();

            // Breadcrumb
            model.BreadCrumb.Should().NotBeNull();
            model.BreadCrumb.BreadcrumbItems.Count().Should().Be(2);
            model.BreadCrumb.BreadcrumbItems.ElementAt(0).DisplayName.Should().Be(BreadcrumbContent.Home);
            model.BreadCrumb.BreadcrumbItems.ElementAt(0).RouteName.Should().Be(RouteConstants.Home);
            model.BreadCrumb.BreadcrumbItems.ElementAt(1).DisplayName.Should().Be(BreadcrumbContent.Tlevel_ViewAll);
            model.BreadCrumb.BreadcrumbItems.ElementAt(1).RouteName.Should().BeNull();
        }
    }
}
