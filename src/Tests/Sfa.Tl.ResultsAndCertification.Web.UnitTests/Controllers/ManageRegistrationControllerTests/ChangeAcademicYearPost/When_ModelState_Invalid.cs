using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeAcademicYearPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        protected ChangeAcademicYearViewModel MockResult = null;

        public override void Given()
        {
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
            Controller.ModelState.AddModelError(nameof(ChangeAcademicYearViewModel.AcademicYearChangeTo), Content.Registration.ChangeAcademicYear.Validation_Message);
        }
        public async override Task When()
        {
            Result = await Controller.ChangeAcademicYearAsync(MockResult);
        }

        [Fact]
        void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeAcademicYearViewModel));

            var model = viewResult.Model as ChangeAcademicYearViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(MockResult.ProfileId);
            model.Uln.Should().Be(MockResult.Uln);
            model.Name.Should().Be(MockResult.Name);
            model.ProviderDisplayName.Should().Be(MockResult.ProviderDisplayName);
            model.PathwayDisplayName.Should().Be(MockResult.PathwayDisplayName);
            model.AoUkprn.Should().Be(MockResult.AoUkprn);
            model.AcademicYear.Should().Be(MockResult.AcademicYear);
            model.AcademicYearToBe.Should().Be(MockResult.AcademicYearToBe);
            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(ChangeAcademicYearViewModel.AcademicYearChangeTo)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ChangeAcademicYearViewModel.AcademicYearChangeTo)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.Registration.ChangeAcademicYear.Validation_Message);
        }
    }
}
