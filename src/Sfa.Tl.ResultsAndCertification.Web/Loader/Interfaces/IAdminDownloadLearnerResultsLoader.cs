using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDownloadLearnerResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminDownloadLearnerResultsLoader
    {
        Task<AdminDownloadLearnerResultsByProviderViewModel> GetDownloadLearnerResultsByProviderViewModel(int providerId);
    }
}