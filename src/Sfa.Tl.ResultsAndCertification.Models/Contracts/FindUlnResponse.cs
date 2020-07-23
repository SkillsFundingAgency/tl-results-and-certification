using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class FindUlnResponse
    {
        public int RegistrationProfileId { get; set; }
        public long Uln { get; set; }
        public bool IsRegisteredWithOtherAo { get; set; }
    }
}
