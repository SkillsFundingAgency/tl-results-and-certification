using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel
{
    public class TlevelQueryViewModel
    {
        public TlevelQueryViewModel()
        {
            Specialisms = new List<string>();
        }

        public int TqAwardingOrganisationId { get; set; }
        public int PathwayStatusId { get; set; }
        public string PathwayName { get; set; }
        public IEnumerable<string> Specialisms { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.SelectToReview), ErrorMessageResourceName = "Required_Validation_Message")]
        public string Query { get; set; }
    }
}
