using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using System;


namespace Sfa.Tl.ResultsAndCertification.Models.Configuration
{
    public class AnalystOverallResultExtractSettings
    {
        /// <summary>
        /// Gets or sets the academic years to process.
        /// </summary>
        /// <value>
        /// The academic years to process.
        /// </value>
        public int[] AcademicYearsToProcess { get; set; }

        /// <summary>
        /// Gets or sets the valid date ranges to run the process.
        /// </summary>
        /// <value>
        /// The valid date ranges to run the process.
        /// </value>
        public DateTimeRange[] ValidDateRanges { get; set; }
    }
}