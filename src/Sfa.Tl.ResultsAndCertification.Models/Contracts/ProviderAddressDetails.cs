using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ProviderAddressDetails
    {
        public long UkPrn { get; set; }
        public string Name { get; set;}
        public string AddressLine1 { get; set;}
        public string AddressLine2 { get; set;}
        public string Town { get; set;}
        public string Postcode { get; set;}        
        public DateTime? CreatedOn { get; set;}
        public DateTime? ModifiedOn { get; set; }

    }
}
