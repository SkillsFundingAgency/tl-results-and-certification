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

        public void AddErrors(int rownum, string rowRef, ValidationResult validationResult = null)
        {
            foreach (var err in validationResult.Errors)
                AddError(rownum, rowRef, err.ErrorMessage);
        }

        public void AddError(int rowNum, string rowRef, string message) 
        {
            ValidationErrors.Add(new ValidationError
            {
                RowNum = rowNum,
                RowRef = rowRef,
                ErrorMessage = message
            });
        }
    }
}
