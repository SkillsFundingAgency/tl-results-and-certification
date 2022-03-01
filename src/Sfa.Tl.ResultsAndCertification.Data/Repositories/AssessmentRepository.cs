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
        private readonly ILogger<AssessmentRepository> _logger;

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
                    .Include(x => x.TqRegistrationSpecialisms)
                        .ThenInclude(x => x.TqSpecialismAssessments)
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
            var assessments = await _dbContext.TqPathwayAssessment
                .Include(x => x.TqPathwayResults)
                .Include(x => x.AssessmentSeries)
                .Where(x => registrationPathwayIds.Contains(x.TqRegistrationPathwayId) && x.EndDate == null && x.IsOptedin)
                .ToListAsync();

            return assessments
               .GroupBy(x => x.TqRegistrationPathwayId)
               .Select(x => x.OrderByDescending(o => o.AssessmentSeries.StartDate).First())
               .ToList();
        }

        public async Task<IList<TqSpecialismAssessment>> GetBulkSpecialismAssessmentsAsync(IList<TqSpecialismAssessment> specialismAssessments)
        {
            var registrationSpecialismIds = new HashSet<int>();
            specialismAssessments.ToList().ForEach(r => registrationSpecialismIds.Add(r.TqRegistrationSpecialismId));
            
            var assessments = await _dbContext.TqSpecialismAssessment
                .Include(x => x.TqSpecialismResults)
                .Include(x => x.AssessmentSeries)
                .Where(x => registrationSpecialismIds.Contains(x.TqRegistrationSpecialismId) && x.EndDate == null && x.IsOptedin)
                .ToListAsync();

            return assessments
               .GroupBy(x => x.TqRegistrationSpecialismId)
               .Select(x => x.OrderByDescending(o => o.AssessmentSeries.StartDate).First())
               .ToList();
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
                            var bulkConfig = new BulkConfig() { UseTempDB = true, SetOutputIdentity = false, PreserveInsertOrder = false, BatchSize = 5000, BulkCopyTimeout = 60 };

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
                    .Include(x => x.TqPathwayAssessments)
                       .ThenInclude(x => x.TqPathwayResults)
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
                    .Include(x => x.IndustryPlacements)
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

            foreach (var pathwayAssessment in regPathway.TqPathwayAssessments)
            {
                // TqPathwayResults
                Func<TqPathwayResult, bool> pathwayResultPredicate = e => e.IsOptedin && e.EndDate == null;
                if (regPathway.Status == RegistrationPathwayStatus.Withdrawn)
                    pathwayResultPredicate = e => e.IsOptedin && e.EndDate != null;

                pathwayAssessment.TqPathwayResults = pathwayAssessment.TqPathwayResults.Where(pathwayResultPredicate).ToList();
            }

            return regPathway;
        }

        public async Task<IList<AssessmentSeries>> GetAvailableAssessmentSeriesAsync(long aoUkprn, int profileId, int startYearOffset)
        {
            var currentDate = DateTime.Now.Date;
            
            var series = await _dbContext.TqRegistrationPathway
                .Where(rpw => rpw.Status == RegistrationPathwayStatus.Active && rpw.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn && 
                       rpw.TqRegistrationProfile.Id == profileId)
                .SelectMany(reg => _dbContext.AssessmentSeries
                        .Where(s => s.Year > reg.AcademicYear + startYearOffset && s.Year <= reg.AcademicYear + Common.Helpers.Constants.AssessmentEndInYears && 
                        currentDate >= s.StartDate && currentDate <= s.EndDate))
                .ToListAsync();

            return series;
        }

        public async Task<TqPathwayAssessment> GetPathwayAssessmentDetailsAsync(long aoUkprn, int pathwayAssessmentId)
        {
            var pathwayAssessment = await _dbContext.TqPathwayAssessment
                .Include(p => p.AssessmentSeries)
                .Include(p => p.TqRegistrationPathway)
                    .ThenInclude(P => P.TqRegistrationProfile)
                .Include(p => p.TqPathwayResults)
                .FirstOrDefaultAsync(pa => pa.Id == pathwayAssessmentId && pa.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn);

            return pathwayAssessment;
        }

        public async Task<IList<TqSpecialismAssessment>> GetSpecialismAssessmentDetailsAsync(long aoUkprn, IList<int> specialismAssessmentIds)
        {
            var specialismAssessments = await _dbContext.TqSpecialismAssessment
                                .Include(s => s.AssessmentSeries)
                                .Include(s => s.TqRegistrationSpecialism)
                                    .ThenInclude(s => s.TqRegistrationPathway)
                                        .ThenInclude(s => s.TqRegistrationProfile)
                 .Where(s => specialismAssessmentIds.Contains(s.Id) && s.TqRegistrationSpecialism.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn).ToListAsync();

            return specialismAssessments;
        }
    }
}
