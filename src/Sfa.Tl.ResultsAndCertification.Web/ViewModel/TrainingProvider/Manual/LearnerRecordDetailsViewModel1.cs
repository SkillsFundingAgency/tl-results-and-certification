using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class LearnerRecordDetailsViewModel1
    {
        // Header
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public long ProviderUkprn { get; set; }
        public string CoreName { get; set; }
        public int StartYear { get; set; }
        public string AwardingOrganisationName { get; set; }
        public SubjectStatus MathsStatus { get; set; }
        public SubjectStatus EnglishStatus { get; set; }

        // Maths and English
        public SummaryItemModel SummaryMathsStatus => new SummaryItemModel
        {
            Id = "mathsstatus",
            Title = "Maths",
            Value = MathsStatus.ToString(), // TODO: get content
            ActionText = "Add",
            RouteName = "#",
            //RouteAttributes = TODO,
            RenderHiddenActionText = true,
            HiddenActionText = " maths status"
        };

        public SummaryItemModel SummaryEnglishStatus => new SummaryItemModel
        {
            Id = "englishstatus",
            Title = "English",
            Value = EnglishStatus.ToString(), // TODO: get content
            ActionText = "Add",
            RouteName = "#",
            //RouteAttributes = TODO,
            RenderHiddenActionText = true,
            HiddenActionText = " english status"
        };

        // Industry Placement
        // Todo: Next Story. 

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.Home
                };
            }
        }
    }
}
