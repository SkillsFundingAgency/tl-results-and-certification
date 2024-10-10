using Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class UploadSuccessfulViewModel
    {
        public BulkUploadStatsViewModel Stats { get; set; }

        public bool HasMoreThanOneStatsToShow { get { return !(Stats?.TotalRecordsCount == Stats?.NewRecordsCount || Stats?.TotalRecordsCount == Stats?.AmendedRecordsCount || Stats?.TotalRecordsCount == Stats?.UnchangedRecordsCount); } }

        public bool HasNewRomms { get { return Stats?.NewRecordsCount != 0; } }

        public bool HasAmendedRomms { get { return Stats?.AmendedRecordsCount != 0; } }

        public bool HasUnchangedRomms { get { return Stats?.UnchangedRecordsCount != 0; } }

        public string NewRommsText
        {
            get { return string.Format(Stats?.NewRecordsCount == 1 ? UploadRommsSuccessful.New_Romms_Singular_Text : UploadRommsSuccessful.New_Romms_Text, Stats?.NewRecordsCount); }
        }

        public string AmendedRommsText
        {
            get { return string.Format(Stats?.AmendedRecordsCount == 1 ? UploadRommsSuccessful.Amended_Romms_Singular_Text : UploadRommsSuccessful.Amended_Romms_Text, Stats?.AmendedRecordsCount); }
        }

        public string UnchangedRommsText
        {
            get { return string.Format(Stats?.UnchangedRecordsCount == 1 ? UploadRommsSuccessful.Unchanged_Romms_Singular_Text : UploadRommsSuccessful.Unchanged_Romms_Text, Stats?.UnchangedRecordsCount); }
        }

        public string SuccessfulRommText
        {
            get
            {
                if (Stats?.TotalRecordsCount == Stats?.NewRecordsCount)
                {
                    return string.Format(Stats?.NewRecordsCount == 1 ? UploadRommsSuccessful.Successfully_Sent_New_Romms_Singular_Text : UploadRommsSuccessful.Successfully_Sent_New_Romms_Text, Stats?.NewRecordsCount);
                }
                else if (Stats?.TotalRecordsCount == Stats?.AmendedRecordsCount)
                {
                    return string.Format(Stats?.AmendedRecordsCount == 1 ? UploadRommsSuccessful.Successfully_Sent_Amended_Romms_Singular_Text : UploadRommsSuccessful.Successfully_Sent_Amended_Romms_Text, Stats?.AmendedRecordsCount);
                }
                else if (Stats?.TotalRecordsCount == Stats?.UnchangedRecordsCount)
                {
                    return string.Format(Stats?.UnchangedRecordsCount == 1 ? UploadRommsSuccessful.Successfully_Sent_Unchanged_Romms_Singular_Text : UploadRommsSuccessful.Successfully_Sent_Unchanged_Romms_Text, Stats?.UnchangedRecordsCount);
                }
                else
                {
                    return string.Format(UploadRommsSuccessful.Successfully_Sent_Total_Romms_Text, Stats?.TotalRecordsCount);
                }
            }
        }
    }
}
