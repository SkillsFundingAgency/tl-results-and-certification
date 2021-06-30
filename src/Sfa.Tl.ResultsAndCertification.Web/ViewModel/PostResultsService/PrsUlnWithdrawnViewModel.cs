using PrsUlnWithdrawnContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsUlnWithdrawn;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsUlnWithdrawnViewModel : PrsBaseViewModel
    {
        public PrsUlnWithdrawnViewModel()
        {
            UlnLabel = PrsUlnWithdrawnContent.Title_Uln_Text;
            LearnerNameLabel = PrsUlnWithdrawnContent.Title_Name_Text;
            DateofBirthLabel = PrsUlnWithdrawnContent.Title_DateofBirth_Text;
            ProviderNameLabel = PrsUlnWithdrawnContent.Title_Provider_Text;
            TlevelTitleLabel = PrsUlnWithdrawnContent.Title_TLevel_Text;
        }
    }
}