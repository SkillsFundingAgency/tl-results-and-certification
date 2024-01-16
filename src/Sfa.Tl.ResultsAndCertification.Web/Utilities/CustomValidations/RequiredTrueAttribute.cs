using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations
{
    public class RequiredTrueAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && (bool)value == true;
        }
    }
}