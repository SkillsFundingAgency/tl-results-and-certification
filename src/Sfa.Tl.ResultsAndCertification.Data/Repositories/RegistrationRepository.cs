using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
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

        public async Task<TqRegistrationProfile> GetRegistrationProfileAsync(long aoUkprn, int profileId)
        {
            var profile = await _dbContext.TqRegistrationProfile
                .Where(x => x.Id == profileId && x.TqRegistrationPathways.Any(pw => pw.Status == RegistrationPathwayStatus.Active && pw.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn))
                .Include(x => x.TqRegistrationPathways)
                    .ThenInclude(x => x.TqRegistrationSpecialisms)
                .FirstOrDefaultAsync();

            return profile;
        }

        public async Task<RegistrationDetails> GetRegistrationDetailsByProfileIdAsync(long aoUkprn, int profileId)
        {
            var registrationDetails = await _dbContext.TqRegistrationPathway
                .Where(p => p.Status == RegistrationPathwayStatus.Active && p.TqRegistrationProfile.Id == profileId && p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn)
                .Select(p => new RegistrationDetails
                {
                    ProfileId = p.TqRegistrationProfileId,
                    Uln = p.TqRegistrationProfile.UniqueLearnerNumber,
                    Name = $"{p.TqRegistrationProfile.Firstname} {p.TqRegistrationProfile.Lastname}",
                    DateofBirth = p.TqRegistrationProfile.DateofBirth,
                    ProviderDisplayName = $"{p.TqProvider.TlProvider.Name} ({p.TqProvider.TlProvider.UkPrn})",
                    PathwayDisplayName = $"{p.TqProvider.TqAwardingOrganisation.TlPathway.Name} ({p.TqProvider.TqAwardingOrganisation.TlPathway.LarId})",
                    SpecialismsDisplayName = p.TqRegistrationSpecialisms.Where(s => s.Status == RegistrationSpecialismStatus.Active).OrderBy(s => s.TlSpecialism.Name).Select(s => $"{s.TlSpecialism.Name} ({s.TlSpecialism.LarId})"),
                    AcademicYear = p.AcademicYear
                }).FirstOrDefaultAsync();

            return registrationDetails;
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
                            var bulkConfig = new BulkConfig() { UseTempDB = true, SetOutputIdentity = true, BatchSize = 5000, BulkCopyTimeout = 60 };

                            var pathwayRegistrations = pathwayEntities ?? new List<TqRegistrationPathway>();

                            pathwayRegistrations = await ProcessProfileEntities(bulkConfig, profileEntities, pathwayRegistrations);

                            var specialismRegistrations = specialismEntities ?? new List<TqRegistrationSpecialism>();

                            specialismRegistrations = await ProcessRegistrationPathwayEntities(bulkConfig, pathwayRegistrations, specialismRegistrations);

                            await ProcessRegistrationSpecialismEntities(specialismRegistrations);

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
                    {
                        profileEntity = profileEntities.FirstOrDefault(x => x.UniqueLearnerNumber == profile.UniqueLearnerNumber);
                    }

                    foreach (var pathway in profile.TqRegistrationPathways)
                    {
                        // update fk relationship id for newely added records
                        if (pathway.TqRegistrationProfileId == 0)
                        {
                            pathway.TqRegistrationProfileId = profileEntity.Id;
                        }
                        pathwayRegistrations.Add(pathway);
                    }
                });
            }
            return pathwayRegistrations;
        }

        private async Task<List<TqRegistrationSpecialism>> ProcessRegistrationPathwayEntities(BulkConfig bulkConfig, List<TqRegistrationPathway> pathwayRegistrations, List<TqRegistrationSpecialism> specialismRegistrations)
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
                                                                                && p.Status == Common.Enum.RegistrationPathwayStatus.Active);
                    }

                    foreach (var specialism in pathway.TqRegistrationSpecialisms)
                    {
                        // update fk relationship id for newely added records
                        if (specialism.TqRegistrationPathwayId == 0)
                        {
                            specialism.TqRegistrationPathwayId = pathwayEntity.Id;
                        }
                        specialismRegistrations.Add(specialism);
                    }
                });
            }
            return specialismRegistrations;
        }

        private async Task ProcessRegistrationSpecialismEntities(List<TqRegistrationSpecialism> specialismRegistrations)
        {
            if (specialismRegistrations.Count > 0)
            {
                specialismRegistrations = SortUpdateAndInsertOrder(specialismRegistrations, x => x.Id);
                await _dbContext.BulkInsertOrUpdateAsync(specialismRegistrations, bulkConfig => { bulkConfig.UseTempDB = true; bulkConfig.BatchSize = 5000; bulkConfig.BulkCopyTimeout = 60; });
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
