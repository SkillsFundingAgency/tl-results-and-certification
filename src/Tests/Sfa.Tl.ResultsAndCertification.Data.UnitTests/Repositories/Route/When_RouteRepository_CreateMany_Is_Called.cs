using Xunit;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_CreateMany_Is_Called : BaseTest<TlRoute>
    {
        private int _result;
        private IList<TlRoute> _data;

        public override void Given()
        {
            _data = new TlRouteBuilder().BuildList();
        }

        public override void When()
        {
            _result = Repository.CreateManyAsync(_data)
                   .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);
    }
}
