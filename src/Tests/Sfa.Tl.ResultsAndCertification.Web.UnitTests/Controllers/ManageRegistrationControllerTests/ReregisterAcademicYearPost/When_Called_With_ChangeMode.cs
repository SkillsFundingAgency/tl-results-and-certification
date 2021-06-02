using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeLearnersNameGet;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterAcademicYearPost
{
    public class When_Called_With_ChangeMode : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private ReregisterProviderViewModel _reregisterProviderViewModel;
        private ReregisterCoreViewModel _reregisterCoreViewModel;
        private ReregisterSpecialismQuestionViewModel _reregisterSpecialismQuestionViewModel;
        private ReregisterSpecialismViewModel _reregisterSpecialismViewModel;
        private PathwaySpecialismsViewModel _pathwaySpecialismsViewModel;
        private ReregisterAcademicYearViewModel _reregisterAcademicYearViewModel;
        private readonly string _coreCode = "12345678";
        private string _selectedAcademicYear;

        public override void Given()
        {
            _selectedAcademicYear = ((int)AcademicYear.Year2020).ToString();
            _reregisterProviderViewModel = new ReregisterProviderViewModel { SelectedProviderUkprn = "98765432", SelectedProviderDisplayName = "Barnsley College (98765432)" };
            _reregisterCoreViewModel = new ReregisterCoreViewModel { SelectedCoreCode = _coreCode, SelectedCoreDisplayName = $"Education ({_coreCode})", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
            _reregisterSpecialismQuestionViewModel = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
            _pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel { PathwayCode = _coreCode, PathwayName = "Education", Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Code = "7654321", Name = "Test Education", DisplayName = "Test Education (7654321)", IsSelected = true } } };
            _reregisterSpecialismViewModel = new ReregisterSpecialismViewModel { PathwaySpecialisms = _pathwaySpecialismsViewModel };
            _reregisterAcademicYearViewModel = new ReregisterAcademicYearViewModel { SelectedAcademicYear = "2020" };

            cacheResult = new ReregisterViewModel
            {
                ReregisterProvider = _reregisterProviderViewModel,
                ReregisterCore = _reregisterCoreViewModel,
                SpecialismQuestion = _reregisterSpecialismQuestionViewModel,
                ReregisterSpecialisms = _reregisterSpecialismViewModel,
                ReregisterAcademicYear = _reregisterAcademicYearViewModel
            };

            AcademicYearViewModel = new ReregisterAcademicYearViewModel { ProfileId = ProfileId, SelectedAcademicYear = _selectedAcademicYear, IsChangeMode = true };
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_AddRegistrationCheckAndSubmit()
        {
            var route = Result as RedirectToRouteResult;
            var routeName = route.RouteName;
            routeName.Should().Be(RouteConstants.ReregisterCheckAndSubmit);
            route.RouteValues[Constants.ProfileId].Should().Be(AcademicYearViewModel.ProfileId);
        }
    }
}
