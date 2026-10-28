using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminChangeUlnViewModel
    {
        public int RegistrationPathwayId { get; set; }
        public long Uln { get; set; }

        [Required(ErrorMessageResourceType = typeof(ChangeUln), ErrorMessageResourceName = "Validation_Uln_Must_Be_Digits")]
        [RegularExpression("^((?!(0))[0-9]{10})$", ErrorMessageResourceType = typeof(ChangeUln), ErrorMessageResourceName = "Validation_Uln_Must_Be_Digits")]
        public string UlnTo { get; set; }
    }
}
