using Sfa.Tl.ResultsAndCertification.Web.Content.Result;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result
{
    public class UploadSuccessfulViewModel
    {
        public BulkUploadStatsViewModel Stats { get; set; }

        public string SuccessfulResultText
        {
            get
            {
                if (Stats?.TotalRecordsCount == 1)
                {
                    return UploadSuccessful.Successfully_Sent_One_Result_Text;
                }
                else
                {
                    return string.Format(UploadSuccessful.Successfully_Sent_Total_Results_Text, Stats?.TotalRecordsCount);
                }
            }
        }
    }
}
