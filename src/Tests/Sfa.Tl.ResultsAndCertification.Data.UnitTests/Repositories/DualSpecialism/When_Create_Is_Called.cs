using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.DualSpecialism
{
    public class When_Create_Is_Called : BaseTest<TlDualSpecialism>
    {
        private TlDualSpecialism _data;
        private int _result;
        private EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Ncfe;

        public override void Given()
        {
            var tlDualSpecialisms = new TlDualSpecialismBuilder().BuildList(_awardingOrganisation);
            _data = tlDualSpecialisms.FirstOrDefault();
        }

        public async override Task When()
        {
            _result = await Repository.CreateAsync(_data);
        }

        [Fact]
        public void Then_One_Record_Should_Have_Been_Created() =>
            _result.Should().Be(1);
    }
}
