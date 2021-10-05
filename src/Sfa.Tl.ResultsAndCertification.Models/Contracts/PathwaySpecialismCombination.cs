using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class PathwaySpecialismCombination
    {
        public PathwaySpecialismCombination()
        {
            SpecialismDetails = new List<SpecialismDetails>();
        }

        public string Code { get { return string.Join("|", SpecialismDetails.Select(x => x.Code)); } }
        public IEnumerable<SpecialismDetails> SpecialismDetails { get; set; }
    }
}