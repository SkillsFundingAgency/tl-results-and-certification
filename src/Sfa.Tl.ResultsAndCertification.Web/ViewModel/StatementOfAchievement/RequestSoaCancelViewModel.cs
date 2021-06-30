using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement
{
    public class RequestSoaCancelViewModel
    {
        public int ProfileId { get; set; }
        public string LearnerName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.RequestSoaCancel), ErrorMessageResourceName = "Validation_Message")]
        public bool? CancelRequest { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.RequestSoaCheckAndSubmit,
                    RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
                };
            }
        }
    }
}