using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel
{
    public class ConfirmTlevelViewModel
    {
        public ConfirmTlevelViewModel()
        {
            Specialisms = new List<string>();
        }

        public int TqAwardingOrganisationId { get; set; }
        public int RouteId { get; set; }
        public int PathwayId { get; set; }
        public int PathwayStatusId { get; set; }
        public string PathwayName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Verify), ErrorMessageResourceName = "IsEverythingCorrect_Required_Validation_Message")]
        public bool? IsEverythingCorrect { get; set; }
        public IEnumerable<string> Specialisms { get; set; }
        
        public BackLinkModel BackLink { get { return new BackLinkModel { RouteName = RouteConstants.TlevelSelect }; } }
    }
}
