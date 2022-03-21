using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsCancelRommUpdateViewModel
    {
        public int ProfileId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.PrsCancelAppealUpdate), ErrorMessageResourceName = "Validation_Message")]
        public bool? AreYouSureToCancel { get; set; }

        public BackLinkModel BackLink => new() { RouteName = RouteConstants.PrsRommCheckAndSubmit };
    }
}
