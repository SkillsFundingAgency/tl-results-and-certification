using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatementOfAchievementController : ControllerBase, IStatementOfAchievementController
    {
        protected IStatementOfAchievementService _statementOfAchievementService;

        public StatementOfAchievementController(IStatementOfAchievementService statementOfAchievementService)
        {
            _statementOfAchievementService = statementOfAchievementService;
        }

        [HttpGet]
        [Route("FindSoaLearnerRecord/{providerUkprn}/{uln}")]
        public async Task<FindSoaLearnerRecord> FindSoaLearnerRecordAsync(long providerUkprn, long uln)
        {
            return await _statementOfAchievementService.FindSoaLearnerRecordAsync(providerUkprn, uln);
        }

        [HttpGet]
        [Route("GetSoaLearnerRecordDetails/{providerUkprn}/{profileId}")]
        public async Task<SoaLearnerRecordDetails> GetSoaLearnerRecordDetailsAsync(long providerUkprn, int profileId)
        {
            return await _statementOfAchievementService.GetSoaLearnerRecordDetailsAsync(providerUkprn, profileId);
        }

        [HttpPost]
        [Route("CreateSoaPrintingRequest")]
        public async Task<SoaPrintingResponse> CreateSoaPrintingRequestAsync(SoaPrintingRequest request)
        {
            return await _statementOfAchievementService.CreateSoaPrintingRequestAsync(request);
        }

        [HttpGet]
        [Route("GetPrintRequestSnapshot/{providerUkprn}/{profileId}/{pathwayId}")]
        public async Task<PrintRequestSnapshot> GetPrintRequestSnapshotAsync(long providerUkprn, int profileId, int pathwayId)
        {
            return await _statementOfAchievementService.GetPrintRequestSnapshotAsync(providerUkprn, profileId, pathwayId);
        }
    }
}