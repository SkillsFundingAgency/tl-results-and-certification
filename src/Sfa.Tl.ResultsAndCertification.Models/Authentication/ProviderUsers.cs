using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Authentication
{
    public class DfeUsers
    {
        [JsonProperty("ukprn")]
        public string Ukprn { get; set; }

        [JsonProperty("users")]
        public IEnumerable<ServiceUser> Users { get; set; }
    }

    public class ServiceUser
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DfeUserStatus UserStatus { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    public enum DfeUserStatus
    {
        Active = 1,
        Inactive = 0
    }
}
