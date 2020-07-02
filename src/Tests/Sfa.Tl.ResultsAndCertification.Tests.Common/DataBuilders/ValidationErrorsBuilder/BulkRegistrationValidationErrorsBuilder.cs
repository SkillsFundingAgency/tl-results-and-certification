using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders.BulkRegistrations.ValidationErrorsBuilder
{
    public class BulkRegistrationValidationErrorsBuilder
    {
        public IList<RegistrationValidationError> BuildRequiredValidationErrorsList() => new List<RegistrationValidationError>
        {
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "",
                ErrorMessage = "ULN required"
            },
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "",
                ErrorMessage = "First name required"
            },
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "",
                ErrorMessage = "Last name required"
            },
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "",
                ErrorMessage = "Date of birth required"
            },
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "",
                ErrorMessage = "UKPRN required"
            },
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "",
                ErrorMessage = "Start date required"
            },            
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "",
                ErrorMessage = "Core code required"
            }
        };

        public IList<RegistrationValidationError> BuildValidationErrorsList() => new List<RegistrationValidationError>
        {
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "ULN must be a 10 digit number"
            },
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "First name cannot have more than 100 characters"
            },
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "Last name cannot have more than 100 characters"
            },
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "Date of birth must be a valid date in DDMMYYYY format"
            },
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "UKPRN must be an 8 digit number"
            },
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "Start date must be a valid date in DDMMYYYY format"
            },
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "Core code must have 8 characters only"
            },
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = "Specialism code must have 8 characters only"
            }
        };

        public IList<RegistrationValidationError> BuildStage3ValidationErrorsList() => new List<RegistrationValidationError>
        {
            new RegistrationValidationError
            {
                RowNum = "2",
                Uln = "111111111",
                ErrorMessage = ValidationMessages.ProviderNotRegisteredWithAo
            },
            new RegistrationValidationError
            {
                RowNum = "3",
                Uln = "111111112",
                ErrorMessage = ValidationMessages.CoreNotRegisteredWithProvider
            },
            new RegistrationValidationError
            {
                RowNum = "4",
                Uln = "111111113",
                ErrorMessage = ValidationMessages.SpecialismNotValidWithCore
            }
        };

        public IList<RegistrationValidationError> BuildStage4ValidationErrorsList() => new List<RegistrationValidationError>
        {
            new RegistrationValidationError
            {
                Uln = "1111111111",
                ErrorMessage = ValidationMessages.ActiveUlnWithDifferentAo
            },
            new RegistrationValidationError
            {
                Uln = "1111111112",
                ErrorMessage = ValidationMessages.CoreForUlnCannotBeChangedYet
            }
        };
    }
}
