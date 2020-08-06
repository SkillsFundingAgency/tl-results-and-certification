using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationSpecialismQuestionPost
{
    public class Then_On_ChangeMode_Redirected_To_CheckAndSubmit_Route : When_AddRegistrationSpecialismQuestionAsync_Action_Is_Called
    {
        private RegistrationViewModel cacheResult;
        private UlnViewModel _ulnViewModel;
        private LearnersNameViewModel _learnersNameViewModel;
        private DateofBirthViewModel _dateofBirthViewModel;
        private SelectProviderViewModel _selectProviderViewModel;
        private SelectCoreViewModel _selectCoreViewModel;
        private SpecialismQuestionViewModel _specialismQuestionViewModel;
        //private PathwaySpecialismsViewModel _pathwaySpecialismsViewModel;
        private SelectAcademicYearViewModel _academicYearViewModel;
        private string _coreCode = "12345678";
        private string _selectedAcademicYear;

        public override void Given()
        {
            _selectedAcademicYear = ((int)AcademicYear.Year2020).ToString();
            _ulnViewModel = new UlnViewModel { Uln = "1234567890" };
            _learnersNameViewModel = new LearnersNameViewModel { Firstname = "First", Lastname = "Last" };
            _dateofBirthViewModel = new DateofBirthViewModel { Day = "01", Month = "01", Year = "2020" };
            _selectProviderViewModel = new SelectProviderViewModel { SelectedProviderUkprn = "98765432", SelectedProviderDisplayName = "Barnsley College (98765432)" };
            _selectCoreViewModel = new SelectCoreViewModel { SelectedCoreCode = _coreCode, SelectedCoreDisplayName = $"Education ({_coreCode})", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
            _specialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
            //_pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel { PathwayCode = _coreCode, PathwayName = "Education", Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Code = "7654321", Name = "Test Education", DisplayName = "Test Education (7654321)", IsSelected = true } } };
            _academicYearViewModel = new SelectAcademicYearViewModel { SelectedAcademicYear = _selectedAcademicYear.ToString() };

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

            SpecialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false, IsChangeMode = true };
            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_AddRegistrationCheckAndSubmit_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationCheckAndSubmit);
        }
    }
}
