using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement
{
    public class LearningDetails
    {
        public string TLevelTitle { get; set; }
        public string Grade { get; set; }
        public string Date { get; set; }
        public string Core { get; set; }
        public string CoreGrade { get; set; }
        public IList<OccupationalSpecialismDetails> OccupationalSpecialism { get; set; }
        public string IndustryPlacement { get; set; }
        public string EnglishAndMaths { get; set; }       
    }
}