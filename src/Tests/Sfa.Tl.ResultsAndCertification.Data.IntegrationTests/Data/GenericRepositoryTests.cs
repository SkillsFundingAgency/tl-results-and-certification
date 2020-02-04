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
        public async Task GetTqProvidersByAwardingOrganisation()
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
           var result = await genericRepository.GetManyAsync(x => x.TqAwardingOrganisation.Name.Equals(inputAo)).ToListAsync();

            // 4. Assert results. 
            Assert.NotNull(result);
            Assert.True(result.First().TlProviderId == 2);

            // Assert not found scenario.
            inputAo = "AO1";
            genericRepository = new GenericRepository<TqProvider>(mockLogger.Object, context);
            result = await genericRepository.GetManyAsync(x => x.TqAwardingOrganisation.Name.Equals(inputAo)).ToListAsync();

            // 4. Assert results. 
            Assert.NotNull(result);
            Assert.True(result.Count() == 0);
        }


        [Fact(Skip = "This is not fully functional.")]
        public async Task GetQualificationsByAwardingOrganisation()
        {
            // Input: "AOID"
            // Output[i.e. Qualification]: "RouteName: PathwayName" 

            // 1. Build Provider, Ao and TlProvider data
            using var context = CreateDbContext("GetQualificationsByAwardingOrganisation");

            // TODO: Write/Use Builders
            await CreateRoutes(context);
            await CreatePathways(context);
            await CreateSpecialisms(context);
            await CreateProviders(context);

            var inputAoId = 1;

            //var result = context.TqProvider.Where(x => x.AwardingOrganisationId == inputAoId)
            //    .Select(x => x.Route.Name + ": " + x.Pathway.Name);

            //Assert.Equal("Construction: Design, Surveying and Planning", result.FirstOrDefault());

            var genericRepository = new GenericRepository<TqProvider>(mockLogger.Object, context);
            
            //IEnumerable<TqProvider> result = await genericRepository.GetManyAsync(x => x.AwardingOrganisation.Name.Equals(inputAoId));
        }

        private async Task CreateProviders(ResultsAndCertificationDbContext context)
        {
            await context.TqProvider.AddRangeAsync(new List<TqProvider>
            {
                new TqProvider { TqAwardingOrganisationId  = 1, TlProviderId = 1, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            });

            await context.SaveChangesAsync();
        }

        private async Task CreateSpecialisms(ResultsAndCertificationDbContext context)
        {
            await context.TlSpecialism.AddRangeAsync(new List<TlSpecialism>
            {
                new TlSpecialism {  Id = 1, Name ="Surveying and design for construction and the built environment", TlPathwayId = 1, CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
                new TlSpecialism {  Id = 2, Name ="Civil Engineering", TlPathwayId = 1, CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
                new TlSpecialism {  Id = 3, Name ="Building services design", TlPathwayId = 1, CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
                new TlSpecialism {  Id = 4, Name ="Hazardous materials analysis and surveying", TlPathwayId = 1, CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
                new TlSpecialism {  Id = 5, Name ="Early years education and childcare", TlPathwayId = 2, CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
                new TlSpecialism {  Id = 6, Name ="Assisting teaching", TlPathwayId = 2, CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
                new TlSpecialism {  Id = 7, Name ="Supporting and mentoring students in further and higher education", TlPathwayId = 2, CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
                new TlSpecialism {  Id = 8, Name ="Digital Production, Design and Development", TlPathwayId = 3, CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
            });

            await context.SaveChangesAsync();
        }

        private async Task CreatePathways(ResultsAndCertificationDbContext context)
        {
            var data = new List<TlPathway>
            {
                new TlPathway {  Id = 1, TlRouteId = 1, LarId = "10123456", Name = "Design, Surveying and Planning", CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
                new TlPathway {  Id = 2, TlRouteId = 2, LarId = "10223456", Name = "Education", CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
                new TlPathway {  Id = 3, TlRouteId = 3, LarId = "10223456", Name = "Digital Production, Design and Development", CreatedBy = "Sys", CreatedOn = DateTime.UtcNow }
            };

            var mockLogger = new Mock<ILogger<GenericRepository<TlPathway>>>();
            var genericRepository = new GenericRepository<TlPathway>(mockLogger.Object, context);
            await genericRepository.CreateManyAsync(data);

            //await context.TlPathway.AddRangeAsync(data);
            //context.Entry<IEnumerable<TlPathway>>(data).State = EntityState.Detached;
            //await context.SaveChangesAsync();
        }

        private async Task CreateRoutes(ResultsAndCertificationDbContext context)
        {
            var data = new List<TlRoute>
            {
                new TlRoute { Id = 1, Name = "Construction", CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
                new TlRoute { Id = 2, Name = "Education and Childcare", CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
                new TlRoute { Id = 3, Name = "Digital", CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
            };

            var mockLogger = new Mock<ILogger<GenericRepository<TlRoute>>>();
            var genericRepository = new GenericRepository<TlRoute>(mockLogger.Object, context);
            await genericRepository.CreateManyAsync(data);

            //await context.TlRoute.AddRangeAsync(data);

            //context.Entry<IEnumerable<TlRoute>>(data).State = EntityState.Detached;

            //await context.SaveChangesAsync();
        }

        private async Task CreateOneTqProvider(ResultsAndCertificationDbContext context)
        {
            await context.TqProvider.AddRangeAsync(new List<TqProvider>
            {
                new TqProvider { TqAwardingOrganisationId = 2, TlProviderId = 2,  CreatedBy = "Sys", CreatedOn = DateTime.UtcNow },
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
            await context.TlProvider.AddRangeAsync(new List<TlProvider>
            {
                new TlProvider{ Name = "P1", DisplayName = "Provider1", UkPrn = 111, CreatedBy = "sys", CreatedOn = DateTime.UtcNow },
                new TlProvider{ Name = "P2", DisplayName = "Provider2", UkPrn = 111, CreatedBy = "sys", CreatedOn = DateTime.UtcNow },
                // TODO: which process will set the flag - IsTlevelProvider.. 
            });
            await context.SaveChangesAsync();
        }

        private ResultsAndCertificationDbContext CreateDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ResultsAndCertificationDbContext>()
                .UseInMemoryDatabase(dbName)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            return new ResultsAndCertificationDbContext(options);
        }
    }
}
