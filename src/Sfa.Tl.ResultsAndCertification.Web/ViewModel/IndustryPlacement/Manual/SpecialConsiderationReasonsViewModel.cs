using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class SpecialConsiderationReasonsViewModel
    {
        public SpecialConsiderationReasonsViewModel()
        {
            ReasonsList = new List<IpLookupDataViewModel>();
        }

        public int ProfileId { get; set; }
        public string LearnerName { get; set; }
        public int AcademicYear { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(ErrorResource.IpSpecialConsiderationReasons), ErrorMessageResourceName = "Validation_Message_Select_One_Or_More_Reasons")]
        public bool? IsReasonSelected => (ReasonsList.Any(x => x.IsSelected) == true) ? true : (bool?)null;

        public IList<IpLookupDataViewModel> ReasonsList { get; set; }

        public virtual BackLinkModel BackLink => new() { RouteName = RouteConstants.IpSpecialConsiderationHours };
    }
}
