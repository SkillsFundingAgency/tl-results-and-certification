using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class AssessmentRepository : GenericRepository<TqPathwayAssessment>, IAssessmentRepository
    {
        private ILogger<AssessmentRepository> _logger;

        public AssessmentRepository(ILogger<AssessmentRepository> logger, ResultsAndCertificationDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
        }

        public async Task<IList<TqPathwayAssessment>> GetPathwayAssessmentsAsync(IList<TqPathwayAssessment> pathwayAssessments)
        {
            var registrationPathwayIds = new HashSet<int>();
            pathwayAssessments.ToList().ForEach(r => registrationPathwayIds.Add(r.TqRegistrationPathwayId));
            return await _dbContext.TqPathwayAssessment.Where(x => registrationPathwayIds.Contains(x.TqRegistrationPathwayId) && x.EndDate == null && x.IsOptedin).ToListAsync();
        }

        public async Task<IList<TqSpecialismAssessment>> GetSpecialismAssessmentsAsync(IList<TqSpecialismAssessment> specialismAssessments)
        {
            var registrationSpecialismIds = new HashSet<int>();
            specialismAssessments.ToList().ForEach(r => registrationSpecialismIds.Add(r.TqRegistrationSpecialismId));
            return await _dbContext.TqSpecialismAssessment.Where(x => registrationSpecialismIds.Contains(x.TqRegistrationSpecialismId) && x.EndDate == null && x.IsOptedin).ToListAsync();
        }

        public async Task<bool> BulkInsertOrUpdateAssessments(List<TqPathwayAssessment> pathwayAssessments, List<TqSpecialismAssessment> specialismAssessments)
        {
            var result = true;
            if ((pathwayAssessments != null && pathwayAssessments.Count > 0) || (specialismAssessments != null && specialismAssessments.Count > 0))
            {
                var strategy = _dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var bulkConfig = new BulkConfig() { UseTempDB = true, BatchSize = 5000, BulkCopyTimeout = 60 };

                            await ProcessPathwayAssessments(bulkConfig, pathwayAssessments);

                            await ProcessSpecialismAssessments(bulkConfig, specialismAssessments);

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

        private async Task ProcessPathwayAssessments(BulkConfig bulkConfig, List<TqPathwayAssessment> pathwayAssessments)
        {
            if (pathwayAssessments.Count > 0)
            {
                pathwayAssessments = SortUpdateAndInsertOrder(pathwayAssessments, x => x.Id);
                await _dbContext.BulkInsertOrUpdateAsync(pathwayAssessments, bulkConfig);
            }
        }

        private async Task ProcessSpecialismAssessments(BulkConfig bulkConfig, List<TqSpecialismAssessment> specialismAssessments)
        {
            if (specialismAssessments.Count > 0)
            {
                specialismAssessments = SortUpdateAndInsertOrder(specialismAssessments, x => x.Id);
                await _dbContext.BulkInsertOrUpdateAsync(specialismAssessments, bulkConfig);
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
