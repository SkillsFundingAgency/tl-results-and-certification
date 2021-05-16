using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Linq;
using Xunit;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ManagePostalAddressContent = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress.ManagePostalAddress;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.ManagePostalAddress
{
    public class When_Address_Found : TestSetup
    {
        private ManagePostalAddressViewModel _mockResult;
        public override void Given()
        {
            _mockResult = new ManagePostalAddressViewModel { DepartmentName = "Dept", OrganisationName = "Org", AddressLine1 = "Line1", AddressLine2 = "Line2", Town = "Town", Postcode = "x11 1yy" };
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
            model.DepartmentName.Should().Be(_mockResult.DepartmentName);
            model.OrganisationName.Should().Be(_mockResult.OrganisationName);
            model.AddressLine1.Should().Be(_mockResult.AddressLine1);
            model.AddressLine2.Should().Be(_mockResult.AddressLine2);
            model.Town.Should().Be(_mockResult.Town);
            model.Postcode.Should().Be(_mockResult.Postcode);
            model.HasAddress.Should().BeTrue();

            // Department
            model.SummaryDepartment.Title.Should().Be(ManagePostalAddressContent.Department_Label);
            model.SummaryDepartment.Value.Should().Be(_mockResult.DepartmentName);
            model.SummaryDepartment.NeedBorderBottomLine.Should().Be(false);
            model.SummaryDepartment.RenderActionColumn.Should().Be(false);

            // Organisation Name
            model.SummaryOrganisationName.Title.Should().Be(ManagePostalAddressContent.OrganisationName_Label);
            model.SummaryOrganisationName.Value.Should().Be(_mockResult.OrganisationName);
            model.SummaryOrganisationName.NeedBorderBottomLine.Should().Be(false);
            model.SummaryDepartment.RenderActionColumn.Should().Be(false);

            // AddressLine1
            model.SummaryAddressLine1.Title.Should().Be(ManagePostalAddressContent.BuildingAndStreet_Label);
            model.SummaryAddressLine1.Value.Should().Be(_mockResult.AddressLine1);
            model.SummaryAddressLine1.NeedBorderBottomLine.Should().Be(false);
            model.SummaryDepartment.RenderActionColumn.Should().Be(false);

            // AddressLine2
            model.SummaryAddressLine2.Title.Should().BeEmpty();
            model.SummaryAddressLine2.Value.Should().Be(_mockResult.AddressLine2);
            model.SummaryAddressLine2.NeedBorderBottomLine.Should().Be(false);
            model.SummaryDepartment.RenderActionColumn.Should().Be(false);

            // Town
            model.SummaryTown.Title.Should().Be(ManagePostalAddressContent.Town_Label);
            model.SummaryTown.Value.Should().Be(_mockResult.Town);
            model.SummaryTown.NeedBorderBottomLine.Should().Be(false);
            model.SummaryDepartment.RenderActionColumn.Should().Be(false);

            // Postcode
            model.SummaryPostcode.Title.Should().Be(ManagePostalAddressContent.Postcode_Label);
            model.SummaryPostcode.Value.Should().Be(_mockResult.Postcode);
            model.SummaryPostcode.NeedBorderBottomLine.Should().Be(false);
            model.SummaryDepartment.RenderActionColumn.Should().Be(false);

            model.Breadcrumb.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Should().NotBeNull();
            model.Breadcrumb.BreadcrumbItems.Count().Should().Be(2);
            model.Breadcrumb.BreadcrumbItems[0].RouteName.Should().Be(RouteConstants.Home);
            model.Breadcrumb.BreadcrumbItems[0].DisplayName.Should().Be(BreadcrumbContent.Home);
            model.Breadcrumb.BreadcrumbItems[1].DisplayName.Should().Be(BreadcrumbContent.Manage_Postal_Address);
        }
    }
}
