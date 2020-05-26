using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.Details
{
    public class Then_TlevelLoader_Returns_Expected_Result : When_Details_Action_Called
    {
        private TLevelDetailsViewModel mockresult;

        public override void Given()
        {
            mockresult = new TLevelDetailsViewModel 
            { 
                PageTitle = "Tlevel Details", 
                PathwayName = "Education", 
                ShowSomethingIsNotRight = true,
                RouteName = "Digital Education", 
                Specialisms = new List<string> { "Specialism1", "Specialism2" }
            };

            TlevelLoader.GetTlevelDetailsByPathwayIdAsync(ukPrn, id)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_GetTlevelDetailsByPathwayIdAsync_Is_Called()
        {
            TlevelLoader.Received().GetTlevelDetailsByPathwayIdAsync(ukPrn, id);
        }

        [Fact]
        public void Then_Expected_Result_Is_Returned()
        {
            var viewResult = Result.Result as ViewResult;
            var actualResult = viewResult.Model as TLevelDetailsViewModel;

            actualResult.PageTitle.Should().Be(mockresult.PageTitle);
            actualResult.PathwayName.Should().Be(mockresult.PathwayName);
            actualResult.ShowSomethingIsNotRight.Should().Be(mockresult.ShowSomethingIsNotRight);
            actualResult.RouteName.Should().Be(mockresult.RouteName);
            actualResult.Specialisms.Count().Should().Be(2);
            actualResult.Specialisms.First().Should().Be(mockresult.Specialisms.First());

            // Breadcrumb
            actualResult.BreadCrumb.Should().NotBeNull();
            actualResult.BreadCrumb.BreadcrumbItems.Count().Should().Be(3);
            actualResult.BreadCrumb.BreadcrumbItems.ElementAt(0).DisplayName.Should().Be(BreadcrumbContent.Home);
            actualResult.BreadCrumb.BreadcrumbItems.ElementAt(0).RouteName.Should().Be(RouteConstants.Home);
            actualResult.BreadCrumb.BreadcrumbItems.ElementAt(1).DisplayName.Should().Be(BreadcrumbContent.Tlevel_ViewAll);
            actualResult.BreadCrumb.BreadcrumbItems.ElementAt(1).RouteName.Should().Be(RouteConstants.YourTlevels);
            actualResult.BreadCrumb.BreadcrumbItems.ElementAt(2).DisplayName.Should().Be(BreadcrumbContent.Tlevel_Details);
            actualResult.BreadCrumb.BreadcrumbItems.ElementAt(2).RouteName.Should().BeNull();
        }
    }
}
