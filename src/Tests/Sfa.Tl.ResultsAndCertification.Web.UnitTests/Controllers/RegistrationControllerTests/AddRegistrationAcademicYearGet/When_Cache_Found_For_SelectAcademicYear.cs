using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationAcademicYearGet
{
    public class When_Cache_Found_For_SelectAcademicYear : TestSetup
    {
        private RegistrationViewModel cacheResult;
        private SpecialismQuestionViewModel _specialismQuestionViewModel;
        private SelectAcademicYearViewModel _selectAcademicYearViewModel;
        private IList<AcademicYear> _academicYears;
        private string _selectedAcademicYear;

        public override void Given()
        {
            _selectedAcademicYear = "2020".ToString();
            _academicYears = new List<AcademicYear> { new AcademicYear { Id = 1, Name = "2020/21", Year = 2020 } };
            _specialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false };
            _selectAcademicYearViewModel = new SelectAcademicYearViewModel { SelectedAcademicYear = _selectedAcademicYear, AcademicYears = _academicYears };
            cacheResult = new RegistrationViewModel
            {
                SpecialismQuestion = _specialismQuestionViewModel,
                SelectAcademicYear = _selectAcademicYearViewModel
            };
            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SelectAcademicYearViewModel));

            var model = viewResult.Model as SelectAcademicYearViewModel;
            model.Should().NotBeNull();

            model.SelectedAcademicYear.Should().Be(_selectedAcademicYear);
            model.IsValidAcademicYear.Should().BeTrue();
            model.AcademicYears.Should().BeEquivalentTo(_academicYears);
            model.AcademicYearSelectList.Should().BeEquivalentTo(_academicYears.Select(a => new SelectListItem { Text = a.Name, Value = a.Year.ToString() }));
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddRegistrationSpecialismQuestion);
        }
    }
}
