using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Tl.ResultsAndCertification.Models.Authentication
{
    public class Organisation
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the urn.</summary>
        /// <value>The urn.</value>
        public int? URN { get; set; }

        /// <summary>Gets or sets the uid.</summary>
        /// <value>The uid.</value>
        public int? UID { get; set; }

        /// <summary>Gets or sets the ukprn.</summary>
        /// <value>The ukprn.</value>
        public int? UKPRN { get; set; }

        /// <summary>Gets or sets the establishment number.</summary>
        /// <value>The establishment number.</value>
        public int? EstablishmentNumber { get; set; }

        /// <summary>Gets or sets the telephone.</summary>
        /// <value>The telephone.</value>
        public string Telephone { get; set; }

        /// <summary>Gets or sets the legacy identifier.</summary>
        /// <value>The legacy identifier.</value>
        public int? LegacyId { get; set; }

        /// <summary>Gets or sets the company registration number.</summary>
        /// <value>The company registration number.</value>
        public int? CompanyRegistrationNumber { get; set; }
    }
}
