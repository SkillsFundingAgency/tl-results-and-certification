using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpSpecialConsiderationReasonsPost
{
    public class When_NoCache_Found : TestSetup
    {
        public override void Given()
        {
            ViewModel = new SpecialConsiderationReasonsViewModel
            {
                AcademicYear = 2020,
                LearnerName = "Test User",
                ReasonsList = new List<IpLookupDataViewModel> { new IpLookupDataViewModel { Id = 1, Name = "Medical", IsSelected = true }, new IpLookupDataViewModel { Id = 2, Name = "Withdrawn", IsSelected = true } }
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
