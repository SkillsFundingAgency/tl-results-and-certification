using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryList
{
    public class SummaryListModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Value { get; set; }
        public string RouteName { get; set; }
        public Dictionary<string, string> RouteAttributes { get; set; }
        public string ActionText { get; set; }
        public string HiddenText { get; set; }
    }
}
