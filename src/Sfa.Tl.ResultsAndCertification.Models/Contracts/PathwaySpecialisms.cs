using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class PathwaySpecialisms : BaseModel
    {
        public PathwaySpecialisms()
        {
            PathwaySpecialismCombinations = new List<PathwaySpecialismCombination>();
        }
        public string PathwayName { get; set; }
        public string PathwayCode { get; set; }
        public IList<SpecialismDetails> Specialisms { get; set; } // TODO: to be removed
        public IEnumerable<PathwaySpecialismCombination> PathwaySpecialismCombinations { get; set; }

    }
}
