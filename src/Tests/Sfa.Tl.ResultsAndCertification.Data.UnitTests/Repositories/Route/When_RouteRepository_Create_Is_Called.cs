using Xunit;
using NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_Create_Is_Called
    {
        private readonly int _result;

        public When_RouteRepository_Create_Is_Called()
        {

            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.TlRoute>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var data = new TlRouteBuilder().Build();

                var repository = new GenericRepository<Domain.Models.TlRoute>(logger, dbContext);
                _result = repository.CreateAsync(data).GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_One_Record_Should_Have_Been_Created() =>
            _result.Should().Be(1);
    }
}
