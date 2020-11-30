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
    public class AssessmentRepository : GenericRepository<TqPathwayAssessment>, IAssessmentRepository
    {
        private ILogger<AssessmentRepository> _logger;

        public AssessmentRepository(ILogger<AssessmentRepository> logger, ResultsAndCertificationDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
        }

        #region Bulk Assessments

        public async Task<IEnumerable<TqRegistrationPathway>> GetBulkAssessmentsAsync(long aoUkprn, IEnumerable<long> uniqueLearnerNumbers)
        {
            var registrations = await _dbContext.TqRegistrationPathway
                   .Include(x => x.TqRegistrationProfile)
                   .Include(x => x.TqProvider)
                       .ThenInclude(x => x.TqAwardingOrganisation)
                           .ThenInclude(x => x.TlPathway)
                    .Include(x => x.TqRegistrationSpecialisms)
                       .ThenInclude(x => x.TlSpecialism)
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
                Func<TqRegistrationSpecialism, bool> specialismPredicate = e => e.IsOptedin && e.EndDate == null;
                if (reg.Status == RegistrationPathwayStatus.Withdrawn)
                    specialismPredicate = e => e.IsOptedin && e.EndDate != null;
                reg.TqRegistrationSpecialisms = reg.TqRegistrationSpecialisms.Where(specialismPredicate).ToList();
            }

            return latestRegistratons;
        }

        public async Task<IList<TqPathwayAssessment>> GetBulkPathwayAssessmentsAsync(IList<TqPathwayAssessment> pathwayAssessments)
        {
            var registrationPathwayIds = new HashSet<int>();
            pathwayAssessments.ToList().ForEach(r => registrationPathwayIds.Add(r.TqRegistrationPathwayId));
            return await _dbContext.TqPathwayAssessment.Where(x => registrationPathwayIds.Contains(x.TqRegistrationPathwayId) && x.EndDate == null && x.IsOptedin).ToListAsync();
        }

        public async Task<IList<TqSpecialismAssessment>> GetBulkSpecialismAssessmentsAsync(IList<TqSpecialismAssessment> specialismAssessments)
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

        #endregion

        public async Task<TqRegistrationPathway> GetAssessmentsAsync(long aoUkprn, int profileId)
        {
            var regPathway = await _dbContext.TqRegistrationPathway
                   .Include(x => x.TqPathwayAssessments)
                       .ThenInclude(x => x.AssessmentSeries)
                   .Include(x => x.TqRegistrationProfile)
                   .Include(x => x.TqProvider)
                       .ThenInclude(x => x.TqAwardingOrganisation)
                           .ThenInclude(x => x.TlPathway)
                   .Include(x => x.TqProvider)
                       .ThenInclude(x => x.TlProvider)
                   .Include(x => x.TqRegistrationSpecialisms)
                       .ThenInclude(x => x.TlSpecialism)
                   .Include(x => x.TqRegistrationSpecialisms)
                       .ThenInclude(x => x.TqSpecialismAssessments)
                            .ThenInclude(x => x.AssessmentSeries)
                    .OrderByDescending(o => o.CreatedOn)
                    .FirstOrDefaultAsync(p => p.TqRegistrationProfile.Id == profileId &&
                           p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn &&
                           (
                                p.Status == RegistrationPathwayStatus.Active || p.Status == RegistrationPathwayStatus.Withdrawn
                           ));


            if (regPathway == null) return null;

            Func<TqPathwayAssessment, bool> pathwayAssessmentPredicate = e => e.IsOptedin && e.EndDate == null;
            if (regPathway.Status == RegistrationPathwayStatus.Withdrawn)
                pathwayAssessmentPredicate = e => e.IsOptedin && e.EndDate != null;
            regPathway.TqPathwayAssessments = regPathway.TqPathwayAssessments.Where(pathwayAssessmentPredicate).ToList();

            // PathwaySpecialism
            Func<TqRegistrationSpecialism, bool> specialismPredicate = e => e.IsOptedin && e.EndDate == null;
            if (regPathway.Status == RegistrationPathwayStatus.Withdrawn)
                specialismPredicate = e => e.IsOptedin && e.EndDate != null;
            regPathway.TqRegistrationSpecialisms = regPathway.TqRegistrationSpecialisms.Where(specialismPredicate).ToList();

            foreach (var specialism in regPathway.TqRegistrationSpecialisms)
            {
                // SpecialismAssessment
                Func<TqSpecialismAssessment, bool> specialismAssessmentPredicate = e => e.IsOptedin && e.EndDate == null;
                if (regPathway.Status == RegistrationPathwayStatus.Withdrawn)
                    specialismAssessmentPredicate = e => e.IsOptedin && e.EndDate != null;

                specialism.TqSpecialismAssessments = specialism.TqSpecialismAssessments.Where(specialismAssessmentPredicate).ToList();
            }

            return regPathway;
        }

        public async Task<AssessmentSeries> GetAvailableAssessmentSeriesAsync(long aoUkprn, int profileId, int startInYear)
        {
            var currentDate = DateTime.Now.Date;
            
            var series = await _dbContext.TqRegistrationPathway
                .Where(rpw => rpw.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn && 
                       rpw.TqRegistrationProfile.Id == profileId)
                .Select(reg => _dbContext.AssessmentSeries
                        .FirstOrDefault(s => s.Year >= reg.AcademicYear + startInYear && s.Year <= reg.AcademicYear + Common.Helpers.Constants.AssessmentEndInYears && 
                        currentDate >= s.StartDate && currentDate <= s.EndDate))
                .FirstOrDefaultAsync();

            return series;
        }
    }
}
