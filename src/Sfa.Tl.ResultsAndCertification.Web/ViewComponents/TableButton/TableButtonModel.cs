using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.TableButton
{
    public class TableButtonModel
    {
        public string Text { get; set; }

        public string Route { get; set; }

        public Dictionary<string, string> RouteAttributes { get; set; }
    }
}