using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Authentication
{
    public class DfeUserInfo
    {        
        /// <summary>Gets or sets the user identifier.</summary>
        /// <value>The user identifier.</value>
        public Guid UserId { get; set; }

        /// <summary>/// Gets or sets the first name./// </summary>
        /// <value>/// The first name./// </value>
        public string FirstName { get; set; }

        /// <summary>/// Gets or sets the surname.</summary>
        /// <value>/// The surname.</value>
        public string Surname { get; set; }

        /// <summary>Gets or sets the email.</summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>Gets or sets the ukprn.</summary>
        /// <value>The ukprn.</value>
        public long? Ukprn { get; set; }

        /// <summary>Gets or sets the roles.</summary>
        /// <value>The roles.</value>
        public IEnumerable<Role> Roles { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance has access to service.</summary>
        /// <value><c>true</c> if this instance has access to service; otherwise, <c>false</c>.</value>
        public bool HasAccessToService { get; set; } = true;
    }   
}
