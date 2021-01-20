using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class ResultRepository : GenericRepository<TqPathwayAssessment>, IResultRepository
    {
        private ILogger<ResultRepository> _logger;

        public ResultRepository(ILogger<ResultRepository> logger, ResultsAndCertificationDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<TqRegistrationPathway>> GetBulkResultsAsync(long aoUkprn, IEnumerable<long> uniqueLearnerNumbers)
        {
            var registrations = await _dbContext.TqRegistrationPathway
                   .Include(x => x.TqPathwayAssessments)
                       .ThenInclude(x => x.AssessmentSeries)                   
                   .Include(x => x.TqRegistrationProfile)
                   .Include(x => x.TqProvider)
                       .ThenInclude(x => x.TqAwardingOrganisation)
                           .ThenInclude(x => x.TlPathway)                  
                   .Where(p => uniqueLearnerNumbers.Contains(p.TqRegistrationProfile.UniqueLearnerNumber) &&
                          p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn &&
                          (p.Status == RegistrationPathwayStatus.Active || p.Status == RegistrationPathwayStatus.Withdrawn))
                   .ToListAsync();

            if (registrations == null) return null;

            var latestRegistratons = registrations
                    .GroupBy(x => x.TqRegistrationProfileId)
                    .Select(x => x.OrderByDescending(o => o.CreatedOn).First())
                    .ToList();

            foreach (var reg in latestRegistratons)
            {
                Func<TqPathwayAssessment, bool> pathwayAssessmentPredicate = e => e.IsOptedin && e.EndDate == null;
                if (reg.Status == RegistrationPathwayStatus.Withdrawn)
                    pathwayAssessmentPredicate = e => e.IsOptedin && e.EndDate != null;
                reg.TqPathwayAssessments = reg.TqPathwayAssessments.Where(pathwayAssessmentPredicate).ToList();                
            }

            return latestRegistratons;
        }

        public async Task<IList<TqPathwayResult>> GetBulkPathwayResultsAsync(IList<TqPathwayResult> pathwayResults)
        {
            var pathwayAssessmentIds = new HashSet<int>();
            pathwayResults.ToList().ForEach(r => pathwayAssessmentIds.Add(r.TqPathwayAssessmentId));
            return await _dbContext.TqPathwayResult.Where(x => pathwayAssessmentIds.Contains(x.TqPathwayAssessmentId) && x.EndDate == null && x.IsOptedin).ToListAsync();
        }

        public async Task<bool> BulkInsertOrUpdateResults(List<TqPathwayResult> pathwayResults)
        {
            var result = true;
            if ((pathwayResults != null && pathwayResults.Count > 0))
            {
                var strategy = _dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var bulkConfig = new BulkConfig() { UseTempDB = true, BatchSize = 5000, BulkCopyTimeout = 60 };

                            await ProcessPathwayResults(bulkConfig, pathwayResults);

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message, ex.InnerException);
                            transaction.Rollback();
                            result = false;
                        }
                    }
                });
            }
            return result;
        }

        private async Task ProcessPathwayResults(BulkConfig bulkConfig, List<TqPathwayResult> pathwayResults)
        {
            if (pathwayResults.Count > 0)
            {
                pathwayResults = SortUpdateAndInsertOrder(pathwayResults, x => x.Id);
                await _dbContext.BulkInsertOrUpdateAsync(pathwayResults, bulkConfig);
            }
        }
        private List<T> SortUpdateAndInsertOrder<T>(List<T> entities, Func<T, int> selector) where T : class
        {
            // It is important as we are doing BulkInsertOrUpdate in one go, we would like to have update
            // records at the top and newley added records at the bootom of the list, so that SetOutputIdentiy
            // will work as expected. If you change the order of the entities then Id values will be interchanged. 
            // please do not make any changes to below code

            var returnResult = new List<T>();

            if (entities != null && selector != null)
            {
                returnResult.AddRange(entities.Where(x => selector(x) > 0).OrderBy(x => selector)); // listToUpdate
                returnResult.AddRange(entities.Where(x => selector(x) <= 0).OrderBy(x => selector)); // listToAdd
            }
            return returnResult;
        }

    }
}
