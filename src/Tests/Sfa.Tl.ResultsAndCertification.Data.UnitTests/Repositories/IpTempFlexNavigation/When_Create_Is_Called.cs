using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.IpTempFlexNavigation
{
    public class When_Create_Is_Called : BaseTest<Domain.Models.IpTempFlexNavigation>
    {
        private Domain.Models.IpTempFlexNavigation _data;
        private int _result;

        public override void Given()
        {
            _data = new IpTempFlexNavigationBuilder().Build(EnumAwardingOrganisation.Pearson);
        }

        public async override Task When()
        {
            _result = await Repository.CreateAsync(_data);
        }

        [Fact]
        public void Then_Expected_Records_Should_Have_Been_Created() => _result.Should().Be(1);
    }
}
