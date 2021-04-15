using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual
{
    public class QueryEnglishAndMathsViewModel
    {
        public int ProfileId { get; set; }
        public string Name { get; set; }
        
        public BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.LearnerRecordDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }
}
