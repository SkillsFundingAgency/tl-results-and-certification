using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminRemovePathwayAssessmentEntryViewModel
    {
        public int RegistrationPathwayId { get; set; }

        public string PathwayName { get; set; }

        #region Personal details

        public string Learner { get; set; }

        public long Uln { get; set; }

        public string Provider { get; set; }

        public string Tlevel { get; set; }

        public string StartYear { get; set; }

        public SummaryItemModel SummaryLearner
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_Learner_Id, RemoveAssessmentEntryCore.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_ULN_Id, RemoveAssessmentEntryCore.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_Provider_Id, RemoveAssessmentEntryCore.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_TLevel_Id, RemoveAssessmentEntryCore.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_StartYear_Id, RemoveAssessmentEntryCore.Summary_StartYear_Text, StartYear);

        private static SummaryItemModel CreateSummaryItemModel(string id, string title, string value)
             => new()
             {
                 Id = id,
                 Title = title,
                 Value = value
             };

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public string Grade { get; set; }

        public string LastUpdated { get; set; }

        public string UpdatedBy { get; set; }

        #endregion

        public bool CanAssessmentEntryBeRemoved { get; set; }

        [Required(ErrorMessageResourceType = typeof(RemoveAssessmentEntryCore), ErrorMessageResourceName = "Validation_Message")]
        public bool? DoYouWantToRemoveThisAssessmentEntry { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminLearnerRecord,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
        };
    }
}