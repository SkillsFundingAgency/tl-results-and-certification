using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public interface IDownloadOverallResultsService
    {
        Task<DownloadOverallResultSlipsData> DownloadLearnerOverallResultSlipsDataAsync(long providerUkprn, long profileId);
        Task<IList<DownloadOverallResultsData>> DownloadOverallResultsDataAsync(long providerUkprn, DateTime now);
        Task<IList<DownloadOverallResultSlipsData>> DownloadOverallResultSlipsDataAsync(long providerUkprn, DateTime now);
    }
}