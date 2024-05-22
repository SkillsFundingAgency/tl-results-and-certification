using WithdrawResultContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultWithdrawnDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class ResultWithdrawnViewModel : ResultsBaseViewModel
    {
        public ResultWithdrawnViewModel()
        {
            UlnLabel = WithdrawResultContent.Title_Uln_Text;
            DateofBirthLabel = WithdrawResultContent.Title_DateofBirth_Text;
            ProviderUkprnLabel = WithdrawResultContent.Title_Provider_Ukprn_Text;
            ProviderNameLabel = WithdrawResultContent.Title_Provider_Name_Text;
            TlevelTitleLabel = WithdrawResultContent.Title_TLevel_Text;
        }
    }
}
