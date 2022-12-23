using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
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

        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public DateTime DateofBirth { get; set; }

        public string TlevelTitle { get; set; }

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
            Value = DateofBirth.ToDobFormat(trimLeadingZero: true)
        };

        public SummaryItemModel SummaryTlevelTitle => new()
        {
            Id = "tleveltitle",
            Title = CheckAndSubmitContent.Title_TLevel_Text,
            Value = TlevelTitle
        };

        public IList<SummaryItemModel> IpDetailsList { get; set; }

        public virtual BackLinkModel BackLink { get; set; }

        public string DeclarationText { get; set; }
        public void SetDeclarationText(IndustryPlacementViewModel cacheModel)
        {
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus != null &&
                cacheModel?.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.NotSpecified)
            {
                // TODO: Cover UT.
                switch (cacheModel?.IpCompletion?.IndustryPlacementStatus.Value)
                {
                    case IndustryPlacementStatus.Completed:
                    case IndustryPlacementStatus.CompletedWithSpecialConsideration:
                        DeclarationText = string.Format(CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Held_On_Records, LearnerName);
                        break;
                    case IndustryPlacementStatus.NotCompleted:
                        DeclarationText = string.Format(CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Is_Held, LearnerName);
                        break;
                    case IndustryPlacementStatus.WillNotComplete:
                        DeclarationText = "TODO";
                        break;
                }
            }
        }

        public void SetBackLink(IndustryPlacementViewModel cacheModel)
        {
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
                BackLink = new BackLinkModel { RouteName = RouteConstants.IpSpecialConsiderationReasons };
            else
                BackLink = new BackLinkModel { RouteName = RouteConstants.IpCompletion, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } } };
        }
    }
}
