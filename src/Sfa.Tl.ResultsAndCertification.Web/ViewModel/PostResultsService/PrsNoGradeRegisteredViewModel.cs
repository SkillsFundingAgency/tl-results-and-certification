using NoGradeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsNoGradeRegistered;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsNoGradeRegisteredViewModel : PrsBaseViewModel
    {
        public PrsNoGradeRegisteredViewModel()
        {
            UlnLabel = NoGradeContent.Title_Uln_Text;
            LearnerNameLabel = NoGradeContent.Title_Name_Text;
            DateofBirthLabel = NoGradeContent.Title_DateofBirth_Text;
            ProviderNameLabel = NoGradeContent.Title_Provider_Text;
            TlevelTitleLabel = NoGradeContent.Title_TLevel_Text;
        }

        public int ProfileId { get; set; }
        public string AssessmentSeries { get; set; }
    }
}