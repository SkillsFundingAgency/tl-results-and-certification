using Xunit;
using NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.TlRoute _result;
        private readonly Domain.Models.TlRoute _data;

        public When_RouteRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.TlRoute>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                _data = new TlRouteBuilder().Build();
                dbContext.AddRange(_data);
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.TlRoute>(logger, dbContext);
                _result = repository.GetSingleOrDefaultAsync(x => x.Id == 1).GetAwaiter().GetResult();
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
            _result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
