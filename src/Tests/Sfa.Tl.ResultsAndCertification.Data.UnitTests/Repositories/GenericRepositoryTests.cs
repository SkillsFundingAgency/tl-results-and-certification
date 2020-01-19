using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Data.UnitTests.Builders;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Linq;
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
        public void CreateAsync_RowAddedToTable() 
        {
            var options = new DbContextOptionsBuilder<ResultsAndCertificationDbContext>()
           .UseInMemoryDatabase(databaseName: "CreateAsync_RowAddedToEntity")
           .Options;

            using var context = new ResultsAndCertificationDbContext(options);

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
            var resultEntity = genericRepository.GetManyAsync().Result;
            Assert.True(resultEntity.Count == 1);

            var entity = resultEntity.First();
            
            Assert.True(entity.Name.Equals("SchoolOfBirmingham"));
            Assert.True(entity.DisplayName.Equals("School Of Birmingham"));
            Assert.True(entity.IsTlevelProvider.Equals(true));
            Assert.True(entity.UkPrn.Equals(123));
        }
        
        [Fact]
        public void UpdateAsync_RowUpdatedInTable()
        {
            var options = new DbContextOptionsBuilder<ResultsAndCertificationDbContext>()
                       .UseInMemoryDatabase(databaseName: "UpdateAsync_RowUpdatedInTable")
                       .Options;

            using var context = new ResultsAndCertificationDbContext(options);

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
            var providerData = genericRepository.GetManyAsync().Result;
            Assert.True(providerData.Count == 1);

            // Update an entity 
            var entityToUpdate = providerData.FirstOrDefault(x => x.Name.Equals("UOB") && !x.IsTlevelProvider);
            entityToUpdate.IsTlevelProvider = true;
            entityToUpdate.Name = "COB";
            entityToUpdate.DisplayName = "College of Birmingham";
            entityToUpdate.UkPrn = 786;

            var updateResult = genericRepository.UpdateAsync(entityToUpdate);


            // When
            var results = genericRepository.GetManyAsync().Result;

            // Then
            var entity = results.First();

            Assert.True(entity.Name.Equals("COB"));
            Assert.True(entity.DisplayName.Equals("College of Birmingham"));
            Assert.True(entity.IsTlevelProvider.Equals(true));
            Assert.True(entity.UkPrn.Equals(786));
        }

        [Fact]
        public void GetFirstOrDefaultAsync_ReturnsFirstMatchingRecord()
        {
            var options = new DbContextOptionsBuilder<ResultsAndCertificationDbContext>()
            .UseInMemoryDatabase(databaseName: "GetFirstDB")
            .Options;

            using var context = new ResultsAndCertificationDbContext(options);

            // Given
            List<Provider> inputEntity = new List<Provider>
            {
                new ProviderBuilder()
                .WithName("MAN")
                .WithDisplayName("Manchester University")
                .WithIsTlevelProvider(false)
                .WithUkPrn(111)
                .Build(),

                new ProviderBuilder()
                .WithName("MANManchester University")
                .WithIsTlevelProvider(true)
                .WithUkPrn(111)
                .Build(),
            };

            var genericRepository = new GenericRepository<Provider>(mockLogger.Object, context);
            var ids = genericRepository.CreateManyAsync(inputEntity);

            // When
            var resultset = genericRepository.GetFirstOrDefaultAsync(x => x.Name.Equals("MAN") && x.IsTlevelProvider);

            // Assert result.
            Assert.NotNull(resultset.Result);
            Assert.True(resultset.Result.Name.Equals("MAN"));
        }
    }
}
