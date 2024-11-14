using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IResultSlipsGeneratorService
    {
        byte[] GetByteData(IEnumerable<DownloadOverallResultSlipsData> data);
    }
}