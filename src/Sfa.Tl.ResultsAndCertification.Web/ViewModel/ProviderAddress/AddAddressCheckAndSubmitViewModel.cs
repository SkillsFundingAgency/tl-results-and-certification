﻿using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress.CheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddAddressCheckAndSubmitViewModel
    {
        private bool IsManual => ProviderAddress.AddAddressManual != null;
        private string PreviousRoute => IsManual ? RouteConstants.AddPostalAddressManual : RouteConstants.AddAddressSelect;

        public AddAddressViewModel ProviderAddress { get; set; }
        public bool IsValid
        {
            get
            {
                return ProviderAddress != null &&
                    (IsManual ? ProviderAddress.AddAddressSelect == null : ProviderAddress.AddAddressPostcode != null && ProviderAddress.AddAddressSelect != null);
            }
        }

        public SummaryItemModel SummaryDepartment => new SummaryItemModel
        {
            Id = "department",
            Title = CheckAndSubmitContent.Summary_Department,
            Value = IsManual ? ProviderAddress.AddAddressManual.DepartmentName : ProviderAddress.AddAddressSelect.DepartmentName,
            NeedBorderBottomLine = false,

            ActionText = CheckAndSubmitContent.Link_Change_Address,
            RouteName = PreviousRoute,
        };

        public SummaryItemModel SummaryOrganisationName => new SummaryItemModel
        {
            Id = "organisationname",
            Title = CheckAndSubmitContent.Summary_OrganisationName,
            Value = IsManual ? ProviderAddress.AddAddressManual.OrganisationName : ProviderAddress.AddAddressSelect.SelectedAddress.OrganisationName,
            NeedBorderBottomLine = false
        };

        public SummaryItemModel SummaryAddressLine1 => new SummaryItemModel
        {
            Id = "addressline1",
            Title = CheckAndSubmitContent.Summary_Building_And_Street,
            Value = IsManual ? ProviderAddress.AddAddressManual.AddressLine1 : ProviderAddress.AddAddressSelect.SelectedAddress.AddressLine1,
            NeedBorderBottomLine = false
        };

        public SummaryItemModel SummaryAddressLine2 => new SummaryItemModel
        {
            Id = "addressline2",
            Title = string.Empty,
            Value = IsManual ? ProviderAddress.AddAddressManual.AddressLine2 : ProviderAddress.AddAddressSelect.SelectedAddress.AddressLine2,
            NeedBorderBottomLine = false
        };

        public SummaryItemModel SummaryTown => new SummaryItemModel
        {
            Id = "town",
            Title = CheckAndSubmitContent.Summary_Town_Or_City,
            Value = IsManual ? ProviderAddress.AddAddressManual.Town : ProviderAddress.AddAddressSelect.SelectedAddress.Town,
            NeedBorderBottomLine = false
        };

        public SummaryItemModel SummaryPostcode => new SummaryItemModel
        {
            Id = "postcode",
            Title = CheckAndSubmitContent.Summary_Postcode,
            Value = IsManual ? ProviderAddress.AddAddressManual.Postcode : ProviderAddress.AddAddressSelect.SelectedAddress.Postcode,
            NeedBorderBottomLine = false
        };

        public BackLinkModel BackLink => new BackLinkModel { RouteName = PreviousRoute };
    }
}