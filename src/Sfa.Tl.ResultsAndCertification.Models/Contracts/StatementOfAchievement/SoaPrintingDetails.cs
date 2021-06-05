using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement
{
    public class SoaPrintingDetails
    {
        public long Uln { get; set; }
        public string Name { get; set; }
        public string Dateofbirth { get; set; }
        public string ProviderName { get; set; }
        public string TlevelTitle { get; set; }
        public string Core { get; set; }
        public string CoreGrade { get; set; }
        public string Specialism { get; set; }
        public string SpecialismGrade { get; set; }
        public string EnglishAndMaths { get; set; }
        public string IndustryPlacement { get; set; }
        public Address ProviderAddress { get; set; }
    }
}