using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAdminPostResultsService
    {
        Task<bool> ProcessAdminOpenCoreAppealAsync(OpenCoreAppealRequest request);
        Task<bool> ProcessAdminOpenPathwayRommAsync(OpenPathwayRommRequest request);
        Task<bool> ProcessAdminOpenSpecialismAppealAsync(OpenSpecialismAppealRequest request);
        Task<bool> ProcessAdminOpenSpecialismRommAsync(OpenSpecialismRommRequest request);
        Task<bool> ProcessAdminReviewChangesAppealOutcomeCoreAsync(ReviewChangesAppealOutcomeCoreRequest request);
        Task<bool> ProcessAdminReviewChangesAppealOutcomeSpecialismAsync(ReviewChangesAppealOutcomeSpecialismRequest request);
        Task<bool> ProcessAdminReviewChangesRommOutcomeCoreAsync(ReviewChangesRommOutcomeCoreRequest request);

        Task<bool> ProcessAdminReviewChangesRommOutcomeSpecialismAsync(ReviewChangesRommOutcomeSpecialismRequest request);
    }
}