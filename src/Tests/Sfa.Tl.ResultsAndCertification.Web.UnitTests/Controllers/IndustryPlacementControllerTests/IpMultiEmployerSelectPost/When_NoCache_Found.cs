using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpMultiEmployerSelectPost
{
    public class When_NoCache_Found : TestSetup
    {
        public override void Given()
        {
            ViewModel = new IpMultiEmployerSelectViewModel
            {
                LearnerName = "First Last",
                PlacementModels = new List<IpLookupDataViewModel>
                {
                    new IpLookupDataViewModel { Id = 1, Name = "Test", IsSelected = true },
                    new IpLookupDataViewModel { Id = 2, Name = "New", IsSelected = true }
                }
            };
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
