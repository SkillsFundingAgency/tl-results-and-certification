using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPostResultsController : ControllerBase
    {
        private readonly IAdminPostResultsService _adminPostResultsService;

        public AdminPostResultsController(IAdminPostResultsService adminPostResultsService)
        {
            _adminPostResultsService = adminPostResultsService;
        }

        [HttpPost]
        [Route("ProcessAdminOpenPathwayRomm")]
        public Task<bool> ProcessAdminOpenPathwayRommAsync(OpenPathwayRommRequest request)
            => _adminPostResultsService.ProcessAdminOpenPathwayRommAsync(request);

        [HttpPost]
        [Route("ProcessAdminOpenSpecialismRomm")]
        public Task<bool> ProcessAdminOpenSpecialismRommAsync(OpenSpecialismRommRequest request)
             => _adminPostResultsService.ProcessAdminOpenSpecialismRommAsync(request);


        [HttpPost]
        [Route("ProcessAdminReviewChangesRommOutcomeCore")]
        public Task<bool> ProcessAdminReviewChangesRommOutcomeCoreAsync(ReviewChangesRommOutcomeCoreRequest request)
           => _adminPostResultsService.ProcessAdminReviewChangesRommOutcomeCoreAsync(request);

        [HttpPost]
        [Route("ProcessAdminReviewChangesRommOutcomeSpecialism")]
        public Task<bool> ProcessAdminReviewChangesRommOutcomeSpecialismAsync(ReviewChangesRommOutcomeSpecialismRequest request)
           => _adminPostResultsService.ProcessAdminReviewChangesRommOutcomeSpecialismAsync(request);


        [HttpPost]
        [Route("ProcessAdminOpenCoreAppeal")]
        public Task<bool> ProcessAdminOpenCoreAppealAsync(OpenCoreAppealRequest request)
            => _adminPostResultsService.ProcessAdminOpenCoreAppealAsync(request);

        [HttpPost]
        [Route("ProcessAdminOpenSpecialismAppeal")]
        public Task<bool> ProcessAdminOpenSpecialisAppealAsync(OpenSpecialismAppealRequest request)
            => _adminPostResultsService.ProcessAdminOpenSpecialismAppealAsync(request);
    }
}