using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterCheckAndSubmitGet
{
    public class ReregisterCheckAndSubmitTestDataGenerator : IEnumerable<object[]>
    {
        private static readonly string _coreCode = "12345678";
        private static readonly ReregisterProviderViewModel _reregisterProviderViewModel = new ReregisterProviderViewModel { SelectedProviderUkprn = "98765432", SelectedProviderDisplayName = "Barnsley College (98765432)" };
        private static readonly ReregisterCoreViewModel _reregisterCoreViewModel = new ReregisterCoreViewModel { SelectedCoreCode = _coreCode, SelectedCoreDisplayName = $"Education ({_coreCode})", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
        private static readonly ReregisterSpecialismQuestionViewModel _reregisterSpecialismQuestionViewModel = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
        private static readonly PathwaySpecialismsViewModel _pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel { PathwayCode = _coreCode, PathwayName = "Education", Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Code = "7654321", Name = "Test Education", DisplayName = "Test Education (7654321)", IsSelected = true } } };
        private static readonly ReregisterSpecialismViewModel _reregisterSpecialismViewModel = new ReregisterSpecialismViewModel { PathwaySpecialisms = _pathwaySpecialismsViewModel };

        private readonly List<object[]> _data = new List<object[]>
        {
               new object[] { new ReregisterCheckAndSubmitTestDataModel { ProfileId = 1, ReregisterViewModel = new ReregisterViewModel(), RouteName = RouteConstants.PageNotFound} },
               new object[] { new ReregisterCheckAndSubmitTestDataModel { ProfileId = 1, ReregisterViewModel = new ReregisterViewModel { ReregisterProvider = _reregisterProviderViewModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new ReregisterCheckAndSubmitTestDataModel { ProfileId = 1, ReregisterViewModel = new ReregisterViewModel { ReregisterProvider = _reregisterProviderViewModel, ReregisterCore = _reregisterCoreViewModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new ReregisterCheckAndSubmitTestDataModel { ProfileId = 1, ReregisterViewModel = new ReregisterViewModel { ReregisterProvider = _reregisterProviderViewModel, ReregisterCore = _reregisterCoreViewModel, SpecialismQuestion = _reregisterSpecialismQuestionViewModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new ReregisterCheckAndSubmitTestDataModel { ProfileId = 1, ReregisterViewModel = new ReregisterViewModel { ReregisterProvider = _reregisterProviderViewModel, ReregisterCore = _reregisterCoreViewModel, SpecialismQuestion = _reregisterSpecialismQuestionViewModel, ReregisterSpecialisms = _reregisterSpecialismViewModel }, RouteName = RouteConstants.PageNotFound} },
               new object[] { new ReregisterCheckAndSubmitTestDataModel { ProfileId = 1, ReregisterViewModel = new ReregisterViewModel { ReregisterProvider = _reregisterProviderViewModel, ReregisterCore = _reregisterCoreViewModel, SpecialismQuestion = _reregisterSpecialismQuestionViewModel, ReregisterSpecialisms = _reregisterSpecialismViewModel, ReregisterAcademicYear = null }, RouteName = RouteConstants.PageNotFound} }
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
