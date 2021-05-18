﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressCheckAndSubmitGet
{
    public class When_Both_Manual_SelectedAddress_AreExist : TestSetup
    {
        private AddAddressViewModel _cacheResult;

        public override void Given()
        {
            _cacheResult = new AddAddressViewModel
            {
                AddAddressPostcode = new  AddAddressPostcodeViewModel(),
                AddAddressSelect = new AddAddressSelectViewModel(),
                AddAddressManual = new AddAddressManualViewModel()
            };

            CacheService.GetAsync<AddAddressViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<AddAddressViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
