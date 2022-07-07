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
    public class RegistrationRepository : GenericRepository<TqRegistrationProfile>, IRegistrationRepository
    {
        private readonly ILogger<RegistrationRepository> _logger;

        public RegistrationRepository(ILogger<RegistrationRepository> logger, ResultsAndCertificationDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
        }

        public async Task<TqRegistrationPathway> GetRegistrationLiteAsync(long aoUkprn, int profileId, bool includeProfile = true, bool includeIndustryPlacements = false, bool includeOverallResults = false)
        {
            var pathwayQueryable = _dbContext.TqRegistrationPathway.AsQueryable();

            if (includeProfile)
                pathwayQueryable = pathwayQueryable.Include(p => p.TqRegistrationProfile);

            if (includeIndustryPlacements)
                pathwayQueryable = pathwayQueryable.Include(p => p.IndustryPlacements);
            
            if (includeOverallResults)
                pathwayQueryable = pathwayQueryable.Include(p => p.OverallResults.Where(x => x.EndDate == null));

            var registrationPathway = await pathwayQueryable
               .Include(p => p.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin && (rs.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn) ? rs.EndDate != null : rs.EndDate == null))
                    .ThenInclude(x => x.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? sa.EndDate != null : sa.EndDate == null))
                        .ThenInclude(x => x.TqSpecialismResults.Where(sr => sr.IsOptedin && sr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? sr.EndDate != null : sr.EndDate == null))
               .Include(p => p.TqPathwayAssessments.Where(pa => pa.IsOptedin && (pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn) ? pa.EndDate != null : pa.EndDate == null))
                    .ThenInclude(pa => pa.TqPathwayResults.Where(pr => pr.IsOptedin && pr.TqPathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? pr.EndDate != null : pr.EndDate == null))
               .OrderByDescending(p => p.CreatedOn)
               .FirstOrDefaultAsync(p => p.TqRegistrationProfile.Id == profileId && p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn);

            return registrationPathway;            
        }

        public async Task<TqRegistrationProfile> GetRegistrationDataWithHistoryAsync(long aoUkprn, int profileId)
        {
            var profile = await _dbContext.TqRegistrationProfile
                .Where(x => x.Id == profileId && x.TqRegistrationPathways.Any(pw => pw.Status == RegistrationPathwayStatus.Active && pw.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn))
                .Include(x => x.QualificationAchieved)
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.TqRegistrationSpecialisms)
                    .ThenInclude(x => x.TqSpecialismAssessments)
                    .ThenInclude(x => x.TqSpecialismResults)
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.IndustryPlacements)
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.TqPathwayAssessments)
                    .ThenInclude(x => x.TqPathwayResults)                
                .FirstOrDefaultAsync();
            return profile;
        }

        public async Task<TqRegistrationPathway> GetRegistrationAsync(long aoUkprn, int profileId)
        {
            var regPathway = await _dbContext.TqRegistrationPathway
                .Include(x => x.TqRegistrationProfile)
                .Include(x => x.TqProvider)
                    .ThenInclude(x => x.TqAwardingOrganisation)
                        .ThenInclude(x => x.TlAwardingOrganisaton)    
                .Include(x => x.TqProvider)
                    .ThenInclude(x => x.TqAwardingOrganisation)
                        .ThenInclude(x => x.TlPathway)
                .Include(x => x.TqProvider)
                    .ThenInclude(x => x.TlProvider)
                .Include(x => x.TqRegistrationSpecialisms)
                    .ThenInclude(x => x.TlSpecialism)
                .OrderByDescending(o => o.CreatedOn)
                .FirstOrDefaultAsync(p => p.TqRegistrationProfile.Id == profileId &&
                       p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn &&
                       (
                            p.Status == RegistrationPathwayStatus.Active || p.Status == RegistrationPathwayStatus.Withdrawn
                       ));

            if (regPathway == null) return null;

            Func<TqRegistrationSpecialism, bool> predicate = e => e.IsOptedin && e.EndDate == null;
            if (regPathway.Status == RegistrationPathwayStatus.Withdrawn)
                predicate = e => e.IsOptedin && e.EndDate != null;
            
            regPathway.TqRegistrationSpecialisms = regPathway.TqRegistrationSpecialisms.Where(predicate).ToList();
            return regPathway;
        }

        public async Task<IList<TqRegistrationProfile>> GetRegistrationProfilesByIdsAsync(HashSet<int> profileIds, bool includeQualificationAchieved = false)
        {
            var profileQueryable = _dbContext.TqRegistrationProfile.Where(x => profileIds.Contains(x.Id)).AsQueryable();

            if (includeQualificationAchieved)
                profileQueryable = profileQueryable.Include(p => p.QualificationAchieved);

            return await profileQueryable.ToListAsync();
        }

        #region Bulk Registration

        public async Task<IList<TqRegistrationProfile>> GetRegistrationProfilesAsync(IList<TqRegistrationProfile> registrations)
        {
            var ulns = new HashSet<long>();
            registrations.ToList().ForEach(r => ulns.Add(r.UniqueLearnerNumber));

            return await _dbContext.TqRegistrationProfile.Where(x => ulns.Contains(x.UniqueLearnerNumber))
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.TqRegistrationSpecialisms)
                        .ThenInclude(x => x.TqSpecialismAssessments)
                            .ThenInclude(x => x.TqSpecialismResults)
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.TqPathwayAssessments)
                        .ThenInclude(x => x.TqPathwayResults)
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.TqProvider)
                        .ThenInclude(x => x.TqAwardingOrganisation)
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.IndustryPlacements)
                .ToListAsync();
        }

        public async Task<bool> BulkInsertOrUpdateTqRegistrations(List<TqRegistrationProfile> profileEntities, List<TqRegistrationPathway> pathwayEntities, List<TqRegistrationSpecialism> specialismEntities)
        {
            var result = true;
            if ((profileEntities != null && profileEntities.Count > 0) || (pathwayEntities != null && pathwayEntities.Count > 0) || (specialismEntities != null && specialismEntities.Count > 0))
            {
                var strategy = _dbContext.Database.CreateExecutionStrategy();
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                         {
                            var bulkConfig = new BulkConfig() 
                            {
                                UseTempDB = true,
                                SetOutputIdentity = true,
                                PreserveInsertOrder = false,
                                OnSaveChangesSetFK = false,
                                BatchSize = 5000,
                                BulkCopyTimeout = 60
                            };

                            var pathwayRegistrations = pathwayEntities ?? new List<TqRegistrationPathway>();

                            pathwayRegistrations = await ProcessProfileEntities(bulkConfig, profileEntities, pathwayRegistrations);

                            var specialismRegistrations = specialismEntities ?? new List<TqRegistrationSpecialism>();

                            var industryPlacements = new List<IndustryPlacement>();
                            var pathwayAssessments = new List<TqPathwayAssessment>();
                            var processEntitiesResult = await ProcessRegistrationPathwayEntities(bulkConfig, pathwayRegistrations, specialismRegistrations, pathwayAssessments, industryPlacements);

                            specialismRegistrations = processEntitiesResult.Item1;
                            pathwayAssessments = processEntitiesResult.Item2;
                            industryPlacements = processEntitiesResult.Item3;

                            await ProcessIndustryPlacements(industryPlacements);

                            var pathwayResults = new List <TqPathwayResult>();
                            await ProcessPathwayAssessments(bulkConfig, pathwayAssessments, pathwayResults);
                            await ProcessPathwayResults(pathwayResults);

                            // Specialisms
                            var specialismResults = new List<TqSpecialismResult>();
                            var specialismAssessments = await ProcessRegistrationSpecialismEntities(bulkConfig, specialismRegistrations);
                            await ProcessSpecialismAssessments(bulkConfig, specialismAssessments, specialismResults);
                            await ProcessSpecialismResults(specialismResults);

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

        private async Task<List<TqRegistrationPathway>> ProcessProfileEntities(BulkConfig bulkConfig, List<TqRegistrationProfile> profileEntities, List<TqRegistrationPathway> pathwayRegistrations)
        {
            if (profileEntities != null && profileEntities.Count > 0)
            {
                var orignialProfileEntititesCopy = new List<TqRegistrationProfile>(profileEntities);

                profileEntities = SortUpdateAndInsertOrder(profileEntities, x => x.Id);
                await _dbContext.BulkInsertOrUpdateAsync(profileEntities, bulkConfig);

                orignialProfileEntititesCopy.ForEach(profile =>
                {
                    TqRegistrationProfile profileEntity = null;

                    if (profile.Id <= 0)
                        profileEntity = profileEntities.FirstOrDefault(x => x.UniqueLearnerNumber == profile.UniqueLearnerNumber);
                    
                    foreach (var pathway in profile.TqRegistrationPathways)
                    {
                        // update fk relationship id for newly added records
                        if (pathway.TqRegistrationProfileId == 0)
                            pathway.TqRegistrationProfileId = profileEntity.Id;
                        
                        pathwayRegistrations.Add(pathway);
                    }
                });
            }
            return pathwayRegistrations;
        }

        private async Task<Tuple<List<TqRegistrationSpecialism>, List<TqPathwayAssessment>, List<IndustryPlacement>>> ProcessRegistrationPathwayEntities(BulkConfig bulkConfig, List<TqRegistrationPathway> pathwayRegistrations, List<TqRegistrationSpecialism> specialismRegistrations, List<TqPathwayAssessment> pathwayAssessments, List<IndustryPlacement> industryPlacements)
        {
            if (pathwayRegistrations.Count > 0)
            {
                var orignialPathwayEntititesCopy = new List<TqRegistrationPathway>(pathwayRegistrations);
                pathwayRegistrations = SortUpdateAndInsertOrder(pathwayRegistrations, x => x.Id);
                await _dbContext.BulkInsertOrUpdateAsync(pathwayRegistrations, bulkConfig);

                orignialPathwayEntititesCopy.ForEach(pathway =>
                {
                    TqRegistrationPathway pathwayEntity = null;
                    if (pathway.Id <= 0)
                    {
                        pathwayEntity = pathwayRegistrations.FirstOrDefault(p => p.TqRegistrationProfileId == pathway.TqRegistrationProfileId
                                                                                && p.TqProviderId == pathway.TqProviderId
                                                                                && p.Status == RegistrationPathwayStatus.Active);
                    }

                    foreach (var industryPlacement in pathway.IndustryPlacements)
                    {
                        // update fk relationship id for newly added records
                        // take only newly added industry placement record as we are not maintaining history in IP table
                        // so we don't want to update existing records
                        if (industryPlacement.TqRegistrationPathwayId == 0)
                        {
                            industryPlacement.TqRegistrationPathwayId = pathwayEntity.Id;
                            industryPlacements.Add(industryPlacement);
                        }
                    }

                    foreach (var specialism in pathway.TqRegistrationSpecialisms)
                    {
                        // update fk relationship id for newly added records
                        if (specialism.TqRegistrationPathwayId == 0)
                        {
                            specialism.TqRegistrationPathwayId = pathwayEntity.Id;
                        }
                        specialismRegistrations.Add(specialism);
                    }

                    foreach (var assessment in pathway.TqPathwayAssessments)
                    {
                        if (assessment.TqRegistrationPathwayId == 0)
                        {
                            assessment.TqRegistrationPathwayId = pathwayEntity.Id;
                        }

                        pathwayAssessments.Add(assessment);
                    }
                });
            }

            return new Tuple<List<TqRegistrationSpecialism>, List<TqPathwayAssessment>, List<IndustryPlacement>>(specialismRegistrations, pathwayAssessments, industryPlacements);
        }

        private async Task<List<TqSpecialismAssessment>> ProcessRegistrationSpecialismEntities(BulkConfig bulkConfig, List<TqRegistrationSpecialism> specialismRegistrations)
        {
            if (specialismRegistrations.Count <= 0)
                return null;

            var originalSpecialismRegistrationsCopy = new List<TqRegistrationSpecialism>(specialismRegistrations);

            specialismRegistrations = SortUpdateAndInsertOrder(specialismRegistrations, x => x.Id);
            await _dbContext.BulkInsertOrUpdateAsync(specialismRegistrations, bulkConfig);

            // Add foreignkey ref(i.e. SpecialismId available after save from above) to the SpecialismAssessment enitty.
            var specialismAssessments = new List<TqSpecialismAssessment>();
            originalSpecialismRegistrationsCopy.ForEach(specialism =>
            {
                TqRegistrationSpecialism specialismEntity = null;
                if (specialism.Id <= 0)
                    specialismEntity = specialismRegistrations.FirstOrDefault(s => s.TqRegistrationPathwayId == specialism.TqRegistrationPathwayId && s.TlSpecialismId == specialism.TlSpecialismId);

                foreach (var splAssessment in specialism.TqSpecialismAssessments)
                {
                    if (splAssessment.TqRegistrationSpecialismId == 0)
                        splAssessment.TqRegistrationSpecialismId = specialismEntity.Id;

                    specialismAssessments.Add(splAssessment);
                }
            });

            return specialismAssessments;
        }

        private async Task ProcessPathwayAssessments(BulkConfig bulkConfig, List<TqPathwayAssessment> pathwayAssessments, List<TqPathwayResult> pathwayResults)
        {
            if (pathwayAssessments == null || !pathwayAssessments.Any())
                return;

            // Dev note: below Copy obj has navigation prop's but no newly added Id's
            var pathwayAssessmentsCopy = new List<TqPathwayAssessment>(pathwayAssessments);

            pathwayAssessments = SortUpdateAndInsertOrder(pathwayAssessments, x => x.Id);
            
            await _dbContext.BulkInsertOrUpdateAsync(pathwayAssessments, bulkConfig);

            foreach (var assessmentCopy in pathwayAssessmentsCopy)
            {
                // Dev note: below pathwayAssessments obj has no navigation prop's but has newly added Id's
                var assessment = pathwayAssessments.FirstOrDefault(x => x.TqRegistrationPathwayId == assessmentCopy.TqRegistrationPathwayId && x.AssessmentSeriesId == assessmentCopy.AssessmentSeriesId);
                foreach (var resultCopy in assessmentCopy.TqPathwayResults)
                {
                    if (resultCopy.TqPathwayAssessmentId == 0)
                        resultCopy.TqPathwayAssessmentId = assessment.Id;

                    pathwayResults.Add(resultCopy);
                }
            }
        }

        private async Task ProcessPathwayResults(List<TqPathwayResult> pathwayResults)
        {
            if (pathwayResults == null || !pathwayResults.Any())
                return;

            pathwayResults = SortUpdateAndInsertOrder(pathwayResults, x => x.Id);
            await _dbContext.BulkInsertOrUpdateAsync(pathwayResults, bulkConfig => 
            { 
                bulkConfig.UseTempDB = true;
                bulkConfig.SetOutputIdentity = false;
                bulkConfig.PreserveInsertOrder = false;
                bulkConfig.BatchSize = 5000;
                bulkConfig.BulkCopyTimeout = 60;
            });
        }

        private async Task ProcessSpecialismAssessments(BulkConfig bulkConfig, List<TqSpecialismAssessment> specialismAssessments, List<TqSpecialismResult> specialismResults)
        {
            if (specialismAssessments == null || !specialismAssessments.Any())
                return;

            // Dev note: below Copy obj has navigation prop's but no newly added Id's
            var specialismAssessmentsCopy = new List<TqSpecialismAssessment>(specialismAssessments);

            specialismAssessments = SortUpdateAndInsertOrder(specialismAssessments, x => x.Id);

            await _dbContext.BulkInsertOrUpdateAsync(specialismAssessments, bulkConfig);

            foreach (var assessmentCopy in specialismAssessmentsCopy)
            {
                // Dev note: below specialismAssessments obj has no navigation prop's but has newly added Id's
                var assessment = specialismAssessments.FirstOrDefault(x => x.TqRegistrationSpecialismId == assessmentCopy.TqRegistrationSpecialismId && x.AssessmentSeriesId == assessmentCopy.AssessmentSeriesId);
                foreach (var resultCopy in assessmentCopy.TqSpecialismResults)
                {
                    if (resultCopy.TqSpecialismAssessmentId == 0)
                        resultCopy.TqSpecialismAssessmentId = assessment.Id;

                    specialismResults.Add(resultCopy);
                }
            }
        }

        private async Task ProcessSpecialismResults(List<TqSpecialismResult> specialismResults)
        {
            if (specialismResults == null || !specialismResults.Any())
                return;

            specialismResults = SortUpdateAndInsertOrder(specialismResults, x => x.Id);
            await _dbContext.BulkInsertOrUpdateAsync(specialismResults, bulkConfig =>
            {
                bulkConfig.UseTempDB = true;
                bulkConfig.SetOutputIdentity = false;
                bulkConfig.PreserveInsertOrder = false;
                bulkConfig.BatchSize = 5000;
                bulkConfig.BulkCopyTimeout = 60;
            });
        }

        private async Task ProcessIndustryPlacements(List<IndustryPlacement> industryPlacements)
        {
            if (industryPlacements == null || !industryPlacements.Any())
                return;

            industryPlacements = SortUpdateAndInsertOrder(industryPlacements, x => x.Id);
            await _dbContext.BulkInsertOrUpdateAsync(industryPlacements, bulkConfig => 
            { 
                bulkConfig.UseTempDB = true;
                bulkConfig.SetOutputIdentity = false;
                bulkConfig.PreserveInsertOrder = false;
                bulkConfig.OnSaveChangesSetFK = false;
                bulkConfig.BatchSize = 5000;
                bulkConfig.BulkCopyTimeout = 60; 
            });
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
    }
}
