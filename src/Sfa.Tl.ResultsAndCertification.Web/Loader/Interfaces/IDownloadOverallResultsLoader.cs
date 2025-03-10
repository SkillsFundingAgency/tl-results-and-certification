﻿using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IDownloadOverallResultsLoader
    {
        Task<Stream> DownloadOverallResultsDataAsync(long providerUkprn, string performedBy);
        Task<Stream> DownloadOverallResultSlipsDataAsync(long providerUkprn, string performedBy);
        Task<Stream> DownloadLearnerOverallResultSlipsDataAsync(long providerUkprn, int profileId, string performedBy);
    }
}
