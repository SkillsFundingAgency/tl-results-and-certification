using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.ProviderAddress.CheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress
{
    public class AddAddressCheckAndSubmitViewModel
    {
        public AddProviderAddressViewModel ProviderAddress { get; set; }
        public bool IsValid { get { return true; } }  /*TODO*/
        public bool IsManual { get { return true; } }

        private (string, Dictionary<string, string>) BackRoute
        {
            get
            {
                return ProviderAddress.IsManual ?
                    (RouteConstants.AddPostalAddressManual, new Dictionary<string, string> { { Constants.IsFromSelectAddress, ProviderAddress.Manual.IsFromSelectAddress.ToString() } })
                    : (RouteConstants.AddAddressSelect, null);
            }
        }

        public SummaryItemModel SummaryDepartment => new SummaryItemModel
        {
            Id = "department",
            Title = CheckAndSubmitContent.Summary_Department,
            Value = IsManual ? ProviderAddress.Manual.Department : "TODO-Dept",
            NeedBorderBottomLine = false,

            ActionText = CheckAndSubmitContent.Link_Change_Address,
            RouteName = BackRoute.Item1,
            RouteAttributes = BackRoute.Item2
        };

        public SummaryItemModel SummaryAddressLine1 => new SummaryItemModel
        {
            Id = "addressline1",
            Title = CheckAndSubmitContent.Summary_Building_And_Street,
            Value = IsManual ? ProviderAddress.Manual.AddressLine1 : "TODO-Address1",
            NeedBorderBottomLine = false
        };

        public SummaryItemModel SummaryAddressLine2 => new SummaryItemModel
        {
            Id = "addressline2",
            Title = string.Empty,
            Value = IsManual ? ProviderAddress.Manual.AddressLine2 : "TODO-Address2",
            NeedBorderBottomLine = false
        };

        public SummaryItemModel SummaryTown => new SummaryItemModel
        {
            Id = "town",
            Title = CheckAndSubmitContent.Summary_Town_Or_City,
            Value = IsManual ? ProviderAddress.Manual.Town : "TODO-town",
            NeedBorderBottomLine = false
        };

        public SummaryItemModel SummaryPostcode => new SummaryItemModel
        {
            Id = "postcode",
            Title = CheckAndSubmitContent.Summary_Postcode,
            Value = IsManual ? ProviderAddress.Manual.Postcode : "TODO-postcode",
            NeedBorderBottomLine = false
        };

        public BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = BackRoute.Item1,
            RouteAttributes = BackRoute.Item2
        };
    }
}
