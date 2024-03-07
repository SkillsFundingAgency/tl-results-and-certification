using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog
{
    public class AdminSearchChangeLogDetailsViewModel
    {
        public int ChangeLogId { get; set; }

        public string DateAndTimeOfChange { get; set; }

        public string ZendeskTicketID { get; set; }

        public string Learner { get; set; }

        public string Provider { get; set; }

        public string LastUpdatedBy { get; set; }
    }
}