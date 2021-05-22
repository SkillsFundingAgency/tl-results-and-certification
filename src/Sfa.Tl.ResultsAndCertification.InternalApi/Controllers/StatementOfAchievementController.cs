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
        public Task<FindSoaLearnerRecord> FindSoaLearnerRecordAsync(long providerUkprn, long uln)
        {
            return _statementOfAchievementService.FindSoaLearnerRecordAsync(providerUkprn, uln);
        }
    }
}
