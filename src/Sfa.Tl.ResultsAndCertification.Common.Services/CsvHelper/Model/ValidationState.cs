using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model
{
    public class ValidationState<T>
    {
        public ValidationState()
        {
            ValidationErrors = new List<T>();
        }

        public IList<T> ValidationErrors { get; set; }

        public bool IsValid { get { return ValidationErrors.Count() == 0; } }
    }
}
