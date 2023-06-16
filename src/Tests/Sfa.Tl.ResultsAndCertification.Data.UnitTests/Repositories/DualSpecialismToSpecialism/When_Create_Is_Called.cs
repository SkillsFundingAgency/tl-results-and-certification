using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.DualSpecialismToSpecialism
{
    public class When_Create_Is_Called : BaseTest<TlDualSpecialismToSpecialism>
    {
        private Domain.Models.TlDualSpecialismToSpecialism _data;
        private int _result;

        public override void Given()
        {
            _data = new TlDualSpecialismToSpecialismBuilder().Build(EnumAwardingOrganisation.Ncfe);
        }

        public async override Task When()
        {
            _result = await Repository.CreateAsync(_data);
        }

        [Fact]
        public void Then_One_Record_Should_Have_Been_Created() => _result.Should().Be(1);
    }
}
