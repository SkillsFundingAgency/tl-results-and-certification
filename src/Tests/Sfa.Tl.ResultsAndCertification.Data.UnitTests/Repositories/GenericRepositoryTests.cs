using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Data.UnitTests.Builders;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories
{
    public class GenericRepositoryTests
    {
        // TODO: GenericRepository tested on Provider Entity, but do we need to extend this or refactor for the coverage?
        private readonly Mock<ILogger<GenericRepository<Provider>>> mockLogger;

        public GenericRepositoryTests()
        {
            mockLogger = new Mock<ILogger<GenericRepository<Provider>>>();
        }

        [Fact]
        public async Task CreateAsync_RowAddedToTable() 
        {
            using var context = CreateDbContext("CreateAsync_RowAddedToEntity");

            // Given
            var genericRepository = new GenericRepository<Provider>(mockLogger.Object, context);
            var inputEntity = new ProviderBuilder()
                .WithName("SchoolOfBirmingham")
                .WithDisplayName("School Of Birmingham")
                .WithIsTlevelProvider(true)
                .WithUkPrn(123)
                .Build();

            // When
            var id = genericRepository.CreateAsync(inputEntity);

            // Then
            var resultEntity = await genericRepository.GetManyAsync().ToListAsync();
            Assert.True(resultEntity.Count == 1);

            var entity = resultEntity.First();
            
            Assert.True(entity.Name.Equals("SchoolOfBirmingham"));
            Assert.True(entity.DisplayName.Equals("School Of Birmingham"));
            Assert.True(entity.IsTlevelProvider.Equals(true));
            Assert.True(entity.UkPrn.Equals(123));
        }

        [Fact]
        public async Task UpdateAsync_RowUpdatedInTable()
        {
            using var context = CreateDbContext("UpdateAsync_RowUpdatedInTable");

            // Given
            var genericRepository = new GenericRepository<Provider>(mockLogger.Object, context);
            var inputEntity = new ProviderBuilder()
                .WithName("UOB")
                .WithDisplayName("University Of Birmingham")
                .WithIsTlevelProvider(false)
                .WithUkPrn(456)
                .Build();
            
            // Add an enity 
            var id = genericRepository.CreateAsync(inputEntity);

            // Get an entity 
            var providerData = await genericRepository.GetManyAsync().ToListAsync();
            Assert.True(providerData.Count == 1);

            // Update an entity 
            var entityToUpdate = providerData.FirstOrDefault(x => x.Name.Equals("UOB") && !x.IsTlevelProvider);
            entityToUpdate.IsTlevelProvider = true;
            entityToUpdate.Name = "COB";
            entityToUpdate.DisplayName = "College of Birmingham";
            entityToUpdate.UkPrn = 786;

            var updateResult = genericRepository.UpdateAsync(entityToUpdate);


            // When
            var results = await genericRepository.GetManyAsync().ToListAsync();

            // Then
            var entity = results.First();

            Assert.True(entity.Name.Equals("COB"));
            Assert.True(entity.DisplayName.Equals("College of Birmingham"));
            Assert.True(entity.IsTlevelProvider.Equals(true));
            Assert.True(entity.UkPrn.Equals(786));
        }

        [Fact]
        public async Task GetFirstOrDefaultAsync_ReturnsFirstMatchingRecord()
        {
            using var context = CreateDbContext("GetFirstOrDefaultAsync_ReturnsFirstMatchingRecord");

            // Given
            List<Provider> providers = new List<Provider>
            {
                new ProviderBuilder()
                .WithName("MAN")
                .WithDisplayName("Manchester University")
                .WithIsTlevelProvider(false)
                .WithUkPrn(111)
                .Build(),

                new ProviderBuilder()
                .WithName("MAN")
                .WithDisplayName("Manchester University")
                .WithIsTlevelProvider(false)
                .WithIsTlevelProvider(true)
                .WithUkPrn(222)
                .Build(),
            };

            var genericRepository = new GenericRepository<Provider>(mockLogger.Object, context);
            var ids = genericRepository.CreateManyAsync(providers);

            // When
            var resultset = await genericRepository.GetFirstOrDefaultAsync(x => x.Name.Equals("MAN") && x.IsTlevelProvider);

            // Assert result.
            Assert.NotNull(resultset);
            Assert.True(resultset.Name.Equals("MAN"));
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
