using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddPostalAddressManualPost
{
    public class When_Cache_IsFound : TestSetup
    {
        private AddAddressViewModel _cacheResult;

        public override void Given()
        {
            ViewModel = new AddAddressManualViewModel
            {
                DepartmentName = "Finance",
                OrganisationName = "Org name",
                AddressLine1 = "50",
                AddressLine2 = "Street",
                Town = "Coventry",
                Postcode = "CV1 1XX",
                IsFromSelectAddress = true
            };

            _cacheResult = new AddAddressViewModel  {  AddAddressSelect = new AddAddressSelectViewModel() };
            CacheService.GetAsync<AddAddressViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<AddAddressViewModel>(CacheKey);

            CacheService.Received(1).SetAsync(CacheKey,
                Arg.Is<AddAddressViewModel>(x =>
                x.AddAddressSelect == null &&
                x.AddAddressManual != null &&
                x.AddAddressManual.DepartmentName == ViewModel.DepartmentName &&
                x.AddAddressManual.OrganisationName == ViewModel.OrganisationName &&
                x.AddAddressManual.AddressLine1 == ViewModel.AddressLine1 &&
                x.AddAddressManual.AddressLine2 == ViewModel.AddressLine2 &&
                x.AddAddressManual.Town == ViewModel.Town &&
                x.AddAddressManual.Postcode == ViewModel.Postcode &&
                x.AddAddressManual.IsFromSelectAddress == ViewModel.IsFromSelectAddress));
        }

        [Fact]
        public void Then_Redirected_To_AddAddressCheckAndSubmit()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddAddressCheckAndSubmit);
        }
    }
}
