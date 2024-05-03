using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.Interfaces
{
    public interface ICertificateTrackingExtractionService
    {
        Task<FunctionResponse> ProcessCertificateTrackingExtractAsync(Func<DateTime> getFromDay, Func<string> getExtractFileName);
    }
}