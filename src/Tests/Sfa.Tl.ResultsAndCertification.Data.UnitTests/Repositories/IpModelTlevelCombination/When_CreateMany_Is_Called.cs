using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.IpModelTlevelCombination
{
    public class When_CreateMany_Is_Called : BaseTest<Domain.Models.IpModelTlevelCombination>
    {
        private IList<Domain.Models.IpModelTlevelCombination> _data;
        private int _result;

        public override void Given()
        {
            _data = new IpModelTlevelCombinationBuilder().BuildList(EnumAwardingOrganisation.Pearson);
        }

        public async override Task When()
        {
            _result = await Repository.CreateManyAsync(_data);
        }

        [Fact]
        public void Then_Expected_Records_Should_Have_Been_Created() => _result.Should().Be(_data.Count);
    }
}
