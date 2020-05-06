using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb
{
    public class BreadcrumbItem
    {
        public BreadcrumbItem()
        {
            RouteAttributes = new Dictionary<string, string>();
        }

        public string DisplayName { get; set; }
        public string RouteName { get; set; }
        public Dictionary<string, string> RouteAttributes { get; set; }
    }
}
