using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.ChangeRecordLink
{
    public class ChangeRecordModel
    {
        public ChangeRecordModel()
        {
            RouteAttributes = new Dictionary<string, string>();
        }

        public string Text { get; set; }

        public string Route { get; set; }

        public Dictionary<string, string> RouteAttributes { get; set; }
    }
}