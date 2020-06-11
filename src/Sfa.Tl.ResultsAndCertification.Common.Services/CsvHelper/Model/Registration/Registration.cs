using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration
{
    public class Registration : ValidationState
    {
        public Registration()
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

        public int RowNum { get; set; }

        // Below properties are used for later stage i.e. Db process.
        public int TqProviderId { get; set; }
        public int TlProviderId { get; set; }
        public int TqAwardingOrganisationId { get; set; }
        public int TlAwardingOrganisatonId { get; set; }
        public int TlPathwayId { get; set; }
        public IEnumerable<KeyValuePair<int, string>> TlSpecialismLarIds { get; set; }
    }
}
