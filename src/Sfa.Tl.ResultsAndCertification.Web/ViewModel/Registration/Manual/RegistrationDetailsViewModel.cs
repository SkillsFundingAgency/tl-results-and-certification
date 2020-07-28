using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryList;
using System;
using System.Collections.Generic;
using RegistrationDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.RegistrationDetails;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using System.Linq;

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

        public SummaryItemModel SummaryLearnerName => new SummaryItemModel { Id = "learnername", Title = RegistrationDetailsContent.Title_Name_Text, Value = Name, RouteName = RouteConstants.AddRegistrationLearnersName };
        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel { Id = "dateofbirth", Title = RegistrationDetailsContent.Title_DateofBirth_Text, Value = DateofBirth.ToShortDateString(), RouteName = RouteConstants.AddRegistrationDateofBirth };
        public SummaryItemModel SummaryProvider => new SummaryItemModel { Id = "provider", Title = RegistrationDetailsContent.Title_Provider_Text, Value = ProviderDisplayName, RouteName = RouteConstants.AddRegistrationProvider };
        public SummaryItemModel SummaryCore => new SummaryItemModel { Id = "core", Title = RegistrationDetailsContent.Title_Core_Text, Value = PathwayDisplayName, RouteName = RouteConstants.AddRegistrationCore };
        public SummaryListModel SummarySpecialisms => new SummaryListModel { Id = "specialisms", Title = RegistrationDetailsContent.Title_Specialism_Text, Value = SpecialismsDisplayName, RouteName = RouteConstants.AddRegistrationSpecialism };
        public SummaryItemModel SummaryAcademicYear => new SummaryItemModel { Id = "academicyear", Title = RegistrationDetailsContent.Title_AcademicYear_Text, Value = AcademicYear.ToString(), RouteName = RouteConstants.AddRegistrationAcademicYear };

        public string GetSpecialismHiddenText => (SpecialismsDisplayName == null || !SpecialismsDisplayName.Any()) ? RegistrationDetailsContent.Specialism_None_Selected_Text : null;

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
