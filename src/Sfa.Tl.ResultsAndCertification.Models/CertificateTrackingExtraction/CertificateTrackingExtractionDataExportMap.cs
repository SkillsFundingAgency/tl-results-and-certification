using CsvHelper.Configuration;
using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Models.CertificateTrackingExtraction
{
    public class CertificateTrackingExtractionDataExportMap : ClassMap<CertificateTrackingExtractionData>
    {
        public CertificateTrackingExtractionDataExportMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}