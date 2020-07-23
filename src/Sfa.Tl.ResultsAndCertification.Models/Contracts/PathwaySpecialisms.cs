using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class PathwaySpecialisms : BaseModel
    {
        public string PathwayName { get; set; }
        public string PathwayCode { get; set; }
        public IList<SpecialismDetails> Specialisms { get; set; }
    }
}
