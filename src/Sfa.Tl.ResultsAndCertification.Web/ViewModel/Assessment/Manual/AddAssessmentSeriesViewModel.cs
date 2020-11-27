using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class AddAssessmentSeriesViewModel
    {
        public int ProfileId { get; set; }

        public int AssessmentSeriesId { get; set; }
        public string AssessmentSeriesName { get; set; }

        public bool? IsOpted { get; set; }

        public BackLinkModel BackLink {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.AssessmentDetails,
                    RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
                };
            }
        }
    }
}
