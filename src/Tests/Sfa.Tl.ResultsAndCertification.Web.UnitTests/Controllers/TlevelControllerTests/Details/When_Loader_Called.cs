﻿using FluentAssertions;
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
    public class When_Loader_Called : TestSetup
    {
        private TLevelConfirmedDetailsViewModel mockresult;

        public override void Given()
        {
            mockresult = new TLevelConfirmedDetailsViewModel 
            { 
                IsValid = true,
                Specialisms = new List<string> { "Specialism1", "Specialism2" }
            };

            TlevelLoader.GetTlevelDetailsByPathwayIdAsync(AoUkprn, id)
                .Returns(mockresult);
        }

        [Fact]
        public void Then_Called_GetTlevelDetailsByPathwayId()
        {
            TlevelLoader.Received().GetTlevelDetailsByPathwayIdAsync(AoUkprn, id);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var actualResult = viewResult.Model as TLevelConfirmedDetailsViewModel;

            actualResult.IsValid.Should().Be(mockresult.IsValid);
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
