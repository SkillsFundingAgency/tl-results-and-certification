using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeAcademicYearPost
{
    public class When_Success : TestSetup
    {
        private ChangeAcademicYearViewModel MockResult = null;

        public override void Given()
        {
            MockResult = new ChangeAcademicYearViewModel
            {
                ProfileId = 1,
            };

            MockResult = new ChangeAcademicYearViewModel
            {
                ProfileId = ProfileId,
                Uln = 1234567890,
                AoUkprn = AoUkprn,
                Name = "John Smith",
                ProviderDisplayName = "Barnsley College (10000536)",
                PathwayDisplayName = "T Level in Digital Support Services",
                AcademicYear = 2022,
                AcademicYearToBe = 2023,
                AcademicYears = new List<AcademicYear>
                {
                    new() { Year = 2022 },
                    new() { Year = 2023 }
                }
            };
        }

        public async override Task When()
        {
            Result = await Controller.ChangeAcademicYearAsync(MockResult);
        }

        [Fact]
        public void Then_Redirected_To_AssessmentDetails()
        {
            var route = Result as RedirectToActionResult;
            route.ActionName.Should().Be(nameof(RouteConstants.ReviewChangeAcademicYear));
            route.RouteValues[Constants.ProfileId].Should().Be(MockResult.ProfileId);
        }
    }
}
