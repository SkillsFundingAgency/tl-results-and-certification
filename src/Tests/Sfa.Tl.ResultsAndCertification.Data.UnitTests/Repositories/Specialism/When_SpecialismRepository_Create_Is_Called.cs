using System.Linq;
using Xunit;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Specialism
{
    public class When_SpecialismRepository_Create_Is_Called : BaseTest<TlSpecialism>
    {
        private TlSpecialism _data;
        private int _result;
        private EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Ncfe;

        public override void Given()
        {
            var tlSpecialisms = new TlSpecialismBuilder().BuildList(_awardingOrganisation);
            _data = tlSpecialisms.FirstOrDefault();
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
