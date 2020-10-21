using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterCoreGet
{
    public class When_Cache_Found : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private RegistrationDetailsViewModel mockresult = null;
        private ReregisterProviderViewModel _reRegisterProviderViewModel;
        private ReregisterCoreViewModel _reregisterCoreViewModel;
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Withdrawn;
        private readonly long _providerUkprn = 987654321;

        public override void Given()
        {
            mockresult = new RegistrationDetailsViewModel
            {
                ProfileId = 1,
                Status = _registrationPathwayStatus
            };

            _reregisterCoreViewModel = new ReregisterCoreViewModel { SelectedCoreCode = "123", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = "123" } } };
            _reRegisterProviderViewModel = new ReregisterProviderViewModel { ProfileId = ProfileId, SelectedProviderUkprn = _providerUkprn.ToString() };

            cacheResult = new ReregisterViewModel { ReregisterProvider = _reRegisterProviderViewModel, ReregisterCore = _reregisterCoreViewModel };
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);

            RegistrationLoader.GetRegisteredProviderPathwayDetailsAsync(AoUkprn, _providerUkprn).Returns(_reregisterCoreViewModel);
            RegistrationLoader.GetRegistrationDetailsAsync(AoUkprn, ProfileId, _registrationPathwayStatus).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegisteredProviderPathwayDetailsAsync(AoUkprn, _providerUkprn);
            RegistrationLoader.Received(1).GetRegistrationDetailsAsync(AoUkprn, ProfileId, _registrationPathwayStatus);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ReregisterCoreViewModel));

            var model = viewResult.Model as ReregisterCoreViewModel;
            model.Should().NotBeNull();

            model.SelectedCoreCode.Should().Be(_reregisterCoreViewModel.SelectedCoreCode);
            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.CoreSelectList.Should().NotBeNull();
            model.CoreSelectList.Count.Should().Be(_reregisterCoreViewModel.CoreSelectList.Count);

            model.BackLink.Should().NotBeNull();

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.ReregisterProvider);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
