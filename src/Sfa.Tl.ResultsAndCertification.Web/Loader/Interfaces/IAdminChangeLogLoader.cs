using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminChangeLog;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminChangeLogLoader
    {
        Task<AdminSearchChangeLogViewModel> SearchChangeLogsAsync(string searchKey, int? pageNumber);
    }
}