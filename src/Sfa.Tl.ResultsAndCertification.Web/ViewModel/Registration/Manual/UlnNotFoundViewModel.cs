using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class UlnNotFoundViewModel
    {
        public string Uln { get; set; }
        public int RegistrationProfileId { get; set; }
        public bool IsRegisteredWithOtherAo { get; set; }
        public bool IsActive { get; set; }

        public bool IsUlnRegisteredAlready
        {
            get
            {
                return IsActive || IsRegisteredWithOtherAo;
            }
        }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel { RouteName = RouteConstants.AddRegistrationUln };
            }
        }
    }
}
