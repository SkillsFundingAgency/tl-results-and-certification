using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Models.BulkProcess
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
