namespace Sfa.Tl.ResultsAndCertification.Common.Constants
{
    public class ValidationMessages
    {
        // Property validation messages
        public const string Required = "{0} required";
        public const string MustBeNumberWithLength = "{0} must be a {1} digit number";
        public const string MustBeAnNumberWithLength = "{0} must be an {1} digit number";
        public const string MustHaveDigitsWithLength = "{0} must have {1} digits only";
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

        // Bulk Registration Stage3 Validation Messages
        public const string ProviderNotRegisteredWithAo = "Provider not registered with awarding organisation";
        public const string CoreNotRegisteredWithProvider = "Core not registered with provider";
        public const string SpecialismNotValidWithCore = "Specialism not valid with core";

        // Bulk Registration Stage4 Validation Messages
        public const string ActiveUlnWithDifferentAo = "Active ULN with a different awarding organisation";
        public const string CoreForUlnCannotBeChangedYet = "Core for ULN cannot be changed yet";
        public const string RegistrationCannotBeInWithdrawnStatus = "Cannot upload data for a withdrawn registration or make it active - these can only be done manually";

        // Assessments - Bulk stage 2 validations
        public const string CorecodeMustBeDigitsOnly = "Core code must have 8 digits only";
        public const string CorecodeRequired = "Core code required when core assessment entry is included";
        public const string SpecialismcodeRequired = "Specialism code required when core assessment entry is included";
        public const string CoreAssementEntryInvalidFormat = "Core assessment entry format must be text followed by a space and a 4-digit year";
        public const string SpecialismAssementEntryInvalidFormat = "Specialism assessment entry format must be text followed by a space and a 4-digit year";
        public const string AtleastOneEntryRequired = "File must contain at least one ULN on one row";
        public const string NoDataAfterUln = "No data after ULN - need at least one core code or one specialism code";

        // Assesments - Bulk stage 3 validations
        public const string UlnNotRegistered = "ULN not registered with awarding organisation";
        public const string CannotAddAssessmentToWithdrawnRegistration = "Cannot add assessment entries to a withdrawn registration";
        public const string InvalidCoreCode = "Core code either not recognised or not registered for this ULN";
        public const string InvalidSpecialismCode = "Specialism code either not recognised or not registered for this ULN";
        public const string InvalidCoreAssessmentEntry = "Core assessment entry must be in the format detailed in the 'format rules' and 'example' columns in the technical specification and can only be for the next available series (the second series of the first academic year or subsequent approaching series)";
        public const string InvalidNextCoreAssessmentEntry = "Core assessment entry is beyond the next available series - only the next available series is allowed (the second series of the first academic year or subsequent approaching series)";
        public const string InvalidSpecialismAssessmentEntry = "Specialism assessment entry must be in the format detailed in the 'format rules' and 'example' columns in the technical specification and can only be for the next available series (the second series of the second academic year or subsequent approaching series)";
        public const string InvalidNextSpecialismAssessmentEntry = "Specialism assessment entry is beyond the next available series - only the next available series is allowed (the second series of the second academic year or subsequent approaching series)";

        // Results - Bulk Stage 2 validations
        public const string CorecodeRequiredWhenResultIncluded = "Core component code required when result is included";
        public const string AssessmentSeriesNeedsToBeProvided = "Assessment series needs to be provided";
        public const string InvalidCoreAssessmentSeries = "Core assessment series format must be text followed by a space and a 4-digit year";
        public const string NoDataAfterUlnNeedCoreCode = "No data after ULN - need a core component code";

        // Results - Bulk stage 3 validations
        public const string CannotAddResultToWithdrawnRegistration = "Cannot add results to a withdrawn registration";
        public const string InvalidCoreComponentCode = "Core component code either not recognised or not registered for this ULN";
        public const string InvalidCoreAssessmentSeriesEntry = "Assessment series does not exist - see results data format and rules guide for examples of valid series";
        public const string NoCoreAssessmentEntryCurrentlyActive = "No assessment entry is currently active for the core component on this registration - needs adding first through assessment entries file upload or manual entry";
        public const string AssessmentSeriesDoesNotMatchTheSeriesOnTheRegistration = "Assessment series does not match the series on the registration";
        public const string InvalidCoreComponentGrade = "Core component grade not valid - needs to be A* to E, or Unclassified";

        // Results - Bulk stage 4 validations
        public const string ResultCannotBeInBeingAppealedStatus = "This learner's results cannot be changed because they are appealing a grade. Please remove this learner and try again.";
    }
}