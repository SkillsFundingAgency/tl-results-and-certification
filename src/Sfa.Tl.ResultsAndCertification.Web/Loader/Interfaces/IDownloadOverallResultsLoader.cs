using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IDownloadOverallResultsLoader
    {
        Task<Stream> GetDownloadOverallResultsDataAsync(long providerUkprn, string performedBy);
    }
}
