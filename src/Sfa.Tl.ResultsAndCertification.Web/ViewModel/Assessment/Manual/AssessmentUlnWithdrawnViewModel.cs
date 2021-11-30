using AssessmentWithdrawnDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AssessmentWithdrawnDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class AssessmentUlnWithdrawnViewModel : AssessmentBaseViewModel
    {
        public AssessmentUlnWithdrawnViewModel()
        {
            UlnLabel = AssessmentWithdrawnDetailsContent.Title_Uln_Text;
            LearnerNameLabel = AssessmentWithdrawnDetailsContent.Title_Name_Text;
            DateofBirthLabel = AssessmentWithdrawnDetailsContent.Title_DateofBirth_Text;
            ProviderNameLabel = AssessmentWithdrawnDetailsContent.Title_Provider_Text;
            TlevelTitleLabel = AssessmentWithdrawnDetailsContent.Title_TLevel_Text;
        }
    }
}
