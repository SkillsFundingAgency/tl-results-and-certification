using Xunit;
using NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_Update_Is_Called
    {
        private readonly Domain.Models.TlRoute _result;
        private readonly Domain.Models.TlRoute _data;
        private const string UpdateRouteName = "Route Updated";
        private const string ModifiedUserName = "Route Updated";

        public When_RouteRepository_Update_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.TlRoute>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var entity = new TlRouteBuilder().Build();
                dbContext.Add(entity);
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.TlRoute>(logger, dbContext);

                entity.Name = UpdateRouteName;

                entity.ModifiedOn = DateTime.UtcNow;
                entity.ModifiedBy = ModifiedUserName;

                _data = entity;
                repository.UpdateAsync(entity).GetAwaiter().GetResult();

                _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.Name.Should().BeEquivalentTo(_data.Name);
            _result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
