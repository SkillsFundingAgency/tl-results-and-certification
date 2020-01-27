using Xunit;
using NSubstitute;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Linq;
using System.Collections.Generic;


namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_GetMany_Is_Called
    {
        private readonly IEnumerable<Domain.Models.TlRoute> _result;
        private readonly IList<Domain.Models.TlRoute> _data;

        public When_RouteRepository_GetMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.TlRoute>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                _data = new TlRouteBuilder().BuildList();
                dbContext.AddRange(_data);
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.TlRoute>(logger, dbContext);
                _result = repository.GetManyAsync().ToList();
            }
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Paths_Is_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_First_Path_Fields_Have_Expected_Values()
        {
            var testData = _data.FirstOrDefault();
            var result = _result.FirstOrDefault();
            testData.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().BeEquivalentTo(testData.Name);
            result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            result.CreatedOn.Should().Be(Constants.CreatedOn);
            result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
