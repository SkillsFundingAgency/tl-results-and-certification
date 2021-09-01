using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryList;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VerifyContent = Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel.Verify;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels
{
    public class ConfirmTlevelViewModel
    {
        public ConfirmTlevelViewModel()
        {
            Specialisms = new List<string>();
        }

        public int TqAwardingOrganisationId { get; set; }
        public int RouteId { get; set; }
        public int PathwayId { get; set; }
        public int PathwayStatusId { get; set; }
        public string TlevelTitle { get; set; }
        public string PathwayDisplayName { get; set; }

        [Required(ErrorMessageResourceType = typeof(VerifyContent), ErrorMessageResourceName = "IsEverythingCorrect_Required_Validation_Message")]
        public bool? IsEverythingCorrect { get; set; }
        public IEnumerable<string> Specialisms { get; set; }
        public bool HasMoreToReview { get; set; }

        public SummaryItemModel SummaryTlevelTitle => new SummaryItemModel
        {
            Id = "tleveltitle",
            Title = VerifyContent.Title_TLevel_Text,
            Value = TlevelTitle
        };

        public SummaryItemModel SummaryCore => new SummaryItemModel
        {
            Id = "core",
            Title = VerifyContent.Title_Core_Code_Text,
            Value = PathwayDisplayName,
            IsRawHtml = true
        };

        public SummaryListModel SummarySpecialisms => new SummaryListModel
        {
            Id = "specialisms",
            Title = VerifyContent.Title_Occupational_Specialism_Text,
            Value = Specialisms
        };

        public BackLinkModel BackLink
        {
            get
            {
                if (HasMoreToReview)
                    return new BackLinkModel
                    {
                        RouteName = RouteConstants.SelectTlevel,
                        RouteAttributes = new Dictionary<string, string> { { "id", PathwayId.ToString() } }
                    };
                else
                    return new BackLinkModel
                    {
                        RouteName = RouteConstants.TlevelsDashboard
                    };
            }
        }
    }
}
