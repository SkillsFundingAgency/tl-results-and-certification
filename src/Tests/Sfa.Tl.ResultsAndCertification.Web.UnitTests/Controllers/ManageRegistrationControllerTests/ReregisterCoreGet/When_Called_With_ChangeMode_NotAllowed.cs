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
    public class When_Called_With_ChangeMode_NotAllowed : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private ReregisterProviderViewModel _reregisterProviderViewModel;
        private ReregisterCoreViewModel _reregisterCoreViewModel;
        private RegistrationDetailsViewModel _registrationDetailsViewModel = null;
        private long _providerUkprn = 987654321;
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Withdrawn;

        public override void Given()
        {
            IsChangeMode = true;
            _reregisterProviderViewModel = new ReregisterProviderViewModel { IsChangeMode = true, SelectedProviderUkprn = _providerUkprn.ToString(), SelectedProviderDisplayName = "Barnsley College (98765432)", ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Barnsley College (98765432)", Value = _providerUkprn.ToString() } } };
            _reregisterCoreViewModel = new ReregisterCoreViewModel { SelectedCoreCode = "123", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = "123" } } };

            cacheResult = new ReregisterViewModel
            {
                ReregisterProvider = _reregisterProviderViewModel,
                ReregisterCore = _reregisterCoreViewModel
            };

            _registrationDetailsViewModel = new RegistrationDetailsViewModel
            {
                ProfileId = 1,
                Status = _registrationPathwayStatus
            };

            RegistrationLoader.GetRegisteredProviderPathwayDetailsAsync(AoUkprn, _providerUkprn).Returns(_reregisterCoreViewModel);
            RegistrationLoader.GetRegistrationDetailsAsync(AoUkprn, ProfileId, _registrationPathwayStatus).Returns(_registrationDetailsViewModel);
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
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
            model.CoreSelectList.Should().NotBeNull();
            model.CoreSelectList.Count.Should().Be(_reregisterCoreViewModel.CoreSelectList.Count);
            model.IsChangeMode.Should().BeFalse();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.ReregisterProvider);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(model.ProfileId.ToString());
        }
    }
}
