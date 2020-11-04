namespace Sfa.Tl.ResultsAndCertification.Common.Constants
{
    public class ValidationMessages
    {
        // Todo: can this move to resource file? assess. 
        // Property validation messages
        public const string Required = "{0} required";
        public const string MustBeNumberWithLength = "{0} must be a {1} digit number";
        public const string MustBeAnNumberWithLength = "{0} must be an {1} digit number";
        public const string StringLength = "{0} cannot have more than {1} characters";
        public const string MustBeStringWithLength = "{0} must have {1} characters only";
        public const string MustBeValidDate = "{0} must be a valid date in DDMMYYYY format";
        public const string DateNotinFuture = "{0} must be in the past";

        public const string MustBeInFormat = "{0} must be in the format {1}";
        public const string MustBeCurrentOne = "{0} must be the current one";

        // File based validation messages
        public const string FileHeaderNotFound = "File header is not valid";
        public const string NoRecordsFound = "No registration data received";
        public const string DuplicateRecord = "Duplicate ULN found";
        public const string InvalidColumnFound = "Data in more than the required {0} columns";

        // Generic or unexpected behaviour messages
        public const string UnableToParse = "Unable to parse the row.";
        public const string UnableToReadCsvData = "Unable to interpret content.";
        public const string UnexpectedError = "Unexpected error while reading file content.";

        // Bulk Registration Stage2 Validation Messages
        public const string DuplicateSpecialism = "Cannot have duplicate specialism codes for one registration";

        // Bulk Registration Stage3 Validation Messages
        public const string ProviderNotRegisteredWithAo = "Provider not registered with awarding organisation";
        public const string CoreNotRegisteredWithProvider = "Core not registered with provider";
        public const string SpecialismNotValidWithCore = "Specialism not valid with core";

        // Bulk Registration Stage4 Validation Messages
        public const string ActiveUlnWithDifferentAo = "Active ULN with a different awarding organisation";
        public const string CoreForUlnCannotBeChangedYet = "Core for ULN cannot be changed yet";
        public const string UlnNotRegistered = "ULN not registered with awarding organisation";
        public const string CannotAddAssessmentToWithdrawnRegistration = "Cannot add assessment entries to a withdrawn registration";
        public const string InvalidCoreCode = "Core code either not recognised or not registered for this ULN";
        public const string InvalidSpecialismCode = "Specialism code either not recognised or not registered for this ULN";
        public const string CoreEntryOutOfRange = "Core assessment entry must be no more than 4 years after the starting academic year";
        public const string SpecialismEntryOutOfRange = "Specialism assessment entry must be between one and 4 years after the starting academic year";

        // Assessments - Bulk stage 2 validations
        public const string CorecodeMustBeDigitsOnly = "Core code must have 8 digits only";
        public const string CorecodeRequired = "Core code required when core assessment entry is included";
        public const string SpecialismcodeRequired = "Specialism code required when core assessment entry is included";
        public const string CoreAssementEntryInvalidFormat = "Core assessment entry must be a series followed by a space and a 4 digit year";
        public const string SpecialismAssementEntryInvalidFormat = "Specialism assessment entry must be a series followed by a space and a 4 digit year";
        public const string AtleastOneEntryRequired = "File must contain at least one ULN on one row";
    }
}
