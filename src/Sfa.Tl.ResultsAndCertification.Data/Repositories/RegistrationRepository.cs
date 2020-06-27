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
    public class RegistrationRepository : GenericRepository<TqRegistrationProfile>, IRegistrationRepository
    {
        private ILogger<RegistrationRepository> _logger;
        
        public RegistrationRepository(ILogger<RegistrationRepository> logger, ResultsAndCertificationDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
        }

        public async Task<IList<TqRegistrationProfile>> GetRegistrationProfilesAsync(IList<TqRegistrationProfile> registrations)
        {
            var ulns = new HashSet<long>();
            registrations.ToList().ForEach(r => ulns.Add(r.UniqueLearnerNumber));

            return await _dbContext.TqRegistrationProfile.Where(x => ulns.Contains(x.UniqueLearnerNumber))
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.TqRegistrationSpecialisms)
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.TqProvider)
                        .ThenInclude(x => x.TqAwardingOrganisation)
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
                            var bulkConfig = new BulkConfig() { UseTempDB = true, PreserveInsertOrder = true, SetOutputIdentity = true, BatchSize = 4000 };
                            var pathwayRegistrations = pathwayEntities ?? new List<TqRegistrationPathway>();

                            if (profileEntities != null && profileEntities.Count > 0)
                            {
                                // please do not remove below ordering line. It is important as we are doing BulkInsertOrUpdate in one go, we would like to have update
                                // records at the top and newley added records at the bootom of the list, so that PreserveInsertOrder and SetOutputIdentiy
                                // will work as expected. If you remove below line then Id values will be interchanged.
                                // please do not remove below line
                                profileEntities = SortUpdateAndInsertOrder(profileEntities, x => x.Id);

                                await _dbContext.BulkInsertOrUpdateAsync(profileEntities, bulkConfig);

                                profileEntities.ToList().ForEach(profile =>
                                {
                                    profile.TqRegistrationPathways.ToList().ForEach(pathway =>
                                    {
                                        if (pathway.TqRegistrationProfileId == 0)
                                        {
                                            pathway.TqRegistrationProfileId = profile.Id;
                                        }
                                        pathwayRegistrations.Add(pathway);
                                    });
                                });
                            }

                            var specialismRegistrations = specialismEntities ?? new List<TqRegistrationSpecialism>();

                            if (pathwayRegistrations.Count > 0)
                            {
                                // below ordering line is important as we are doing BulkInsertOrUpdate in one go, we would like to have update 
                                // records at the top and newley added records at the bootom of the list, so that PreserveInsertOrder and SetOutputIdentiy
                                // will work as expected. If you remove below line then Id values will be interchanged.
                                // please do not remove below line
                                pathwayRegistrations = SortUpdateAndInsertOrder(pathwayRegistrations, x => x.Id);

                                await _dbContext.BulkInsertOrUpdateAsync(pathwayRegistrations, bulkConfig);

                                pathwayRegistrations.ForEach(pathway =>
                                {
                                    pathway.TqRegistrationSpecialisms.ToList().ForEach(specialism =>
                                    {
                                        if (specialism.TqRegistrationPathwayId == 0)
                                        {
                                            specialism.TqRegistrationPathwayId = pathway.Id;
                                        }
                                        specialismRegistrations.Add(specialism);
                                    });
                                });
                            }

                            if (specialismRegistrations.Count > 0)
                            {
                                // below ordering line is important as we are doing BulkInsertOrUpdate in one go, we would like to have update 
                                // records at the top and newley added records at the bootom of the list, so that PreserveInsertOrder and SetOutputIdentiy
                                // will work as expected. If you remove below line then Id values will be interchanged.
                                // please do not remove below line
                                specialismRegistrations = SortUpdateAndInsertOrder(specialismRegistrations, x => x.Id);
                                await _dbContext.BulkInsertOrUpdateAsync(specialismRegistrations, bulkConfig => { bulkConfig.UseTempDB = true; bulkConfig.PreserveInsertOrder = true; bulkConfig.SetOutputIdentity = true; bulkConfig.BatchSize = 4000; });
                            }

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

        private List<T> SortUpdateAndInsertOrder<T>(List<T> entities, Func<T, int> selector) where T : class
        {
            var returnResult = new List<T>();

            if (entities != null && selector != null)
            {
                var listToUpdate = entities.Where(x => selector(x) > 0).OrderBy(x => selector);
                var listToAdd = entities.Where(x => selector(x) <= 0).OrderBy(x => selector);

                returnResult.AddRange(listToUpdate);
                returnResult.AddRange(listToAdd);
            }
            return returnResult;
        }
    }
}
