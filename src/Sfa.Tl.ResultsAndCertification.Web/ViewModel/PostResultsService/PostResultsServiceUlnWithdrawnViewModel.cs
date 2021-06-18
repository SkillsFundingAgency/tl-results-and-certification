using PostResultsServiceUlnWithdrawnContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PostResultsServiceUlnWithdrawn;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PostResultsServiceUlnWithdrawnViewModel : PostResultsServiceBaseViewModel
    {
        public PostResultsServiceUlnWithdrawnViewModel()
        {
            UlnLabel = PostResultsServiceUlnWithdrawnContent.Title_Uln_Text;
            LearnerNameLabel = PostResultsServiceUlnWithdrawnContent.Title_Name_Text;
            DateofBirthLabel = PostResultsServiceUlnWithdrawnContent.Title_DateofBirth_Text;
            ProviderNameLabel = PostResultsServiceUlnWithdrawnContent.Title_Provider_Text;
            TlevelTitleLabel = PostResultsServiceUlnWithdrawnContent.Title_TLevel_Text;
        }
    }
}