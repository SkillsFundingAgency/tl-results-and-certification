﻿namespace Sfa.Tl.ResultsAndCertification.Common.Constants
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
        public const string MustBeNumberWithInRange = "{0} must be between {1} and {2} digit number";
        public const string CannotHaveValue = "{0} cannot have a value";
        public const string MustBeYesOrNo = "{0} must be a Yes or No";

        public const string MustBeInFormat = "{0} must be in the format {1}";

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
        public const string AcademicYearMustBeCurrentOne = "Academic year must be the current one";
        public const string AcademicYearIsNotValid = "Academic year is not valid";
        public const string ProviderNotRegisteredWithAo = "Provider not registered with awarding organisation";
        public const string CoreNotRegisteredWithProvider = "Core not registered with provider";
        public const string SpecialismNotValidWithCore = "Specialism not valid with core";
        public const string SpecialismIsNotValid = "Specialism is not valid";
        public const string SpecialismCannotBeSelectedAsSingleOption = "This specialism cannot be selected as a single option (for when only one code of a paired specialism has been entered)";

        // Bulk Withdrawal Learner Validation Messages
        public const string InactiveUln = "Inactive Uln";
        public const string InvalidDateOfBirth = "Invalid date of birth";
        public const string InvalidLastName = "Invalid last name";
        public const string InvalidResultState = "Active ROMM or Appeal";
        public const string InvalidRommResultState = "Active Romm, Appeal or no result unavailable.";
        public const string CoreRommWindowExpired = "Invalid Core ROMM period.";
        public const string SpecialismRommWindowExpired = "Invalid Specialism ROMM period.";
        public const string InvalidCoreResultState = "Active Core ROMM, Appeal or no result unavailable.";
        public const string InvalidSpecialismResultState = "Active Specialism ROMM, Appeal or no result unavailable.";

        // Bulk Registration Stage4 Validation Messages
        public const string ActiveUlnWithDifferentAo = "Active ULN with a different awarding organisation";
        public const string CoreForUlnCannotBeChangedYet = "Core for ULN cannot be changed yet";
        public const string RegistrationCannotBeInWithdrawnStatus = "Cannot upload data for a withdrawn registration or make it active - these can only be done manually";
        public const string AcademicYearCannotBeChanged = "Year of registration cannot be changed - the learner must be withdrawn and re-registered";
        public const string LearnerPreviouslyRegisteredWithAnotherAo = "This learner was previously registered with another AO. You must add it as an individual record before you can include it in a bulk upload";
        public const string SpecialismCannotBeRemovedWhenActiveAssessmentEntryExist = "This learner has been registered for an occupational specialism assessment so the specialism cannot be removed. Either remove the record from the bulk upload or remove the specialism assessment entry for this learner and try again";

        // Assessments - Bulk stage 2 validations
        public const string CorecodeMustBeDigitsOnly = "Core code must have 8 digits only";
        public const string CorecodeRequired = "Core code required when core assessment entry is included";
        public const string SpecialismcodeRequired = "Assessment entry series must be accompanied by a specialism code (or codes)";
        public const string CoreAssementEntryInvalidFormat = "Core assessment entry format must be text followed by a space and a 4-digit year";
        public const string SpecialismAssementEntryInvalidFormat = "Specialism assessment entry format must be text followed by a space and a 4-digit year";
        public const string SpecialismCodesMustBeDifferent = "Specialism codes must be two different numbers";
        public const string AtleastOneEntryRequired = "File must contain at least one ULN on one row";
        public const string NoDataAfterUln = "No data after ULN - need at least one core code or one specialism code";

        // Assesments - Bulk stage 3 validations
        public const string UlnNotRegistered = "ULN not registered with awarding organisation";
        public const string CannotAddAssessmentToWithdrawnRegistration = "Cannot add assessment entries to a withdrawn registration";
        public const string InvalidCoreCode = "Core code either not recognised or not registered for this ULN";
        public const string InvalidSpecialismCode = "There is a problem with the specialism code(s)";
        public const string InvalidCoreAssessmentEntry = "Core assessment entry must be in the format detailed in the 'format rules' and 'example' columns in the technical specification and can only be for the next available series (the second series of the first academic year or subsequent approaching series)";
        public const string InvalidNextCoreAssessmentEntry = "Available to add after the current assessment series has passed";
        public const string InvalidSpecialismAssessmentEntry = "Specialism assessment entry must be in the format detailed in the 'format rules' and 'example' columns in the technical specification. They can only be for the next available series.";
        public const string InvalidNextSpecialismAssessmentEntry = "Specialism assessment entry is beyond the next available series - only the next available series is allowed";
        public const string SpecialismCodeMustBePair = "Specialism code can only be entered or removed as part of a pair";

        // Assessments - Bulk stage 4 validations
        public const string AssessmentEntryForCoreCannotBeAddedUntilResultRecordedForExistingEntry = "This core component assessment cannot be entered as there is a previous assessment that does not have a result. Remove the previous assessment or add a result to proceed.";
        public const string AssessmentEntryForCoreCannotBeRemovedHasResult = "There is a core component assessment entry with an associated result recorded. The core assessment entry cannot be removed using bulk upload.";

        public const string AssessmentEntryForSpecialismCannotBeAddedUntilResultRecordedForExistingEntry = "This occupational specialism assessment cannot be entered as there is a previous assessment that does not have a result. Remove the previous assessment or add a result to proceed.";
        public const string AssessmentEntryForSpecialismCannotBeRemovedHasResult = "There is an occupational specialism assessment entry with an associated result recorded. The specialism assessment entry cannot be removed using bulk upload.";

        // Results - Bulk Stage 2 validations
        public const string CorecodeRequiredWhenResultIncluded = "Core component code required when result is included";
        public const string AssessmentSeriesNeedsToBeProvided = "Assessment series needs to be provided";
        public const string InvalidCoreAssessmentSeries = "Core assessment series format must be text followed by a space and a 4-digit year";
        public const string SpecialismCodeMustBeProvided = "ComponentCode (Specialisms) must be provided when there is an entry in the AssessmentSeries (Specialisms) field";
        public const string SpecialismSeriesRequired = "AssessmentSeries (Specialisms) cannot be blank";
        public const string SpecialismSeriesInvalidFormat = "AssessmentSeries (Specialisms) must contain the word Summer followed by a space and a 4-digit year";
        public const string SpecialismGradeCountMismatch = "There is only one paired specialism grade. Please either add another grade or indicate a blank field using comma separation.";
        public const string NoResultDataAfterUln = "No data provided for this learner. Please provide data or remove the row.";

        // Results - Bulk stage 3 validations
        public const string CannotAddResultToWithdrawnRegistration = "Cannot add results to a withdrawn registration";
        public const string InvalidCoreComponentCode = "Core component code either not recognised or not registered for this ULN";
        public const string InvalidCoreAssessmentSeriesEntry = "Assessment series does not exist - see results data format and rules guide for examples of valid series";
        public const string NoCoreAssessmentEntryCurrentlyActive = "No assessment entry is currently active for the core component on this registration - needs adding first through assessment entries file upload or manual entry";
        public const string AssessmentSeriesDoesNotMatchTheSeriesOnTheRegistration = "Assessment series does not match the series on the registration";
        public const string InvalidCoreRommComponentGrade = "Enter a valid grade for the core component. The grade must be A* to E, Unclassified.";
        public const string InvalidCoreComponentGrade = "Enter a valid grade for the core component. The grade must be A* to E, unclassified, Q - pending result or X - no result.";
        public const string CoreSeriesNotCurrentlyOpen = "Incorrect Assessment series";

        public const string SpecialismCodeNotRecognised = "Specialism code(s) either not recognised or not registered for this ULN";
        public const string NoSpecialismAssessmentEntryCurrentlyActive = "No assessment entry is currently active for the Specialism on this registration - needs adding first through assessment entries file upload or manual entry";
        public const string InvalidSpecialismAssessmentSeriesEntry = "Specialism assessment series does not exist";
        public const string SpecialismSeriesDoesNotMatchTheSeriesOnTheRegistration = "Assessment series does not match the series on the registration";
        public const string SpecialismGradeIsNotValid = "Specialism grade not valid";
        public const string SpecialismSeriesNotCurrentlyOpen = "Incorrect Assessment series";
        public const string InvalidSpecialismRommComponentGrade = "Enter a valid grade for the specialism component. The grade must be Distinction, Merit, Pass, Unclassified.";

        // Results - Bulk stage 4 validations
        public const string ResultCannotBeChanged = "This learner's grade cannot be changed. Please remove this learner and try again.";
        public const string ResultCannotBeInUnderReviewOrBeingAppealedStatus = "This learner's grade cannot be changed because it is being reviewed or appealed. Please remove this row and try again.";
        public const string ResultIsInFinal = "This learner's grade is now final. Please remove this learner and try again.";

        // Industry Placement - Bulk Stage 2 validations
        public const string IpBulkUlnRequired = "Enter ULN";
        public const string IpBulkCorecodeRequired = "Enter core code";
        public const string IpBulkCorecodeMustBe8Chars = "Core code must be 8 characters";
        public const string IpBulkStatusMustBeValid = "Industry placement status not recognised";
        public const string IpBulkHoursRequired = "Industry placement hours must be provided if industry placement status is Completed with special consideration";
        public const string IpBulkHoursMustBeEmpty = "Industry placement hours must be empty unless industry placement status is Completed with special consideration";
        public const string IpBulkHoursMustOutOfRange = "The placement duration must be a whole number between 1 and 999 hours";
        public const string IpBulkReasonMustBeEmpty = "Industry placement reasons must be empty unless industry placement status is Completed with special consideration";
        public const string IpBulkReasonRequired = "At least one industry placement reason must be provided if industry placement status is Completed with special consideration";
        public const string IpBulkReasonMustBeValid = "Industry placement reasons must be one or more of 'ML', 'MF', 'B', 'T', 'D', 'A', 'U', 'W', 'C'";
        public const string IpBulkReasonDuplicated = "Each industry placement reason can only be included once per learner";

        public const string NoIndustryPlacementDataAfterUln = "No data provided for this learner. Please provide data or remove the row.";

        // Industry Placement - Bulk stage 3 validations
        public const string IpBulkUlnNotRegistered = "The ULN must match a learner in your account";
        public const string IpBulkCorecodeInvalid = "The core code does not match the existing core code for this learner";
        public const string BulkIpDuplicateRecord = "Duplicate ULNs are not allowed";
    }
}