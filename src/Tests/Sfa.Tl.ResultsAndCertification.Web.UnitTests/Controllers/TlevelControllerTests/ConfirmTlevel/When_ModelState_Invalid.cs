using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ConfirmTlevel
{
    public class When_ModelState_Invalid : TestSetup
    {
        private readonly int pathwayId = 99;
        private ConfirmTlevelViewModel expectedResult;
        
        public override void Given()
        {
            expectedResult = new ConfirmTlevelViewModel
            {
                PathwayId = pathwayId,
                RouteId = 5,
                IsEverythingCorrect = false,
                PathwayStatusId = 1,
                TqAwardingOrganisationId = 7,
                TlevelTitle = "Tlevel title",
                PathwayDisplayName = "Pathway1<br/>(45789465489)",
                Specialisms = new List<string> { "sp1<br/>(567565)", "sp2<br/>(564547)" }
            };

            InputModel = new ConfirmTlevelViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation, PathwayId = pathwayId };
            TlevelLoader.GetVerifyTlevelDetailsByPathwayIdAsync(AoUkprn, pathwayId)
                .Returns(expectedResult);

            Controller.ModelState.AddModelError("IsEverythingCorrect", "Select yes if this T Level’s details are correct");
        }

        [Fact]
        public void Then_Returns_Expected_View()
        {
            var actualView = (Result as ViewResult);
            actualView.Should().NotBeNull();
            actualView.ViewName.Should().Be("Verify");
        }

        [Fact]
        public void Then_Returns_Expected_ViewModel()
        {
            var actualViewModel = (ConfirmTlevelViewModel) (Result as ViewResult).Model;
            actualViewModel.PathwayId.Should().Be(expectedResult.PathwayId);
            actualViewModel.PathwayDisplayName.Should().Be(expectedResult.PathwayDisplayName);
            actualViewModel.PathwayStatusId.Should().Be(expectedResult.PathwayStatusId);
            actualViewModel.RouteId.Should().Be(expectedResult.RouteId);
            actualViewModel.IsEverythingCorrect.Should().Be(expectedResult.IsEverythingCorrect);
            actualViewModel.TqAwardingOrganisationId.Should().Be(expectedResult.TqAwardingOrganisationId);
            actualViewModel.TlevelTitle.Should().Be(expectedResult.TlevelTitle);
            actualViewModel.Specialisms.Count().Should().Be(expectedResult.Specialisms.Count());
            actualViewModel.Specialisms.Should().BeEquivalentTo(expectedResult.Specialisms);
        }
    }
}
