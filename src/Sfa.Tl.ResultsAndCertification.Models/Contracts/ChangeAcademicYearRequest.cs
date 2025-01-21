namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ChangeAcademicYearRequest
    {
        public long Uln { get; set; }
        public int ProfileId { get; set; }
        public long AoUkprn { get; set; }
        public string PerformedBy { get; set; }
        public string AcademicYearChangeTo { get; set; }
    }
}
