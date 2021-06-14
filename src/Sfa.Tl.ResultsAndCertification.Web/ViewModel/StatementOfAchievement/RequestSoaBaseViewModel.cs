using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement
{
    public abstract class RequestSoaBaseViewModel
    {
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public string TLevelTitle { get; set; }

        protected string UlnLabel { get; set; }
        protected string LearnerNameLabel { get; set; }
        protected string DateofBirthLabel { get; set; }
        protected string ProviderNameLabel { get; set; }
        protected string TlevelTitleLabel { get; set; }

        public SummaryItemModel SummaryUln => new SummaryItemModel
        {
            Id = "uln",
            Title = UlnLabel,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryLearnerName => new SummaryItemModel
        {
            Id = "learnername",
            Title = LearnerNameLabel,
            Value = LearnerName
        };

        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel
        {
            Id = "dateofbirth",
            Title = DateofBirthLabel,
            Value = DateofBirth.ToDobFormat()
        };

        public SummaryItemModel SummaryProvider => new SummaryItemModel
        {
            Id = "providername",
            Title = ProviderNameLabel,
            Value = ProviderName
        };

        public SummaryItemModel SummaryTlevelTitle => new SummaryItemModel
        {
            Id = "tleveltitle",
            Title = TlevelTitleLabel,
            Value = TLevelTitle
        };        

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.RequestSoaUniqueLearnerNumber
        };
    }
}
