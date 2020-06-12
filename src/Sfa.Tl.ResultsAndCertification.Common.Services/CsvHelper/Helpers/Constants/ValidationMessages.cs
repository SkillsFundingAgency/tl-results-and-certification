namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Constants
{
    public class ValidationMessages
    {
        // Todo: can this move to resource file? assess. 
        // Property validation messages
        public const string Required = "{0} is required.";
        public const string MustBeNumberWithLength = "{0} must be a number with {1} digits.";
        public const string StringLength = "{0} cannot be greater than {1} characters.";
        public const string MustBeValidDate = "{0} should be a valid date in DDMMYYYY format.";
        public const string DateNotinFuture = "{0} should be not be a future date.";


        // File based validation messages
        public const string UnAuthorizedFileAccess = "File unauthorized to read.";
        public const string FileHeaderNotFound = "File header is not valid.";
        
        // Generic or unexpected behaviour messages
        public const string UnableToParse = "Unable to parse the row.";
        public const string UnableToReadCsvData = "Unable to interpret content.";
        public const string UnexpectedError = "Unexpected error while reading file content.";

        public const string NoRecordsFound = "There are no registrations found."; // Todo: specific
    }
}
