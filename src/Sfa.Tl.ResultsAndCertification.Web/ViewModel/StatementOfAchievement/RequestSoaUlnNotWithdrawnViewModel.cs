using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using RequestSoaUlnNotWithdrawnContent = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaUlnNotWithdrawn;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement
{
    public class RequestSoaUlnNotWithdrawnViewModel
    {
        public long Uln { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public string TLevelTitle { get; set; }

        public SummaryItemModel SummaryUln => new SummaryItemModel
        {
            Id = "uln",
            Title = RequestSoaUlnNotWithdrawnContent.Title_Uln_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryLearnerName => new SummaryItemModel
        {
            Id = "learnername",
            Title = RequestSoaUlnNotWithdrawnContent.Title_Name_Text,
            Value = Name
        };

        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel
        {
            Id = "dateofbirth",
            Title = RequestSoaUlnNotWithdrawnContent.Title_DateofBirth_Text,
            Value = DateofBirth.ToDobFormat()
        };

        public SummaryItemModel SummaryProvider => new SummaryItemModel
        {
            Id = "provider",
            Title = RequestSoaUlnNotWithdrawnContent.Title_Provider_Text,
            Value = ProviderName
        };

        public SummaryItemModel SummaryCore => new SummaryItemModel
        {
            Id = "tleveltitle",
            Title = RequestSoaUlnNotWithdrawnContent.Title_TLevel_Text,
            Value = TLevelTitle
        };

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.RequestSoaUniqueLearnerNumber
        };
    }
}