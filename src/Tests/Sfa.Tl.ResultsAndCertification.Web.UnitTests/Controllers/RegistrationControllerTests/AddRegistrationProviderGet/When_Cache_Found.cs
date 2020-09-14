using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationProviderGet
{
    public class When_Cache_Found : TestSetup
    {
        private RegistrationViewModel cacheResult;
        private DateofBirthViewModel _dateOfBirthViewModel;
        private SelectProviderViewModel _selectProviderViewModel;

        public override void Given()
        {
            _dateOfBirthViewModel = new DateofBirthViewModel();
            _selectProviderViewModel = new SelectProviderViewModel { ProvidersSelectList = new List<SelectListItem> { new SelectListItem { Text ="Hello", Value = "1" } } };

            cacheResult = new RegistrationViewModel
            {
                DateofBirth = _dateOfBirthViewModel
            };

            RegistrationLoader.GetRegisteredTqAoProviderDetailsAsync(Ukprn).Returns(_selectProviderViewModel);
            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegisteredTqAoProviderDetailsAsync(Ukprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SelectProviderViewModel));

            var model = viewResult.Model as SelectProviderViewModel;
            model.Should().NotBeNull();
            model.ProvidersSelectList.Should().NotBeNull();
            model.ProvidersSelectList.Count.Should().Be(_selectProviderViewModel.ProvidersSelectList.Count);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddRegistrationDateofBirth);
        }
    }
}
