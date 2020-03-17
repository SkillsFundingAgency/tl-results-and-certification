using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TlevelControllerTests.ConfirmTlevel
{
    public class Then_ModelState_Invalid_Returned_Verify_View : When_ConfirmTlevel_Action_Is_Called
    {
        private readonly int pathwayId = 99;
        private ConfirmTlevelViewModel expectedResult;

        public override void Given()
        {
            expectedResult = new ConfirmTlevelViewModel
            {
                PathwayId = pathwayId,
                PathwayName = "PathName",
                RouteId = 5,
                IsEverythingCorrect = false,
                PathwayStatusId = 1,
                TqAwardingOrganisationId = 7,
                Specialisms = new List<string> { "spl1", "spl2" }
            };


            InputModel = new ConfirmTlevelViewModel { PathwayStatusId = (int)TlevelReviewStatus.AwaitingConfirmation, PathwayId = pathwayId };
            TlevelLoader.GetVerifyTlevelDetailsByPathwayIdAsync(ukprn, pathwayId)
                .Returns(expectedResult);

            Controller.ModelState.AddModelError("IsEverythingCorrect", "Select yes if this T Level’s details are correct");
        }

        [Fact]
        public void Then_On_ModelState_Not_Valid_Verify_View_Is_Returned()
        {
            var actualView = (Result.Result as ViewResult);
            actualView.Should().NotBeNull();
            actualView.ViewName.Should().Be("Verify");
        }

        [Fact]
        public void Then_On_ModelState_Not_Valid_Expected_ViewModel_Is_Returned()
        {
            var actualViewModel = (ConfirmTlevelViewModel) (Result.Result as ViewResult).Model;
            
            actualViewModel.PathwayId.Should().Be(expectedResult.PathwayId);
            actualViewModel.PathwayName.Should().Be(expectedResult.PathwayName);
            actualViewModel.PathwayStatusId.Should().Be(expectedResult.PathwayStatusId);
            actualViewModel.RouteId.Should().Be(expectedResult.RouteId);
            actualViewModel.IsEverythingCorrect.Should().Be(expectedResult.IsEverythingCorrect);
            actualViewModel.TqAwardingOrganisationId.Should().Be(expectedResult.TqAwardingOrganisationId);
            actualViewModel.Specialisms.Count().Should().Be(expectedResult.Specialisms.Count());
            actualViewModel.Specialisms.First().Should().Be(expectedResult.Specialisms.First());
        }
    }
}
