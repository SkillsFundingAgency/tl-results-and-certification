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
        private readonly ILogger<ResultRepository> _logger;

        public ResultRepository(ILogger<ResultRepository> logger, ResultsAndCertificationDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<TqRegistrationPathway>> GetBulkResultsAsync(long aoUkprn, IEnumerable<long> uniqueLearnerNumbers)
        {
            var registrations = await _dbContext.TqRegistrationPathway
                   .Include(x => x.TqPathwayAssessments.Where(pa => pa.IsOptedin && pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? pa.EndDate != null : pa.EndDate == null))
                       .ThenInclude(x => x.AssessmentSeries)
                   .Include(x => x.TqRegistrationProfile)
                   .Include(x => x.TqProvider)
                       .ThenInclude(x => x.TqAwardingOrganisation)
                           .ThenInclude(x => x.TlPathway)
                   .Include(x => x.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin && rs.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? rs.EndDate != null : rs.EndDate == null))
                       .ThenInclude(x => x.TlSpecialism)
                   .Include(x => x.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin && rs.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? rs.EndDate != null : rs.EndDate == null))
                       .ThenInclude(x => x.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? sa.EndDate != null : sa.EndDate == null))
                        .ThenInclude(x => x.AssessmentSeries)
                       .OrderByDescending(o => o.CreatedOn)
                    .Where(p => uniqueLearnerNumbers.Contains(p.TqRegistrationProfile.UniqueLearnerNumber) &&
                          p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn &&
                          (p.Status == RegistrationPathwayStatus.Active || p.Status == RegistrationPathwayStatus.Withdrawn))
                   .ToListAsync();

            return registrations;
        }

        public async Task<IList<TqPathwayResult>> GetBulkPathwayResultsAsync(IList<TqPathwayResult> pathwayResults)
        {
            var pathwayAssessmentIds = new HashSet<int>();
            pathwayResults.ToList().ForEach(r => pathwayAssessmentIds.Add(r.TqPathwayAssessmentId));
            return await _dbContext.TqPathwayResult.Where(x => pathwayAssessmentIds.Contains(x.TqPathwayAssessmentId) && x.EndDate == null && x.IsOptedin).ToListAsync();
        }

        public async Task<IList<TqSpecialismResult>> GetBulkSpecialismResultsAsync(IList<TqSpecialismResult> specialismResults)
        {
            var specialismAssessmentIds = new HashSet<int>();
            specialismResults.ToList().ForEach(r => specialismAssessmentIds.Add(r.TqSpecialismAssessmentId));
            return await _dbContext.TqSpecialismResult.Where(x => specialismAssessmentIds.Contains(x.TqSpecialismAssessmentId) && x.EndDate == null && x.IsOptedin).ToListAsync();
        }

        public async Task<bool> BulkInsertOrUpdateResults(List<TqPathwayResult> pathwayResults, List<TqSpecialismResult> specialismResults)
        {
            var result = true;
            if ((pathwayResults != null && pathwayResults.Count > 0) || (specialismResults != null && specialismResults.Count > 0))
            {
                var strategy = _dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var bulkConfig = new BulkConfig() { UseTempDB = true, SetOutputIdentity = false, PreserveInsertOrder = false, BatchSize = 5000, BulkCopyTimeout = 60 };

                            await ProcessPathwayResults(bulkConfig, pathwayResults);

                            await ProcessSpecialismResults(bulkConfig, specialismResults);

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

        public async Task<TqRegistrationPathway> GetResultsAsync(long aoUkprn, int profileId)
        {
            var regPathway = await _dbContext.TqRegistrationPathway
                   .Include(x => x.TqPathwayAssessments.Where(pa => pa.IsOptedin && pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? pa.EndDate != null : pa.EndDate == null))
                       .ThenInclude(x => x.AssessmentSeries)
                   .Include(x => x.TqPathwayAssessments.Where(pa => pa.IsOptedin && pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? pa.EndDate != null : pa.EndDate == null))
                       .ThenInclude(x => x.TqPathwayResults.Where(pr => pr.IsOptedin && pr.TqPathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? pr.EndDate != null : pr.EndDate == null))
                       .ThenInclude(x => x.TlLookup)
                   .Include(x => x.TqRegistrationProfile)
                   .Include(x => x.TqProvider)
                       .ThenInclude(x => x.TqAwardingOrganisation)
                           .ThenInclude(x => x.TlPathway)
                   .Include(x => x.TqProvider)
                       .ThenInclude(x => x.TlProvider)
                   .Include(x => x.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin && rs.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? rs.EndDate != null : rs.EndDate == null))
                       .ThenInclude(x => x.TlSpecialism)
                   .Include(x => x.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin && rs.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? rs.EndDate != null : rs.EndDate == null))
                       .ThenInclude(x => x.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? sa.EndDate != null : sa.EndDate == null))
                            .ThenInclude(x => x.TqSpecialismResults.Where(sr => sr.IsOptedin && sr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? sr.EndDate != null : sr.EndDate == null))
                    .OrderByDescending(o => o.CreatedOn)
                    .FirstOrDefaultAsync(p => p.TqRegistrationProfile.Id == profileId &&
                            p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn &&
                            (
                                p.Status == RegistrationPathwayStatus.Active || p.Status == RegistrationPathwayStatus.Withdrawn
                            ));
            return regPathway;
        }

        public async Task<TqRegistrationPathway> GetPathwayResultAsync(long aoUkprn, int profileId, int assessmentId)
        {
            var result = await GetResultsAsync(aoUkprn, profileId);
            if (result == null)
                return null;

            var assessment = result.TqPathwayAssessments?.Where(x => x.IsOptedin && x.EndDate == null && x.Id == assessmentId);
            if (!assessment.Any())
                return null;

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

        private async Task ProcessSpecialismResults(BulkConfig bulkConfig, List<TqSpecialismResult> specialismResults)
        {
            if (specialismResults.Count > 0)
            {
                specialismResults = SortUpdateAndInsertOrder(specialismResults, x => x.Id);
                await _dbContext.BulkInsertOrUpdateAsync(specialismResults, bulkConfig);
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
