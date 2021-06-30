using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Linq;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.ManagePostalAddress
{
    public class When_Address_IsNotFound : TestSetup
    {
        private ManagePostalAddressViewModel _mockResult;
        public override void Given()
        {
            _mockResult = null;
            ProviderAddressLoader.GetAddressAsync<ManagePostalAddressViewModel>(ProviderUkprn).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ProviderAddressLoader.Received(1).GetAddressAsync<ManagePostalAddressViewModel>(ProviderUkprn);
            CacheService.Received(1).RemoveAsync<AddAddressViewModel>(string.Concat(CacheKey));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as ManagePostalAddressViewModel;

            model.Should().NotBeNull();
            model.DepartmentName.Should().BeNull();
            model.OrganisationName.Should().BeNull();
            model.AddressLine1.Should().BeNull();
            model.AddressLine2.Should().BeNull();
            model.Town.Should().BeNull();
            model.Postcode.Should().BeNull();
            model.HasAddress.Should().BeFalse();

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count().Should().Be(2);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Manage_Postal_Address);
        }        
    }
}
