using Xunit;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Route
{
    public class When_RouteRepository_Create_Is_Called : BaseTest<TlRoute>
    {
        private int _result;
        private TlRoute _data;
        private EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;

        public override void Given()
        {
            _data = new TlRouteBuilder().Build(_awardingOrganisation);

        }
        public override void When()
        {
            _result = Repository.CreateAsync(_data).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_One_Record_Should_Have_Been_Created() =>
                _result.Should().Be(1);
    }
}
