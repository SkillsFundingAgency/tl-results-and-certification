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
    public class When_SelectedCore_Valid : TestSetup
    {
        private readonly long _providerUkprn = 987654321;
        private readonly string _coreCodeAtTheTimeOfWithdrawn = "123456789";
        private readonly string _selectedCoreCode = "9999999999";
        private ReregisterProviderViewModel _reregisterProviderViewModel;
        private SelectCoreViewModel _selectCoreViewModel;
        private RegistrationDetailsViewModel _registrationDetailsViewModel;        
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Withdrawn;

        public override void Given()
        {
            _reregisterProviderViewModel = new ReregisterProviderViewModel { ProfileId = ProfileId, SelectedProviderUkprn = _providerUkprn.ToString(), ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Hello", Value = "1" } } };
            _selectCoreViewModel = new SelectCoreViewModel { CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = "123" } } };
            _registrationDetailsViewModel = new RegistrationDetailsViewModel
            {
                ProfileId = ProfileId,
                PathwayLarId = _coreCodeAtTheTimeOfWithdrawn,
                Status = _registrationPathwayStatus
            };

            var cacheResult = new ReregisterViewModel
            {
                ReregisterProvider = _reregisterProviderViewModel
            };

            ViewModel.ProfileId = ProfileId;
            ViewModel.SelectedCoreCode = _selectedCoreCode;

            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
            RegistrationLoader.GetRegisteredProviderPathwayDetailsAsync(AoUkprn, _providerUkprn).Returns(_selectCoreViewModel);            
            RegistrationLoader.GetRegistrationDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn).Returns(_registrationDetailsViewModel);
        }

        [Fact]
        public void Then_Redirected_To_ReregisterSpecialismQuestion()
        {
            var route = Result as RedirectToRouteResult;
            var routeName = route.RouteName;
            routeName.Should().Be(RouteConstants.ReregisterSpecialismQuestion);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
