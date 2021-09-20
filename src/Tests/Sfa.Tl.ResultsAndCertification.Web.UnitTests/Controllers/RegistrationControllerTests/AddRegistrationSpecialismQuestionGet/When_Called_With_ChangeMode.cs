using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationSpecialismQuestionGet
{
    public class When_Called_With_ChangeMode : TestSetup
    {
        private RegistrationViewModel cacheResult;
        private UlnViewModel _ulnViewModel;
        private LearnersNameViewModel _learnersNameViewModel;
        private DateofBirthViewModel _dateofBirthViewModel;
        private SelectProviderViewModel _selectProviderViewModel;
        private SelectCoreViewModel _selectCoreViewModel;
        private SpecialismQuestionViewModel _specialismQuestionViewModel;
        private SelectAcademicYearViewModel _academicYearViewModel;
        private string _coreCode = "12345678";
        private string _selectedAcademicYear;

        public override void Given()
        {
            IsChangeMode = true;
            //_selectedAcademicYear = ((int)AcademicYearDelete.Year2020).ToString();
            _ulnViewModel = new UlnViewModel { Uln = "1234567890" };
            _learnersNameViewModel = new LearnersNameViewModel { Firstname = "First", Lastname = "Last" };
            _dateofBirthViewModel = new DateofBirthViewModel { Day = "01", Month = "01", Year = "2020" };
            _selectProviderViewModel = new SelectProviderViewModel { SelectedProviderUkprn = "98765432", SelectedProviderDisplayName = "Barnsley College (98765432)" };
            _selectCoreViewModel = new SelectCoreViewModel { SelectedCoreCode = _coreCode, SelectedCoreDisplayName = $"Education ({_coreCode})", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
            _specialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
            _academicYearViewModel = new SelectAcademicYearViewModel { SelectedAcademicYear = "2020" };

            cacheResult = new RegistrationViewModel
            {
                Uln = _ulnViewModel,
                LearnersName = _learnersNameViewModel,
                DateofBirth = _dateofBirthViewModel,
                SelectProvider = _selectProviderViewModel,
                SelectCore = _selectCoreViewModel,
                SpecialismQuestion = _specialismQuestionViewModel,
                SelectAcademicYear = _academicYearViewModel
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SpecialismQuestionViewModel));

            var model = viewResult.Model as SpecialismQuestionViewModel;
            model.Should().NotBeNull();

            model.HasLearnerDecidedSpecialism.Should().BeTrue();
            model.IsChangeMode.Should().Be(IsChangeMode);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddRegistrationCheckAndSubmit);
        }
    }
}
