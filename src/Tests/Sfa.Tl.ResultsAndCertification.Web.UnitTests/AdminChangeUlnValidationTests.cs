using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests
{
    public class AdminChangeUlnValidationTests
    {
        [Fact]
        public void Valid_uln_does_not_require_review_audit_fields()
        {
            var model = new AdminChangeUlnViewModel { RegistrationPathwayId = 3, UlnTo = "9999999991" };
            var errors = new List<ValidationResult>();
            Assert.True(Validator.TryValidateObject(model, new ValidationContext(model), errors, true));
            Assert.Empty(errors);
        }
    }
}
