using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ManagePostalAddressContent = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress.ManagePostalAddress;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class ManagePostalAddressViewModel
    {
        public string DepartmentName { get; set; }
        public string OrganisationName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }

        public bool HasAddress => !string.IsNullOrWhiteSpace(AddressLine1) && !string.IsNullOrWhiteSpace(Postcode);

        public SummaryItemModel SummaryDepartment => new SummaryItemModel
        {
            Id = "department",
            Title = ManagePostalAddressContent.Department_Label,
            Value = DepartmentName,
            NeedBorderBottomLine = false,
            RenderActionColumn = false
        };

        public SummaryItemModel SummaryOrganisationName => new SummaryItemModel
        {
            Id = "organisationname",
            Title = ManagePostalAddressContent.OrganisationName_Label,
            Value = OrganisationName,
            NeedBorderBottomLine = false,
            RenderActionColumn = false
        };

        public SummaryItemModel SummaryAddressLine1 => new SummaryItemModel
        {
            Id = "addressline1",
            Title = ManagePostalAddressContent.BuildingAndStreet_Label,
            Value = AddressLine1,
            NeedBorderBottomLine = false,
            RenderActionColumn = false
        };

        public SummaryItemModel SummaryAddressLine2 => new SummaryItemModel
        {
            Id = "addressline2",
            Title = string.Empty,
            Value = AddressLine2,
            NeedBorderBottomLine = false,
            RenderActionColumn = false
        };

        public SummaryItemModel SummaryTown => new SummaryItemModel
        {
            Id = "town",
            Title = ManagePostalAddressContent.Town_Label,
            Value = Town,
            NeedBorderBottomLine = false,
            RenderActionColumn = false
        };

        public SummaryItemModel SummaryPostcode => new SummaryItemModel
        {
            Id = "postcode",
            Title = ManagePostalAddressContent.Postcode_Label,
            Value = Postcode,
            NeedBorderBottomLine = false,
            RenderActionColumn = false
        };

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Manage_Postal_Address }
                    }
                };
            }
        }
    }
}
