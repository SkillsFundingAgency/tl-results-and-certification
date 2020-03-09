using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "TODO: Enter a valid")]
        public string Query { get; set; }
    }
}
