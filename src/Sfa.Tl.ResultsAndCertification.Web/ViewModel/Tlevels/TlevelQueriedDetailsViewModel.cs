using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using SummaryContent = Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel.QueriedDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels
{
    public class TlevelQueriedDetailsViewModel : TlevelSummary
    {
        public TlevelQueriedDetailsViewModel()
        {
            Specialisms = new List<string>();
        }

        public string QueriedBy { get; set; }
        public string QueriedOn { get; set; }
        public bool IsValid { get; set; }

        public SummaryItemModel SummaryQueriedBy => new SummaryItemModel
        {
            Id = "queriedBy",
            Title = SummaryContent.Title_Queried_By,
            Value = QueriedBy
        };

        public SummaryItemModel SummaryQueriedOn => new SummaryItemModel
        {  
            Id = "queriedOn",
            Title = SummaryContent.Title_Date_Queried,
            Value = QueriedOn
        };

        public BackLinkModel BackLink
        {
            get { return new BackLinkModel { RouteName = RouteConstants.QueriedTlevels }; }
        }
    }
}
