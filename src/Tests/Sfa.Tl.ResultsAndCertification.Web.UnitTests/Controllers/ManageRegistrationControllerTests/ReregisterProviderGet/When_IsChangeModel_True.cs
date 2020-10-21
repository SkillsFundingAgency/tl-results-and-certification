using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterProviderGet
{
    public class When_IsChangeModel_True : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private SelectProviderViewModel _selectProviderViewModel;
        private RegistrationDetailsViewModel mockresult = null;
        private ReregisterProviderViewModel _reRegisterProviderViewModel;
        private RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Withdrawn;

        public override void Given()
        {
            IsChangeMode = true;
            mockresult = new RegistrationDetailsViewModel
            {
                ProfileId = 1,
                Status = _registrationPathwayStatus
            };

            _selectProviderViewModel = new SelectProviderViewModel { ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Hello", Value = "1" } } };
            _reRegisterProviderViewModel = new ReregisterProviderViewModel { ProfileId = ProfileId, SelectedProviderUkprn = "12345678" };
            
            cacheResult = new ReregisterViewModel 
            { 
                ReregisterProvider = _reRegisterProviderViewModel,
                ReregisterCore = new ReregisterCoreViewModel(),
                ReregisterAcademicYear = new ReregisterAcademicYearViewModel()
            };
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);

            RegistrationLoader.GetRegisteredTqAoProviderDetailsAsync(AoUkprn).Returns(_selectProviderViewModel);
            RegistrationLoader.GetRegistrationDetailsAsync(AoUkprn, ProfileId, _registrationPathwayStatus).Returns(mockresult);
        }

        [Fact]
        public void Then_Called_Expected_Methods()
        {
            RegistrationLoader.Received(1).GetRegisteredTqAoProviderDetailsAsync(AoUkprn);
            RegistrationLoader.Received(1).GetRegistrationDetailsAsync(AoUkprn, ProfileId, _registrationPathwayStatus);
        }

        [Fact]
        public void Then_Returns_Expected_BackLink()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ReregisterProviderViewModel));

            var model = viewResult.Model as ReregisterProviderViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.IsChangeMode.Should().BeTrue();
            model.IsFromConfirmation.Should().BeFalse();
            model.SelectedProviderUkprn.Should().Be(_reRegisterProviderViewModel.SelectedProviderUkprn);
            model.ProvidersSelectList.Should().NotBeNull();
            model.ProvidersSelectList.Count.Should().Be(_selectProviderViewModel.ProvidersSelectList.Count);

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.ReregisterCheckAndSubmit);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
