using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationCoreGet
{
    public class Then_On_No_Cache_For_Core_Empty_ViewModel_Returned : When_AddRegistrationCoreAsync_Action_Is_Called
    {
        private RegistrationViewModel cacheResult;
        private DateofBirthViewModel _dateOfBirthViewModel;
        private SelectProviderViewModel _selectProviderViewModel;
        private SelectCoreViewModel _selectCoreViewModel;
        private long _providerUkprn = 987654321;

        public override void Given()
        {
            _dateOfBirthViewModel = new DateofBirthViewModel { Day = DateTime.Now.Day.ToString(), Month = DateTime.Now.Month.ToString(), Year = DateTime.Now.Year.ToString() };
            _selectProviderViewModel = new SelectProviderViewModel { SelectedProviderId = _providerUkprn.ToString(), ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text = "Hello", Value = "1" } } };
            _selectCoreViewModel = new SelectCoreViewModel { CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = "123" } } };
            cacheResult = new RegistrationViewModel
            {
                SelectProvider = _selectProviderViewModel
            };

            RegistrationLoader.GetRegisteredProviderPathwayDetailsAsync(Ukprn, _providerUkprn).Returns(_selectCoreViewModel);
            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_GetRegisteredProviderCoreDetailsAsync_Method_Is_Called()
        {
            RegistrationLoader.Received(1).GetRegisteredProviderPathwayDetailsAsync(Ukprn, _providerUkprn);
        }

        [Fact]
        public void Then_Expected_Select_Core_ViewModel_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SelectCoreViewModel));

            var model = viewResult.Model as SelectCoreViewModel;
            model.Should().NotBeNull();

            model.SelectedCoreCode.Should().BeNullOrEmpty();
            model.CoreSelectList.Should().NotBeNull();
            model.CoreSelectList.Count.Should().Be(_selectCoreViewModel.CoreSelectList.Count);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddRegistrationProvider);
        }
    }
}
