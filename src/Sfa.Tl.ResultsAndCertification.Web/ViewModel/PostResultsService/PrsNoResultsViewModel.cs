using NoResultsContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsNoResults;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsNoResultsViewModel : PrsBaseViewModel
    {
        public PrsNoResultsViewModel()
        {
            LearnerNameLabel = NoResultsContent.Title_Name_Text;
            UlnLabel = NoResultsContent.Title_Uln_Text;            
            DateofBirthLabel = NoResultsContent.Title_DateofBirth_Text;
            ProviderNameLabel = NoResultsContent.Title_Provider_Name_Text;
            ProviderUkprnLabel = NoResultsContent.Title_Provider_Ukprn_Text;
            TlevelTitleLabel = NoResultsContent.Title_TLevel_Text;
        }

        public int ProfileId { get; set; }
    }
}