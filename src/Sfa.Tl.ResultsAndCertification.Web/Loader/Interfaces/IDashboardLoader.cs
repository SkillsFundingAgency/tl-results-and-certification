using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Dashboard;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IDashboardLoader
    {
        Task<DashboardViewModel> GetDashboardViewModel(ClaimsPrincipal claimsPrincipal);
    }
}