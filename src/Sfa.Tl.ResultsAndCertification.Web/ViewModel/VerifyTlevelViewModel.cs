using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel
{
    public class VerifyTlevelViewModel
    {
        public VerifyTlevelViewModel()
        {
            Specialisms = new List<string>();
        }

        public int TqAwardingOrganisationId { get; set; }
        public int RouteId { get; set; }
        public int PathwayId { get; set; }
        public int PathwayStatusId { get; set; }
        public string PathwayName { get; set; }
        [Required(ErrorMessage = "Select yes if this T Level’s details are correct")]
        public bool? IsEverythingCorrect { get; set; }
        public IEnumerable<string> Specialisms { get; set; }
    }
}
