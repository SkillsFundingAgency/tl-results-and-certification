using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb
{
    public class BreadcrumbModel
    {
        public BreadcrumbModel()
        {
            BreadcrumbItems = new List<BreadcrumbItem>();
        }
        public List<BreadcrumbItem> BreadcrumbItems { get; set; }
    }
}
