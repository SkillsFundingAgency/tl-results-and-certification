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
    public class When_Success : TestSetup
    {
        private SelectProviderViewModel _selectProviderViewModel;
        private long _providerUkprn = 987654321;       

        public override void Given()
        {
            SelectCoreViewModel = new SelectCoreViewModel { SelectedCoreCode = "10000057" };

            _selectProviderViewModel = new SelectProviderViewModel { SelectedProviderUkprn = _providerUkprn.ToString(), ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Test Provider", Value = _providerUkprn.ToString() } } };

            var cacheResult = new RegistrationViewModel
            {
                SelectProvider = _selectProviderViewModel
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_AddRegistrationSpecialismQuestion()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationSpecialismQuestion);
        }
    }
}
