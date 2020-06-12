using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration
{
    // Todo: rename valid name RegistrationCsvRecordResponse
    public class RegistrationCsvRecordResponse : ValidationState
    {
        public RegistrationCsvRecordResponse()
        {
            Specialisms = new List<string>();
            TlSpecialismLarIds = new List<KeyValuePair<int, string>>();
        }

        public int Uln { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public long Ukprn { get; set; }

        public DateTime StartDate { get; set; }

        public string Core { get; set; }

        public IEnumerable<string> Specialisms { get; set; }

        // Below properties are used for later stage i.e. Db process.
        public int TqProviderId { get; set; }
        public int TlProviderId { get; set; }
        public int TqAwardingOrganisationId { get; set; }
        public int TlAwardingOrganisatonId { get; set; }
        public int TlPathwayId { get; set; }
        public IEnumerable<KeyValuePair<int, string>> TlSpecialismLarIds { get; set; }
    }
}
