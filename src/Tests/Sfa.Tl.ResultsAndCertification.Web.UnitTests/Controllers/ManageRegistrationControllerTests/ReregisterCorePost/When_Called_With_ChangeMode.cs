using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterCorePost
{
    public class When_Called_With_ChangeMode : TestSetup
    {
        private RegistrationDetailsViewModel _registrationDetailsViewModel = null;
        private ReregisterProviderViewModel _reregisterProviderViewModel;
        private ReregisterAcademicYearViewModel _academicYearViewModel;
        private long _providerUkprn = 987654321;
        private readonly string _coreCodeAtTheTimeOfWithdrawn = "123456789";
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Withdrawn;

        public override void Given()
        {
            IsChangeMode = true;
            ViewModel = new ReregisterCoreViewModel { ProfileId = ProfileId, SelectedCoreCode = "10000057", IsChangeMode = true, CoreCodeAtTheTimeOfWithdrawn = _coreCodeAtTheTimeOfWithdrawn };
            _reregisterProviderViewModel = new ReregisterProviderViewModel { SelectedProviderUkprn = _providerUkprn.ToString(), ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Test Provider", Value = _providerUkprn.ToString() } } };
            _academicYearViewModel = new ReregisterAcademicYearViewModel { SelectedAcademicYear = "2020" };

            var cacheResult = new ReregisterViewModel
            {
                ReregisterProvider = _reregisterProviderViewModel,
                ReregisterAcademicYear = _academicYearViewModel
            };

            _registrationDetailsViewModel = new RegistrationDetailsViewModel
            {
                ProfileId = 1,
                PathwayLarId = _coreCodeAtTheTimeOfWithdrawn,
                Status = _registrationPathwayStatus
            };

            RegistrationLoader.GetRegistrationDetailsAsync(AoUkprn, ProfileId, _registrationPathwayStatus).Returns(_registrationDetailsViewModel);
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_ReregisterSpecialismQuestion()
        {
            var route = Result as RedirectToRouteResult;
            var routeName = route.RouteName;
            routeName.Should().Be(RouteConstants.ReregisterSpecialismQuestion);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
            route.RouteValues[Constants.IsChangeMode].Should().Be(IsChangeMode.ToString().ToLowerInvariant());
        }

        [Fact]
        public void Then_ReregisterViewModel_Reset()
        {
            CacheService.Received(1)
                .SetAsync(CacheKey, Arg.Is<ReregisterViewModel>(x =>
                    x.ReregisterCore == ViewModel &&
                    x.SpecialismQuestion == null &&
                    x.ReregisterSpecialisms == null &&
                    x.ReregisterAcademicYear == _academicYearViewModel
                    ));
        }
    }
}
