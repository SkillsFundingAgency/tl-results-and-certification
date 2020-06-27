using Sfa.Tl.ResultsAndCertification.Web.Content.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration
{
    public class UploadSuccessfulViewModel
    {
        public StatsViewModel Stats { get; set; }

        public bool HasMoreThanOneStatsToShow { get { return !(Stats?.TotalRecordsCount == Stats?.NewRecordsCount || Stats?.TotalRecordsCount == Stats?.UpdatedRecordsCount || Stats?.TotalRecordsCount == Stats?.DuplicateRecordsCount); } }

        public bool HasNewRegistrations { get { return Stats?.NewRecordsCount != 0; } }

        public bool HasAmendedRegistrations { get { return Stats?.UpdatedRecordsCount != 0; } }

        public bool HasUnchangedRegistrations { get { return Stats?.DuplicateRecordsCount != 0; } }

        public string SuccessfulRegistrationText
        {
            get
            {
                if (Stats?.TotalRecordsCount == Stats?.NewRecordsCount)
                {
                    return string.Format(UploadSuccessful.Successfully_Sent_New_Registrations_Text, Stats.NewRecordsCount);
                }
                else if (Stats?.TotalRecordsCount == Stats?.UpdatedRecordsCount)
                {
                    return string.Format(UploadSuccessful.Successfully_Sent_Amended_Registrations_Text, Stats.UpdatedRecordsCount);
                }
                else if (Stats?.TotalRecordsCount == Stats?.DuplicateRecordsCount)
                {
                    return string.Format(UploadSuccessful.Successfully_Sent_Unchanged_Registrations_Text, Stats.DuplicateRecordsCount);
                }
                else
                {
                    return string.Format(UploadSuccessful.Successfully_Sent_Total_Registrations_Text, Stats?.TotalRecordsCount);
                }
            }
        }
    }
}
