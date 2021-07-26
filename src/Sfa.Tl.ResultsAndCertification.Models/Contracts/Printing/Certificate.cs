namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing
{
    public class Certificate
    {
        public string CertificateNumber { get; set; }
        public string Type { get; set; }
        public int Uln { get; set; }
        public string LearnerName { get; set; }
        public LearningDetails LearningDetails { get; set; }
    }
}
