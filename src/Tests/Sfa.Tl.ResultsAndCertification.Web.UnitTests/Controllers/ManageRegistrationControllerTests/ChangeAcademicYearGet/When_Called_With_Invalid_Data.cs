using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Threading.Tasks;
using Xunit;
namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeAcademicYearGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private IActionResult Result;

        public override void Given()
        {
            RegistrationLoader.GetRegistrationProfileAsync<ChangeAcademicYearViewModel>(AoUkprn, ProfileId).Returns(null as ChangeAcademicYearViewModel);
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
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}