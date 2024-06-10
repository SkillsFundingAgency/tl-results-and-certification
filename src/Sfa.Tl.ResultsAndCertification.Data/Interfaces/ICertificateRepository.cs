using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Certificates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface ICertificateRepository
    {
        Task<CertificateDataResponse> SaveCertificatesPrintingDataAsync(Batch batch, List<OverallResult> overallResults);

        Task<IList<PrintCertificate>> GetCertificateTrackingDataAsync(Func<DateTime> getFromDay);
    }
}
