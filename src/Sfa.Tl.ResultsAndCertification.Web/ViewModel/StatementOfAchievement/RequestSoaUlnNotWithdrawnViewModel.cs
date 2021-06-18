using RequestSoaUlnNotWithdrawnContent = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaUlnNotWithdrawn;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement
{
    public class RequestSoaUlnNotWithdrawnViewModel : RequestSoaBaseViewModel
    {
        public RequestSoaUlnNotWithdrawnViewModel()
        {
            UlnLabel = RequestSoaUlnNotWithdrawnContent.Title_Uln_Text;
            LearnerNameLabel = RequestSoaUlnNotWithdrawnContent.Title_Name_Text;
            DateofBirthLabel = RequestSoaUlnNotWithdrawnContent.Title_DateofBirth_Text;
            ProviderNameLabel = RequestSoaUlnNotWithdrawnContent.Title_Provider_Text;
            TlevelTitleLabel = RequestSoaUlnNotWithdrawnContent.Title_TLevel_Text;
        }
    }
}