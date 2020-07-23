using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class UlnCannotBeRegisteredViewModel
    {
        public string Uln { get; set; }
        public int RegistrationProfileId { get; set; }
        public bool IsRegisteredWithOtherAo { get; set; }

        public bool IsUlnRegisteredAlready
        {
            get
            {
                return false;
                // return RegistrationProfileId > 0;
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
