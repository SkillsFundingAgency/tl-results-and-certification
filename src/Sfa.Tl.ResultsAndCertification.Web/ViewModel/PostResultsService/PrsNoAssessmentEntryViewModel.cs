using NoAssessmentContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsNoAssessmentEntry;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsNoAssessmentEntryViewModel : PrsBaseViewModel
    {
        public PrsNoAssessmentEntryViewModel()
        {
            UlnLabel = NoAssessmentContent.Title_Uln_Text;
            LearnerNameLabel = NoAssessmentContent.Title_Name_Text;
            DateofBirthLabel = NoAssessmentContent.Title_DateofBirth_Text;
            ProviderNameLabel = NoAssessmentContent.Title_Provider_Text;
            TlevelTitleLabel = NoAssessmentContent.Title_TLevel_Text;
        }
    }
}