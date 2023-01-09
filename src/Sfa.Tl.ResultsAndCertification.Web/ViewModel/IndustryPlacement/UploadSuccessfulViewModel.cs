using Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement
{
    public class UploadSuccessfulViewModel
    {
        public BulkUploadStatsViewModel Stats { get; set; }

        public string SuccessfulIndustryPlacementsText
        {
            get
            {
                if (Stats?.TotalRecordsCount == 1)
                {
                    return UploadSuccessful.Successfully_Sent_One_Industry_Placement_Text;
                }
                else
                {
                    return string.Format(UploadSuccessful.Successfully_Sent_Total_Industry_Placements_Text, Stats?.TotalRecordsCount);
                }
            }
        }
    }
}
