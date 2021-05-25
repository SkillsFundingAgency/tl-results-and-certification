using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class StatementOfAchievementRepository : IStatementOfAchievementRepository
    {
        protected readonly ResultsAndCertificationDbContext _dbContext;
        private readonly ILogger<StatementOfAchievementRepository> _logger;

        public StatementOfAchievementRepository(ResultsAndCertificationDbContext dbContext, ILogger<StatementOfAchievementRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<FindSoaLearnerRecord> FindSoaLearnerRecordAsync(long providerUkprn, long uln)
        {
            var soaLearnerRecord = await (from tqPathway in _dbContext.TqRegistrationPathway
                                          join tqProfile in _dbContext.TqRegistrationProfile on tqPathway.TqRegistrationProfileId equals tqProfile.Id
                                          join tqProvider in _dbContext.TqProvider on tqPathway.TqProviderId equals tqProvider.Id
                                          join tlProvider in _dbContext.TlProvider on tqProvider.TlProviderId equals tlProvider.Id
                                          join tqAo in _dbContext.TqAwardingOrganisation on tqProvider.TqAwardingOrganisationId equals tqAo.Id
                                          join tlPathway in _dbContext.TlPathway on tqAo.TlPathwayId equals tlPathway.Id
                                          orderby tqPathway.CreatedOn descending
                                          let industryPlacements = _dbContext.IndustryPlacement.Where(p => p.TqRegistrationPathwayId == tqPathway.Id)
                                          let pathwayResults = _dbContext.TqPathwayAssessment.Join(_dbContext.TqPathwayResult, pa => pa.Id, pr => pr.TqPathwayAssessmentId, (pa, pr) => new { pa, pr }).Where(x => x.pa.TqRegistrationPathwayId == tqPathway.Id && x.pa.IsOptedin && x.pr.IsOptedin)
                                          where tqProfile.UniqueLearnerNumber == uln && tlProvider.UkPrn == providerUkprn
                                          select new FindSoaLearnerRecord
                                          {
                                              ProfileId = tqProfile.Id,
                                              Uln = tqProfile.UniqueLearnerNumber,
                                              LearnerName = tqProfile.Firstname + " " + tqProfile.Lastname,
                                              DateofBirth = tqProfile.DateofBirth,
                                              ProviderName = tlProvider.Name + " (" + tlProvider.UkPrn + ")",
                                              TlevelTitle = tlPathway.TlevelTitle,
                                              Status = tqPathway.Status,
                                              IsIndustryPlacementAdded = industryPlacements.Any(),
                                              IndustryPlacementStatus = industryPlacements.FirstOrDefault().Status,
                                              HasPathwayResult = pathwayResults.Any()
                                          })
                                .FirstOrDefaultAsync();
            return soaLearnerRecord;
        }
    }
}