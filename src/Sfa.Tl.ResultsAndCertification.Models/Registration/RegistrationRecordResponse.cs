using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Tl.ResultsAndCertification.Models.Registration
{
    public class RegistrationRecordResponse : ValidationState<RegistrationValidationError>
    {
        public RegistrationRecordResponse()
        {
            TlSpecialismLarIds = new List<KeyValuePair<int, string>>();
        }

        public int RowNum { get; set; }
        public int TqProviderId { get; set; }
        public int TlProviderId { get; set; }
        public int TqAwardingOrganisationId { get; set; }
        public int TlAwardingOrganisatonId { get; set; }
        public int TlPathwayId { get; set; }
        public IEnumerable<KeyValuePair<int, string>> TlSpecialismLarIds { get; set; }
    }
}
