using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IResultLoader
    {
        Task<UploadResultsResponseViewModel> ProcessBulkResultsAsync(UploadResultsRequestViewModel viewModel);
    }
}
