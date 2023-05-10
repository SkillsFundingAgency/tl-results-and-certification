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

        public string DeclarationConfirmSupportText { get; set; }

        public string DeclarationUpdateRecordText { get; set; }

        public void SetDeclarationText(IndustryPlacementViewModel cacheModel)
        {
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus != null &&
                cacheModel?.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.NotSpecified)
            {
                switch (cacheModel?.IpCompletion?.IndustryPlacementStatus.Value)
                {
                    case IndustryPlacementStatus.Completed:
                    case IndustryPlacementStatus.CompletedWithSpecialConsideration:
                        DeclarationConfirmSupportText = string.Format(CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Held_On_Records, LearnerName, cacheModel.IpCompletion.CompletionYear);
                        DeclarationUpdateRecordText = string.Format(CheckAndSubmitContent.Declaration_I_Will_Make_Sure_The_Record_Is_Updated, cacheModel.IpCompletion.CompletionYear);
                        break;
                    case IndustryPlacementStatus.NotCompleted:
                    case IndustryPlacementStatus.WillNotComplete:
                        DeclarationConfirmSupportText = CheckAndSubmitContent.Declaration_I_Confirm_Supporting_Docs_Is_Held;
                        break;
                }
            }
        }

        public void SetBackLink(IndustryPlacementViewModel cacheModel)
        {
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
                BackLink = new BackLinkModel { RouteName = RouteConstants.IpSpecialConsiderationReasons };
            else
                BackLink = new BackLinkModel { RouteName = cacheModel?.IpCompletion?.IsChangeJourney == true ? RouteConstants.IpCompletionChange : RouteConstants.IpCompletion, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } } };
        }
    }
}
