using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel
{
    public class UlnNotFoundViewModel
    {
        public string Uln { get; set; }
        public int RegistrationProfileId { get; set; }
        public bool IsRegisteredWithOtherAo { get; set; }
        public bool IsAllowed { get; set; }
        public bool IsWithdrawn { get; set; }
        public virtual BackLinkModel BackLink { get; set; }
    }
}
