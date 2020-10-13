using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterProviderPost
{
    public class When_IsChangeMode_True : TestSetup
    {
        private SelectProviderViewModel _selectProviderViewModel;
        private ReregisterViewModel cacheResult;

        public override void Given()
        {
            ViewModel = new ReregisterProviderViewModel { ProfileId = ProfileId, SelectedProviderUkprn = "1234567890", IsChangeMode = true };
            _selectProviderViewModel = new SelectProviderViewModel { ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Hello", Value = "1" } } };

            RegistrationLoader.GetRegisteredTqAoProviderDetailsAsync(AoUkprn).Returns(_selectProviderViewModel);

            cacheResult = new ReregisterViewModel
            {
                ReregisterProvider = new ReregisterProviderViewModel { SelectedProviderUkprn = "PrevOne" },
                ReregisterCore = new ReregisterCoreViewModel(),
                SpecialismQuestion = new ReregisterSpecialismQuestionViewModel(),
                ReregisterSpecialisms = new ReregisterSpecialismViewModel(),
                ReregisterAcademicYear = new ReregisterAcademicYearViewModel()
            };
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_ReregisterCore()
        {
            var route = Result as RedirectToRouteResult;
            var routeName = route.RouteName;
            routeName.Should().Be(RouteConstants.ReregisterCore);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
            route.RouteValues[Constants.IsChangeMode].Should().Be("true");
        }

        [Fact]
        public void Then_ReregisterViewModel_Reset()
        {
            CacheService.Received(1)
                .SetAsync(CacheKey, Arg.Is<ReregisterViewModel>(x =>
                    x.ReregisterCore == null &&
                    x.SpecialismQuestion == null &&
                    x.ReregisterSpecialisms == null &&
                    x.ReregisterAcademicYear != null &&
                    x.ReregisterProvider == ViewModel
                    ));
        }
    }
}
