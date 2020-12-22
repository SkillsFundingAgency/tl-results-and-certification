using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders.BulkRegistrations.ValidationErrorsBuilder
{
    public class BulkRegistrationValidationErrorsBuilder
    {
        public IList<BulkProcessValidationError> BuildRequiredValidationErrorsList() => new List<BulkProcessValidationError>
        {
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "",
                ErrorMessage = "ULN required"
            },
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "",
                ErrorMessage = "First name required"
            },
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "",
                ErrorMessage = "Last name required"
            },
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "",
                ErrorMessage = "Date of birth required"
            },
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "",
                ErrorMessage = "UKPRN required"
            },
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "",
                ErrorMessage = "Academic year required"
            },            
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "",
                ErrorMessage = "Core code required"
            }
        };

        public IList<BulkProcessValidationError> BuildValidationErrorsList() => new List<BulkProcessValidationError>
        {
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "ULN must be a 10 digit number"
            },
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "First name cannot have more than 100 characters"
            },
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "Last name cannot have more than 100 characters"
            },
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "Date of birth must be a valid date in DDMMYYYY format"
            },
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "UKPRN must be an 8 digit number"
            },
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "Academic year must be the current one"
            },
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "Core code must have 8 characters only"
            },
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "Specialism code must have 8 characters only"
            }
        };

        public IList<BulkProcessValidationError> BuildStage3ValidationErrorsList() => new List<BulkProcessValidationError>
        {
            new BulkProcessValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = ValidationMessages.ProviderNotRegisteredWithAo
            },
            new BulkProcessValidationError
            {
                RowNum = "3",
                Uln = "111111112",
                ErrorMessage = ValidationMessages.CoreNotRegisteredWithProvider
            },
            new BulkProcessValidationError
            {
                RowNum = "4",
                Uln = "111111113",
                ErrorMessage = ValidationMessages.SpecialismNotValidWithCore
            }
        };

        public IList<BulkProcessValidationError> BuildStage4ValidationErrorsList() => new List<BulkProcessValidationError>
        {
            new BulkProcessValidationError
            {
                Uln = "1111111111",
                ErrorMessage = ValidationMessages.ActiveUlnWithDifferentAo
            },
            new BulkProcessValidationError
            {
                Uln = "1111111112",
                ErrorMessage = ValidationMessages.CoreForUlnCannotBeChangedYet
            },
            new BulkProcessValidationError
            {
                Uln = "1111111113",
                ErrorMessage = ValidationMessages.RegistrationCannotBeInWithdrawnStatus
            }
        };
    }
}
