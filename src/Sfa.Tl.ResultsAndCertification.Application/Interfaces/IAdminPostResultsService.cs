using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAdminPostResultsService
    {
        Task<bool> ProcessAdminOpenPathwayRommAsync(OpenPathwayRommRequest request);

        Task<bool> ProcessAdminOpenSpecialismRommAsync(OpenSpecialismRommRequest request);
    }
}