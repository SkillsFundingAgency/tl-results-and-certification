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

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class RegistrationDetailsViewModel
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderDisplayName { get; set; }
        public string PathwayDisplayName { get; set; }
        public IEnumerable<string> SpecialismsDisplayName { get; set; }
        public int AcademicYear { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
        
        private string ActionText { get { return Status == RegistrationPathwayStatus.Active ? RegistrationDetailsContent.Change_Action_Link_Text : null; } }
        public Dictionary<string, string> ChangeLinkRouteAttributes => new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } };

        public SummaryItemModel SummaryLearnerName => new SummaryItemModel { Id = "learnername", Title = RegistrationDetailsContent.Title_Name_Text, Value = Name, ActionText = ActionText, RouteName = RouteConstants.ChangeRegistrationLearnersName, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel { Id = "dateofbirth", Title = RegistrationDetailsContent.Title_DateofBirth_Text, Value = DateofBirth.ToShortDateString(), ActionText = ActionText, RouteName = RouteConstants.ChangeRegistrationDateofBirth, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryItemModel SummaryProvider => new SummaryItemModel { Id = "provider", Title = RegistrationDetailsContent.Title_Provider_Text, Value = ProviderDisplayName, ActionText = ActionText, RouteName = RouteConstants.ChangeRegistrationProvider, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryItemModel SummaryCore => new SummaryItemModel { Id = "core", Title = RegistrationDetailsContent.Title_Core_Text, Value = PathwayDisplayName, ActionText = ActionText, RouteName = RouteConstants.ChangeRegistrationCore, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryListModel SummarySpecialisms => new SummaryListModel { Id = "specialisms", Title = RegistrationDetailsContent.Title_Specialism_Text, Value = SpecialismsDisplayName, ActionText = ActionText, RouteName = GetSpecialismRouteName, RouteAttributes = ChangeLinkRouteAttributes, HiddenText = GetSpecialismHiddenText };
        public SummaryItemModel SummaryAcademicYear => new SummaryItemModel { Id = "academicyear", Title = RegistrationDetailsContent.Title_AcademicYear_Text, Value = EnumExtensions.GetDisplayName<AcademicYear>(AcademicYear), ActionText = ActionText, RouteName = RouteConstants.ChangeAcademicYear, RouteAttributes = ChangeLinkRouteAttributes };

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
