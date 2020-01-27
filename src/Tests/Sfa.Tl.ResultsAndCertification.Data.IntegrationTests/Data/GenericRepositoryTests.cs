using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.IntegrationTests.Data
{
    public class GenericRepositoryTests
    {
        private readonly Mock<ILogger<GenericRepository<TqProvider>>> mockLogger;

        public GenericRepositoryTests()
        {
            mockLogger = new Mock<ILogger<GenericRepository<TqProvider>>>();
        }

        [Fact]
        public async Task GetTqProvidersOfGivenAwardingOrganisation()
        {
            // 1. Build Provider, Ao and TlProvider data
            using var context = CreateDbContext("GetTqProvidersOfGivenAwardingOrganisation");

            // TODO: Write/Use Builders
            await CreateTwoAwardOrganisations(context);
            await CreateTwoProviders(context);
            await CreateOneTqProvider(context);

            // 3. Get TqProviders for AO1
            var inputAo = "AO2";

           var genericRepository = new GenericRepository<TqProvider>(mockLogger.Object, context);
           var result = await genericRepository.GetManyAsync(x => x.AwardingOrganisation.Name.Equals(inputAo)).ToListAsync();

            // 4. Assert results. 
            Assert.NotNull(result);
            Assert.True(result.First().ProviderId == 2);

            // Assert not found scenario.
            inputAo = "AO1";
            genericRepository = new GenericRepository<TqProvider>(mockLogger.Object, context);
            result = await genericRepository.GetManyAsync(x => x.AwardingOrganisation.Name.Equals(inputAo)).ToListAsync();

            // 4. Assert results. 
            Assert.NotNull(result);
            Assert.True(result.Count() == 0);
        }

        private async Task CreateOneTqProvider(ResultsAndCertificationDbContext context)
        {
            await context.TqProvider.AddRangeAsync(new List<TqProvider>
            {
                new TqProvider { AwardingOrganisationId = 2, ProviderId = 2,  CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
            });

            await context.SaveChangesAsync();
        }

        private async Task CreateTwoAwardOrganisations(ResultsAndCertificationDbContext context)
        {
            await context.TqAwardingOrganisation.AddRangeAsync(new List<TqAwardingOrganisation>
            {
                new TqAwardingOrganisation { Name = "AO1", DisplayName = "Award Org1", UkAon = "test1", CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
                new TqAwardingOrganisation { Name = "AO2", DisplayName = "Award Org2", UkAon = "test2", CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
            });

            await context.SaveChangesAsync();
        }

        private async Task CreateTwoProviders(ResultsAndCertificationDbContext context)
        {
            await context.Provider.AddRangeAsync(new List<Provider>
            {
                new Provider{ Name = "P1", DisplayName = "Provider1", UkPrn = 111, IsTlevelProvider = false, CreatedBy = "sys", CreatedOn = DateTime.UtcNow },
                new Provider{ Name = "P2", DisplayName = "Provider2", UkPrn = 111, IsTlevelProvider = true, CreatedBy = "sys", CreatedOn = DateTime.UtcNow },
                // TODO: which process will set the flag - IsTlevelProvider.. 
            });
            await context.SaveChangesAsync();
        }

        private ResultsAndCertificationDbContext CreateDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ResultsAndCertificationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new ResultsAndCertificationDbContext(options);
        }
    }
}
