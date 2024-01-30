using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Registration.Uln;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeStartYearPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        protected AdminChangeStartYearViewModel MockResult = null;

        public override void Given()
        {
            AdminChangeStartYearViewModel = new AdminChangeStartYearViewModel()
            {
                PathwayId = 1,
                FirstName = "John",
                LastName = "Smith",
                Uln = 1100000001,
                ProviderName = "Barnsley College",
                ProviderUkprn = 10000536,
                TlevelName = "T Level in Digital Support Services",
                TlevelStartYear = 2021,
                AcademicYear = 2022,
                DisplayAcademicYear = "2021 to 2022",
                AcademicStartYearsToBe = new List<int> { 2020 }
            };

            MockResult = new AdminChangeStartYearViewModel()
            {
                PathwayId = 1,
                FirstName = "John",
                LastName = "Smith",
                Uln = 1100000001,
                ProviderName = "Barnsley College",
                ProviderUkprn = 10000536,
                TlevelName = "T Level in Digital Support Services",
                TlevelStartYear = 2021,
                AcademicYear = 2022,
                DisplayAcademicYear = "2021 to 2022",
                AcademicStartYearsToBe = new List<int>() { 2020 }
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminChangeStartYearViewModel>(Arg.Any<int>()).Returns(MockResult);
            Controller.ModelState.AddModelError(nameof(AdminChangeStartYearViewModel.AcademicYearTo), Content.AdminDashboard.ChangeStartYear.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AdminChangeStartYearViewModel));

            var model = viewResult.Model as AdminChangeStartYearViewModel;

            model.Should().NotBeNull();
            model.PathwayId.Should().Be(AdminChangeStartYearViewModel.PathwayId);
            model.FirstName.Should().Be(AdminChangeStartYearViewModel.FirstName);
            model.LastName.Should().Be(AdminChangeStartYearViewModel.LastName);
            model.Uln.Should().Be(AdminChangeStartYearViewModel.Uln);
            model.ProviderName.Should().Be(AdminChangeStartYearViewModel.ProviderName);
            model.ProviderUkprn.Should().Be(AdminChangeStartYearViewModel.ProviderUkprn);
            model.TlevelName.Should().Be(AdminChangeStartYearViewModel.TlevelName);
            model.TlevelStartYear.Should().Be(AdminChangeStartYearViewModel.TlevelStartYear);
            model.AcademicYear.Should().Be(AdminChangeStartYearViewModel.AcademicYear);
            model.DisplayAcademicYear.Should().Be(AdminChangeStartYearViewModel.DisplayAcademicYear);
            model.AcademicStartYearsToBe.Should().NotBeEmpty().And.HaveCount(1).And.ContainItemsAssignableTo<int>();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(AdminChangeStartYearViewModel.AcademicYearTo)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(AdminChangeStartYearViewModel.AcademicYearTo)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.AdminDashboard.ChangeStartYear.Validation_Message);
        }
    }
}
