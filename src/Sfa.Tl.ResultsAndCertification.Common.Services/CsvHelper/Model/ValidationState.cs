using CsvHelper;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model
{
    public class ValidationState
    {
        public ValidationState()
        {
            ValidationErrors = new List<ValidationError>();
        }

        public IList<ValidationError> ValidationErrors { get; set; }

        public bool IsValid { get { return ValidationErrors.Count() == 0; } }
    }
}
