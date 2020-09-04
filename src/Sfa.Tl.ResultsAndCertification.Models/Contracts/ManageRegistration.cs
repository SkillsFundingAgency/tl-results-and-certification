using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ManageRegistration : RegistrationRequest
    {
        public int ProfileId { get; set; }
        public string ModifiedBy { get; set; }
    }
}
