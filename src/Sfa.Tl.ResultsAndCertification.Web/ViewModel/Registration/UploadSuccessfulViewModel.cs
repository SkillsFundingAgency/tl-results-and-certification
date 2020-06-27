using Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration
{
    public class UploadSuccessfulViewModel
    {
        public BulkUploadStatsViewModel Stats { get; set; }

        public bool HasMoreThanOneStatsToShow { get { return !(Stats?.TotalRecordsCount == Stats?.NewRecordsCount || Stats?.TotalRecordsCount == Stats?.AmendedRecordsCount || Stats?.TotalRecordsCount == Stats?.UnchangedRecordsCount); } }

        public bool HasNewRegistrations { get { return Stats?.NewRecordsCount != 0; } }

        public bool HasAmendedRegistrations { get { return Stats?.AmendedRecordsCount != 0; } }

        public bool HasUnchangedRegistrations { get { return Stats?.UnchangedRecordsCount != 0; } }

        public string SuccessfulRegistrationText
        {
            get
            {
                if (Stats?.TotalRecordsCount == Stats?.NewRecordsCount)
                {
                    return string.Format(UploadSuccessful.Successfully_Sent_New_Registrations_Text, Stats.NewRecordsCount);
                }
                else if (Stats?.TotalRecordsCount == Stats?.AmendedRecordsCount)
                {
                    return string.Format(UploadSuccessful.Successfully_Sent_Amended_Registrations_Text, Stats.AmendedRecordsCount);
                }
                else if (Stats?.TotalRecordsCount == Stats?.UnchangedRecordsCount)
                {
                    return string.Format(UploadSuccessful.Successfully_Sent_Unchanged_Registrations_Text, Stats.UnchangedRecordsCount);
                }
                else
                {
                    return string.Format(UploadSuccessful.Successfully_Sent_Total_Registrations_Text, Stats?.TotalRecordsCount);
                }
            }
        }
    }
}
