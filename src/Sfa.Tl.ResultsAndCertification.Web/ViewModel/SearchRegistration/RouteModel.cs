using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration
{
    public class RouteModel
    {
        public RouteModel() { }

        public RouteModel(string routeName, params (string Key, string Value)[] routeValues)
        {
            RouteName = routeName;
            RouteValues = routeValues.ToDictionary(p => p.Key, p => p.Value);
        }

        public string RouteName { get; set; }

        public Dictionary<string, string> RouteValues { get; set; }
    }
}
