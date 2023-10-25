using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Models.JsonConverter;
using Newtonsoft.Json;
namespace Sfa.Tl.ResultsAndCertification.Models.Configuration
{
        public class ProviderAddressExtractSettings
        {
            /// <summary>
            /// Gets or sets the academic years to process.
            /// </summary>
            /// <value>
            /// The academic years to process.
            /// </value>
            public int[] ProviderAddressAcademicYearsToProcess { get; set; }

            /// <summary>
            /// Gets or sets the valid date ranges to run the process.
            /// </summary>
            /// <value>
            /// The valid date ranges to run the process.
            /// </value>
            [JsonConverter(typeof(StringToDateTimeRangeArrayJsonConverter))]
            public DateTimeRange[] ProviderAddressValidDateRanges { get; set; }
        

    }
}
