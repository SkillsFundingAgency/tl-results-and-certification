namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ReJoinRegistrationRequest
    {
        public int ProfileId { get; set; }

        public long AoUkprn { get; set; }

        public string PerformedBy { get; set; }
    }
}
