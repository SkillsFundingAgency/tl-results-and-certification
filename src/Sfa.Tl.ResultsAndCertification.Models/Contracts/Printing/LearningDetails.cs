using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing
{
    public class LearningDetails
    {
        public string TLevelTitle { get; set; }
        public string Grade { get; set; }
        public string Date { get; set; }
        public string Core { get; set; }
        public string CoreGrade { get; set; }
        public List<OccupationalSpecialism> OccupationalSpecialism { get; set; }
        public string IndustryPlacement { get; set; }
        public string EnglishAndMaths { get; set; }
        public List<object> MARS { get; set; }
    }
}
