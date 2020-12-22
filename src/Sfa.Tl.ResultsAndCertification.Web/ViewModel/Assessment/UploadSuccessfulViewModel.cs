using Sfa.Tl.ResultsAndCertification.Web.Content.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment
{
    public class UploadSuccessfulViewModel
    {
        public BulkUploadStatsViewModel Stats { get; set; }

        public string SuccessfulAssessmentEntriesText
        {
            get
            {
                if (Stats?.TotalRecordsCount == 1)
                {
                    return UploadSuccessful.Successfully_Sent_One_Assessment_Entry_Text;
                }
                else
                {
                    return string.Format(UploadSuccessful.Successfully_Sent_Total_Assessment_Entries_Text, Stats?.TotalRecordsCount);
                }
            }
        }
    }
}
