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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterCheckAndSubmitGet
{
    public class When_Cache_Found_And_Incorrect_Status : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private ReregisterProviderViewModel _reregisterProviderViewModel;
        private ReregisterCoreViewModel _reregisterCoreViewModel;
        private ReregisterSpecialismQuestionViewModel _reregisterSpecialismQuestionViewModel;
        private ReregisterSpecialismViewModel _reregisterSpecialismViewModel;
        private PathwaySpecialismsViewModel _pathwaySpecialismsViewModel;
        private ReregisterAcademicYearViewModel _academicYearViewModel;
        private readonly string _coreCode = "12345678";
        private readonly long _uln = 1234567890;
        private RegistrationDetailsViewModel _registrationDetails = null;
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Active;

        public override void Given()
        {
            _reregisterProviderViewModel = new ReregisterProviderViewModel { SelectedProviderUkprn = "98765432", SelectedProviderDisplayName = "Barnsley College (98765432)" };
            _reregisterCoreViewModel = new ReregisterCoreViewModel { SelectedCoreCode = _coreCode, SelectedCoreDisplayName = $"Education ({_coreCode})", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
            _reregisterSpecialismQuestionViewModel = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
            _pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel { PathwayCode = _coreCode, PathwayName = "Education", Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Code = "7654321", Name = "Test Education", DisplayName = "Test Education (7654321)", IsSelected = true } } };
            _reregisterSpecialismViewModel = new ReregisterSpecialismViewModel { PathwaySpecialisms = _pathwaySpecialismsViewModel };
            _academicYearViewModel = new ReregisterAcademicYearViewModel { ProfileId = ProfileId, SelectedAcademicYear = "2020" };

            cacheResult = new ReregisterViewModel
            {
                ReregisterProvider = _reregisterProviderViewModel,
                ReregisterCore = _reregisterCoreViewModel,
                SpecialismQuestion = _reregisterSpecialismQuestionViewModel,
                ReregisterSpecialisms = _reregisterSpecialismViewModel,
                ReregisterAcademicYear = _academicYearViewModel
            };

            _registrationDetails = new RegistrationDetailsViewModel
            {
                ProfileId = 1,
                Uln = _uln,
                Status = _registrationPathwayStatus
            };

            RegistrationLoader.GetRegistrationDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn).Returns(_registrationDetails);
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
