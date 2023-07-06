namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class DualSpecialismDetails : BaseModel
    {
        public int TlPathwayId { get; set; }
        public string LarId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual PathwayDetails PathwayDetails { get; set; }
    }
}
