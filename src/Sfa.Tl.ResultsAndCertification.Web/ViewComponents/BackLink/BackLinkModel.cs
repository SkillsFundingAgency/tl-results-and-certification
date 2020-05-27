using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink
{
    public class BackLinkModel
    {
        public BackLinkModel()
        {
            RouteAttributes = new Dictionary<string, string>();
        }

        public string RouteName { get; set; }
        public Dictionary<string, string> RouteAttributes { get; set; }
    }
}
