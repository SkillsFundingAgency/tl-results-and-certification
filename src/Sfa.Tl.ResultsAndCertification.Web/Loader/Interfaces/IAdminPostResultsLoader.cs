using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminPostResultsLoader
    {
        Task<AdminOpenPathwayRommViewModel> GetAdminOpenRommAsync(int registrationPathwayId, int assessmentId);
    }
}