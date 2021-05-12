using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress.CheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddAddressCheckAndSubmitViewModel
    {
        private bool IsManual { get { return ProviderAddress.AddAddressManual != null; } }
        private (string, Dictionary<string, string>) PreviousRoute
        {
            get
            {
                return IsManual ?
                    (RouteConstants.AddPostalAddressManual, new Dictionary<string, string> { { Constants.IsFromSelectAddress, ProviderAddress.AddAddressManual.IsFromSelectAddress.ToString() } })
                    : (RouteConstants.AddAddressSelect, null);
            }
        }

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
            Value = IsManual ? ProviderAddress.AddAddressManual.Department : ProviderAddress.AddAddressSelect.DepartmentName,
            NeedBorderBottomLine = false,

            ActionText = CheckAndSubmitContent.Link_Change_Address,
            RouteName = PreviousRoute.Item1,
            RouteAttributes = PreviousRoute.Item2
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

        public BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = PreviousRoute.Item1,
            RouteAttributes = PreviousRoute.Item2
        };
    }
}