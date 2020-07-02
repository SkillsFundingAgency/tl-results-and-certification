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

        public string NewRegistrationsText
        {
            get { return string.Format(Stats?.NewRecordsCount == 1 ? UploadSuccessful.New_Registrations_Singular_Text : UploadSuccessful.New_Registrations_Text, Stats?.NewRecordsCount); }            
        }

        public string AmendedRegistrationsText
        {
            get { return string.Format(Stats?.AmendedRecordsCount == 1 ? UploadSuccessful.Amended_Registrations_Singular_Text : UploadSuccessful.Amended_Registrations_Text, Stats?.AmendedRecordsCount); }
        }

        public string UnchangedRegistrationsText
        {
            get { return string.Format(Stats?.UnchangedRecordsCount == 1 ? UploadSuccessful.Unchanged_Registrations_Singular_Text : UploadSuccessful.Unchanged_Registrations_Text, Stats?.UnchangedRecordsCount); }
        }

        public string SuccessfulRegistrationText
        {
            get
            {
                if (Stats?.TotalRecordsCount == Stats?.NewRecordsCount)
                {
                    return string.Format(Stats?.NewRecordsCount == 1 ? UploadSuccessful.Successfully_Sent_New_Registrations_Singular_Text : UploadSuccessful.Successfully_Sent_New_Registrations_Text, Stats?.NewRecordsCount);
                }
                else if (Stats?.TotalRecordsCount == Stats?.AmendedRecordsCount)
                {
                    return string.Format(Stats?.AmendedRecordsCount == 1 ? UploadSuccessful.Successfully_Sent_Amended_Registrations_Singular_Text : UploadSuccessful.Successfully_Sent_Amended_Registrations_Text, Stats?.AmendedRecordsCount);
                }
                else if (Stats?.TotalRecordsCount == Stats?.UnchangedRecordsCount)
                {
                    return string.Format(Stats?.UnchangedRecordsCount == 1 ? UploadSuccessful.Successfully_Sent_Unchanged_Registrations_Singular_Text : UploadSuccessful.Successfully_Sent_Unchanged_Registrations_Text, Stats?.UnchangedRecordsCount);
                }
                else
                {
                    return string.Format(UploadSuccessful.Successfully_Sent_Total_Registrations_Text, Stats?.TotalRecordsCount);
                }
            }
        }
    }
}
