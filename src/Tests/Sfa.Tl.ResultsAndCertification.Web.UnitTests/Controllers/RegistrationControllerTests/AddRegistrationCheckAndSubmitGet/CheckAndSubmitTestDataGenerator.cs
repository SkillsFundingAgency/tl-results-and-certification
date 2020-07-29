using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationCheckAndSubmitGet
{
    public class CheckAndSubmitTestDataGenerator : IEnumerable<object[]>
    {
        private static readonly string _coreCode = "12345678";
        private static readonly UlnViewModel _ulnViewModel = new UlnViewModel { Uln = "1234567890" };
        private static readonly LearnersNameViewModel _learnersNameViewModel = new LearnersNameViewModel { Firstname = "First", Lastname = "Last" };
        private static readonly DateofBirthViewModel _dateofBirthViewModel = new DateofBirthViewModel { Day = "01", Month = "01", Year = "2020" };
        private static readonly SelectProviderViewModel _selectProviderViewModel = new SelectProviderViewModel { SelectedProviderUkprn = "98765432", SelectedProviderDisplayName = "Barnsley College (98765432)" };
        private static readonly SelectCoreViewModel _selectCoreViewModel = new SelectCoreViewModel { SelectedCoreCode = _coreCode, SelectedCoreDisplayName = $"Education ({_coreCode})", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
        private static readonly SpecialismQuestionViewModel _specialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
        private static readonly PathwaySpecialismsViewModel _pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel { PathwayCode = _coreCode, PathwayName = "Education", Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Code = "7654321", Name = "Test Education", DisplayName = "Test Education (7654321)", IsSelected = true } } };
        private static readonly SelectSpecialismViewModel _selectSpecialismViewModel = new SelectSpecialismViewModel { PathwaySpecialisms = _pathwaySpecialismsViewModel };

        private readonly List<object[]> _data = new List<object[]>
        {
               new object[] { new CheckAndSubmitTestDataModel { RegistrationViewModel = new RegistrationViewModel(), RouteName = RouteConstants.PageNotFound} },
               new object[] { new CheckAndSubmitTestDataModel { RegistrationViewModel = new RegistrationViewModel { Uln = _ulnViewModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new CheckAndSubmitTestDataModel { RegistrationViewModel = new RegistrationViewModel { Uln = _ulnViewModel, LearnersName = _learnersNameViewModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new CheckAndSubmitTestDataModel { RegistrationViewModel = new RegistrationViewModel { Uln = _ulnViewModel, LearnersName = _learnersNameViewModel, DateofBirth = _dateofBirthViewModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new CheckAndSubmitTestDataModel { RegistrationViewModel = new RegistrationViewModel { Uln = _ulnViewModel, LearnersName = _learnersNameViewModel, DateofBirth = _dateofBirthViewModel, SelectProvider = _selectProviderViewModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new CheckAndSubmitTestDataModel { RegistrationViewModel = new RegistrationViewModel { Uln = _ulnViewModel, LearnersName = _learnersNameViewModel, DateofBirth = _dateofBirthViewModel, SelectProvider = _selectProviderViewModel, SelectCore = _selectCoreViewModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new CheckAndSubmitTestDataModel { RegistrationViewModel = new RegistrationViewModel { Uln = _ulnViewModel, LearnersName = _learnersNameViewModel, DateofBirth = _dateofBirthViewModel, SelectProvider = _selectProviderViewModel, SelectCore = _selectCoreViewModel, SpecialismQuestion = _specialismQuestionViewModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new CheckAndSubmitTestDataModel { RegistrationViewModel = new RegistrationViewModel { Uln = _ulnViewModel, LearnersName = _learnersNameViewModel, DateofBirth = _dateofBirthViewModel, SelectProvider = _selectProviderViewModel, SelectCore = _selectCoreViewModel, SpecialismQuestion = _specialismQuestionViewModel, SelectSpecialism = _selectSpecialismViewModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new CheckAndSubmitTestDataModel { RegistrationViewModel = new RegistrationViewModel { Uln = _ulnViewModel, LearnersName = _learnersNameViewModel, DateofBirth = _dateofBirthViewModel, SelectProvider = _selectProviderViewModel, SelectCore = _selectCoreViewModel, SpecialismQuestion = _specialismQuestionViewModel, SelectSpecialism = _selectSpecialismViewModel, SelectAcademicYear = null }, RouteName = RouteConstants.PageNotFound} }
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
