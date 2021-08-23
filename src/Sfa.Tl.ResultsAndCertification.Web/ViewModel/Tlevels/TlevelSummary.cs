using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryList;
using System.Collections.Generic;
using SummaryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel.TlevelSummary;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels
{
    public class TlevelSummary
    {
        public string TlevelTitle { get; set; }
        public string PathwayDisplayName { get; set; }
        public IEnumerable<string> Specialisms { get; set; }

        public SummaryItemModel SummaryTlevelTitle => new SummaryItemModel
        {
            Id = "tleveltitle",
            Title = SummaryContent.Title_TLevel_Text,
            Value = TlevelTitle
        };

        public SummaryItemModel SummaryCore => new SummaryItemModel
        {
            Id = "core",
            Title = SummaryContent.Title_Core_Code_Text,
            Value = PathwayDisplayName,
            IsRawHtml = true
        };

        public SummaryListModel SummarySpecialisms => new SummaryListModel
        {
            Id = "specialisms",
            Title = SummaryContent.Title_Occupational_Specialism_Text,
            Value = Specialisms
        };
    }
}
