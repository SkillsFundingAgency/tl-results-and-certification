using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels
{
    public class TlevelQueriedDetailsViewModel : TlevelSummary
    {
        public TlevelQueriedDetailsViewModel()
        {
            Specialisms = new List<string>();
        }

        // TODO: Check if any unreference columns
        public int TqAwardingOrganisationId { get; set; }
        public int RouteId { get; set; }
        public int PathwayId { get; set; }
        public int PathwayStatusId { get; set; }
        public bool IsValid { get; set; }


        public BackLinkModel BackLink
        {
            get { return new BackLinkModel { RouteName = RouteConstants.QueriedTlevels }; }
        }
    }
}
