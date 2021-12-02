using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryList;
using System;
using System.Collections.Generic;
using RegistrationDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.RegistrationDetails;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using System.Linq;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class RegistrationDetailsViewModel
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public string PathwayLarId { get; set; }
        public string ProviderDisplayName { get; set; }
        public string PathwayDisplayName { get; set; }
        public IEnumerable<string> SpecialismsDisplayName { get; set; }
        public int AcademicYear { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
        public IEnumerable<AcademicYear> AcademicYears { get; set; }
        public bool IsActiveWithOtherAo { get; set; }

        public bool ShowAssessmentEntriesLink { get { return Status == RegistrationPathwayStatus.Active;  } }
        
        private string ChangeStatusRouteName => Status == RegistrationPathwayStatus.Active ? RouteConstants.AmendActiveRegistration : RouteConstants.AmendWithdrawRegistration;
        private string TagCssClassName => Status == RegistrationPathwayStatus.Active ? "govuk-tag--green" : "govuk-tag--blue";
        private string ActionText { get { return Status == RegistrationPathwayStatus.Active ? RegistrationDetailsContent.Change_Action_Link_Text : null; } }
        public Dictionary<string, string> ChangeLinkRouteAttributes => new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } };

        public SummaryItemModel SummaryStatus { get { return (IsActiveWithOtherAo) ? new SummaryItemModel { Id = "learnerstatus", Title = RegistrationDetailsContent.Title_Status, Value = Status.ToString(), HasTag = true, TagCssClass = TagCssClassName } : new SummaryItemModel { Id = "learnerstatus", Title = RegistrationDetailsContent.Title_Status, Value = Status.ToString(), HasTag = true, TagCssClass = TagCssClassName, RenderHiddenActionText = false, ActionText = RegistrationDetailsContent.Change_Status_Action_Link_Text, RouteName = ChangeStatusRouteName, RouteAttributes = ChangeLinkRouteAttributes }; } }
        public SummaryItemModel SummaryLearnerName => new SummaryItemModel { Id = "learnername", Title = RegistrationDetailsContent.Title_Name_Text, Value = Name, ActionText = ActionText, RouteName = RouteConstants.ChangeRegistrationLearnersName, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel { Id = "dateofbirth", Title = RegistrationDetailsContent.Title_DateofBirth_Text, Value = DateofBirth.ToShortDateString(), ActionText = ActionText, RouteName = RouteConstants.ChangeRegistrationDateofBirth, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryItemModel SummaryProvider => new SummaryItemModel { Id = "provider", Title = RegistrationDetailsContent.Title_Provider_Text, Value = ProviderDisplayName, ActionText = ActionText, RouteName = RouteConstants.ChangeRegistrationProvider, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryItemModel SummaryCore => new SummaryItemModel { Id = "core", Title = RegistrationDetailsContent.Title_Core_Text, Value = PathwayDisplayName, ActionText = ActionText, RouteName = RouteConstants.ChangeRegistrationCore, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryListModel SummarySpecialisms => new SummaryListModel { Id = "specialisms", Title = RegistrationDetailsContent.Title_Specialism_Text, Value = SpecialismsDisplayName, ActionText = ActionText, RouteName = GetSpecialismRouteName, RouteAttributes = ChangeLinkRouteAttributes, HiddenText = GetSpecialismHiddenText };
        public SummaryItemModel SummaryAcademicYear => new SummaryItemModel { Id = "academicyear", Title = RegistrationDetailsContent.Title_AcademicYear_Text, Value = GetAcademicYearName, ActionText = ActionText, RouteName = RouteConstants.ChangeAcademicYear, RouteAttributes = ChangeLinkRouteAttributes };

        public string GetAcademicYearName => AcademicYears?.FirstOrDefault(a => a.Year == AcademicYear)?.Name;

        public string GetSpecialismHiddenText => (SpecialismsDisplayName == null || !SpecialismsDisplayName.Any()) ? RegistrationDetailsContent.Specialism_None_Selected_Text : null;

        public string GetSpecialismRouteName => SpecialismsDisplayName != null && SpecialismsDisplayName.Any() ? RouteConstants.ChangeRegistrationSpecialismQuestion : RouteConstants.ChangeRegistrationSpecialisms;

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Registration_Dashboard, RouteName = RouteConstants.RegistrationDashboard },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Registration, RouteName = RouteConstants.SearchRegistration },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Registration_Details }
                    }
                };
            }
        }
    }
}
