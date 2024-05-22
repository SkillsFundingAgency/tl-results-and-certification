using CsvHelper.Configuration;
using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Models.CertificateTrackingExtraction
{
    public class CertificateTrackingExtractionDataExportMap : ClassMap<CertificateTrackingExtractionData>
    {
        public CertificateTrackingExtractionDataExportMap()
        {
            AutoMap(CultureInfo.InvariantCulture);

            Map(c => c.FirstName).Convert(c => WrapWithQuotes(c.Value.FirstName));
            Map(c => c.LastName).Convert(c => WrapWithQuotes(c.Value.LastName));
            Map(c => c.TLevelTitle).Convert(c => WrapWithQuotes(c.Value.TLevelTitle));
            Map(c => c.SignedForBy).Convert(c => WrapWithQuotes(c.Value.SignedForBy));
        }

        private static string WrapWithQuotes(string value)
            => $"\"{value}\"";
    }
}