using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Registration
{
    public class RegistrationRecordResponse : ValidationState<RegistrationValidationError>
    {
        public RegistrationRecordResponse()
        {
            TlSpecialismLarIds = new List<KeyValuePair<int, string>>();
        }

        public long Uln { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int AcademicYear { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int TqProviderId { get; set; }
        public int TlProviderId { get; set; }
        public int TqAwardingOrganisationId { get; set; }
        public int TlAwardingOrganisatonId { get; set; }
        public int TlPathwayId { get; set; }
        public IEnumerable<KeyValuePair<int, string>> TlSpecialismLarIds { get; set; }
    }
}
