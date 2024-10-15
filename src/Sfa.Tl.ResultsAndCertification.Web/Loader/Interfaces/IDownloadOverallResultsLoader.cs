using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IDownloadOverallResultsLoader
    {
        Task<Stream> DownloadOverallResultsDataAsync(long providerUkprn, string performedBy);
        Task<Stream> DownloadOverallResultSlipsAsync(long providerUkprn);
        Task<Stream> DownloadOverallResultSlipsDataAsync(long providerUkprn, string performedBy);
    }
}
