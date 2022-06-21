using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination
{
    public class PaginationModel
    {
        public PaginationModel()
        {
            RouteAttributes = new Dictionary<string, string>();
        }

        public PagerViewModel PagerInfo { get; set; }

        public string PaginationSummary { get; set; }
        public string RouteName { get; set; }
        public Dictionary<string, string> RouteAttributes { get; set; }
    }
}