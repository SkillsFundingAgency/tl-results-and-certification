using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement.IpCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IpCheckAndSubmitViewModel
    {
        public IpCheckAndSubmitViewModel()
        {
            IpDetailsList = new List<SummaryItemModel>();
        }

        //public int PathwayId { get; set; }
        //public int AcademicYear { get; set; }

        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public DateTime DateofBirth { get; set; }
        
        public string TlevelTitle { get; set; } // TODO: Tlevel or TlevelTitle?

        public SummaryItemModel SummaryUln => new()
        {
            Id = "uln",
            Title = CheckAndSubmitContent.Title_Uln_Text,
            Value = Uln.ToString()
        };

         public SummaryItemModel SummaryLearnerName => new()
         {
             Id = "learnername",
             Title = CheckAndSubmitContent.Title_Name_Text,
             Value = LearnerName
         };

        public SummaryItemModel SummaryDateofBirth => new()
        {
            Id = "dateofbirth",
            Title = CheckAndSubmitContent.Title_DateofBirth_Text,
            Value = DateofBirth.ToDobFormat()
        };

        public SummaryItemModel SummaryTlevelTitle => new()
        {
            Id = "tleveltitle",
            Title = CheckAndSubmitContent.Title_TLevel_Text,
            Value = TlevelTitle
        };

        public IList<SummaryItemModel> IpDetailsList { get; set; }

        public virtual BackLinkModel BackLink { get; set; }

        // **** BackLinks **** //
        // Ipmodel (NO)
        // MultiEmpSelect (Radios)
        // MultiEmpOther (Radios)
        // Tempflex (NO)
        // Blended
        // Q3-led
        // Q4-non-led
    }
}
