using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class PathwaySpecialisms : BaseModel
    {
        public PathwaySpecialisms()
        {
            Specialisms = new List<PathwaySpecialismCombination>();
        }
        public string PathwayName { get; set; }
        public string PathwayCode { get; set; }
        public IEnumerable<PathwaySpecialismCombination> Specialisms { get; set; }
    }
}