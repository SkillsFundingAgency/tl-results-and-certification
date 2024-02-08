using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.Shared;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Shared
{
    public class PersonalDetailsViewModel
    {
        public string Learner { get; set; }

        public long Uln { get; set; }

        public string Provider { get; set; }

        public string Tlevel { get; set; }

        public string StartYear { get; set; }

        public SummaryItemModel SummaryLearner 
            => CreateSummaryItemModel(PersonalDetails.Summary_Learner_Id, PersonalDetails.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryULN 
            => CreateSummaryItemModel(PersonalDetails.Summary_ULN_Id, PersonalDetails.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider 
            => CreateSummaryItemModel(PersonalDetails.Summary_Provider_Id, PersonalDetails.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(PersonalDetails.Summary_TLevel_Id, PersonalDetails.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryAcademicYear
            => CreateSummaryItemModel(PersonalDetails.Summary_StartYear_Id, PersonalDetails.Summary_StartYear_Text, StartYear);

        private static SummaryItemModel CreateSummaryItemModel(string id, string title, string value)
             => new()
             {
                 Id = id,
                 Title = title,
                 Value = value
             };
    }
}