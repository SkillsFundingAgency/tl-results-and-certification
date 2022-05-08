using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
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

        public void SetBackLink(IndustryPlacementViewModel cacheModel, IpTempFlexNavigation navigation)
        {
            if (navigation == null)
            {
                // Then Back link is one of the IpModel page. 
                if (cacheModel?.IpModelViewModel?.IpModelUsed?.IsIpModelUsed == false)
                {
                    // Pattern future.
                    if (cacheModel?.IpModelViewModel?.IpMultiEmployerUsed.IsMultiEmployerModelUsed == true)
                        BackLink = new BackLinkModel { RouteName = RouteConstants.IpMultiEmployerOther };
                    else
                        BackLink = new BackLinkModel { RouteName = RouteConstants.IpMultiEmployerSelect };
                }
                else
                    BackLink = new BackLinkModel { RouteName = RouteConstants.IpModelUsed };
            }
            else
            {
                // Then Back link is to one of the TempFlex page
                if (navigation.AskTempFlexibility && cacheModel?.TempFlexibility?.IpTempFlexibilityUsed?.IsTempFlexibilityUsed == false)
                    BackLink = new BackLinkModel { RouteName = RouteConstants.IpTempFlexibilityUsed };
                else
                {
                    if (navigation.AskTempFlexibility && !navigation.AskBlendedPlacement) // Pattern 2
                        BackLink = new BackLinkModel { RouteName = RouteConstants.IpGrantedTempFlexibility };
                    else
                    {
                        if (navigation.AskBlendedPlacement && cacheModel?.TempFlexibility?.IpBlendedPlacementUsed != null &&
                            cacheModel?.TempFlexibility?.IpEmployerLedUsed == null && cacheModel?.TempFlexibility?.IpGrantedTempFlexibility == null)
                            BackLink = new BackLinkModel { RouteName = RouteConstants.IpBlendedPlacementUsed }; // Pattern 3
                        else
                        {
                            if (navigation.AskBlendedPlacement && cacheModel?.TempFlexibility?.IpBlendedPlacementUsed != null &&
                                cacheModel?.TempFlexibility?.IpEmployerLedUsed != null && cacheModel?.TempFlexibility?.IpGrantedTempFlexibility == null)
                                BackLink = new BackLinkModel { RouteName = RouteConstants.IpEmployerLedUsed }; // Pattern 1
                            else
                            {
                                if (navigation.AskBlendedPlacement && cacheModel?.TempFlexibility?.IpBlendedPlacementUsed != null &&
                                    cacheModel?.TempFlexibility?.IpEmployerLedUsed == null && cacheModel?.TempFlexibility?.IpGrantedTempFlexibility != null)
                                    BackLink = new BackLinkModel { RouteName = RouteConstants.IpGrantedTempFlexibility }; // Pattern 1
                                else
                                    BackLink = new BackLinkModel { RouteName = RouteConstants.PageNotFound };
                            }
                        }
                    }
                }
            }
        }
    }
}
