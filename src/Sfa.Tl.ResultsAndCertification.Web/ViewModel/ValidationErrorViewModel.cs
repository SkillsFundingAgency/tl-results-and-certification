using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel
{
    public class ValidationErrorViewModel
    {
        public string ElementId { get; set; }

        public ModelStateDictionary ModelState { get; set; }
    }
}
