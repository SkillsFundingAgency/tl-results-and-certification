using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Shared;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminRemoveAssessmentEntryViewModel // Core and specialism
    {
        public int RegistrationPathwayId { get; set; }

        public int PathwayAssessmentId { get; set; }

        public PersonalDetailsViewModel PersonalDetails { get; set; }

        public AdminAssessmentViewModel Assessment { get; set; } // Create a shared one and reduced?

        //[Required(ErrorMessageResourceType = typeof(PrsAddRommContent), ErrorMessageResourceName = "Validation_Message")]
        public bool? DoYouWantToRemoveThisAssessmentEntry { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminLearnerRecord,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
        };
    }
}