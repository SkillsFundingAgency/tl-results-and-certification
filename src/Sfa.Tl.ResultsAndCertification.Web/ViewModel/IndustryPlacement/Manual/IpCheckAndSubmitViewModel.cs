using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;

using Content = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement.IpCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IpCheckAndSubmitViewModel
    {
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public DateTime DateofBirth { get; set; }

        public string TlevelTitle { get; set; } // TODO: Tlevel or TlevelTitle?

        public SummaryItemModel SummaryUln => new()
        {
            Id = "uln",
            Title = Content.IndustryPlacement.IpCheckAndSubmit.Title_Uln_Text,
            Value = Uln.ToString()
        };

         public SummaryItemModel SummaryLearnerName => new()
         {
             Id = "learnername",
             Title = Content.IndustryPlacement.IpCheckAndSubmit.Title_Name_Text,
             Value = LearnerName
         };

        public SummaryItemModel SummaryDateofBirth => new()
        {
            Id = "dateofbirth",
            Title = Content.IndustryPlacement.IpCheckAndSubmit.Title_DateofBirth_Text,
            Value = DateofBirth.ToDobFormat()
        };

        public SummaryItemModel SummaryTlevelTitle => new()
        {
            Id = "tleveltitle",
            Title = Content.IndustryPlacement.IpCheckAndSubmit.Title_TLevel_Text,
            Value = TlevelTitle
        };


        public virtual BackLinkModel BackLink { get; set; }

        // **** BackLinks **** //
        // Ipmodel (NO)
        // MultiEmpSelect (Radios)
        // MultiEmpOther (Radios)
        // Tempflex (NO)
        // Blended
        // Q3-led
        // Q4
    }
}
