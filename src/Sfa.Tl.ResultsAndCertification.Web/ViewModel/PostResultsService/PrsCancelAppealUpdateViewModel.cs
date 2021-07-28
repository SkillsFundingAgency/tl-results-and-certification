using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsCancelAppealUpdateViewModel
    {
        public int ProfileId { get; set; }

        public int AssessmentId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.PrsCancelAppealUpdate), ErrorMessageResourceName = "Validation_Message")]
        public bool? CancelRequest { get; set; }

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.PrsPathwayGradeCheckAndSubmit
                };
            }
        }
    }
}
