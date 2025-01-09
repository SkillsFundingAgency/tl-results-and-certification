using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeAcademicYearGet
{
    public class When_Called_With_Active_Results : TestSetup
    {
        private IActionResult Result;
        private ChangeAcademicYearViewModel mockresult;

        public override void Given()
        {
            mockresult = new ChangeAcademicYearViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                AoUkprn = AoUkprn,
                Name = "John Smith",
                ProviderDisplayName = "Barnsley College (10000536)",
                PathwayDisplayName = "Digital Business Services",
                AcademicYear = 2020,
                AcademicYearToBe = 2021,
                HasActiveAssessmentResults = true
            };

            RegistrationLoader.GetAcademicYearsAsync().Returns(new List<AcademicYear> {
                new AcademicYear { Year = 2020, Name = "2020/21" }
                , new AcademicYear { Year = 2021, Name = "2021/22" } });

            RegistrationLoader.GetRegistrationProfileAsync<ChangeAcademicYearViewModel>(AoUkprn, ProfileId).Returns(mockresult);
        }

        public async override Task When()
        {
            Result = await Controller.ChangeAcademicYearAsync(ProfileId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            RegistrationLoader.GetRegistrationProfileAsync<ChangeAcademicYearViewModel>(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectToRouteResult(RouteConstants.CannotChangeAcademicYear, ("profileId", ProfileId));
        }
    }
}