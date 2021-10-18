namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Ucas
{
    public class UcasDataRequest
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string FileHash { get; set; }
    }
}