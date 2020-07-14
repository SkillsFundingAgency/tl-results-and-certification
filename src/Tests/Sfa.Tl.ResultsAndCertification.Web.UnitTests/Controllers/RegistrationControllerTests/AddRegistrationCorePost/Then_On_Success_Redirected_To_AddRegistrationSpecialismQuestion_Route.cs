using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationCorePost
{
    public class Then_On_Success_Redirected_To_AddRegistrationSpecialismQuestion_Route : When_AddRegistrationCoreAsync_Action_Is_Called
    {
        private SelectProviderViewModel _selectProviderViewModel;
        private long _providerUkprn = 987654321;       

        public override void Given()
        {
            SelectCoreViewModel = new SelectCoreViewModel { SelectedCoreId = "10000057" };

            _selectProviderViewModel = new SelectProviderViewModel { SelectedProviderId = _providerUkprn.ToString(), ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Test Provider", Value = _providerUkprn.ToString() } } };

            var cacheResult = new RegistrationViewModel
            {
                SelectProvider = _selectProviderViewModel
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_On_Success_Redirected_To_AddRegistration_SpecialismQuestion_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationSpecialismQuestion);
        }
    }
}
