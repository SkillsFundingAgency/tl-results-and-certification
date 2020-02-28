using Xunit;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TlAwardingOrganisation
{
    public class When_TlAwardingOrganisationRepository_Create_Is_Called : BaseTest<Domain.Models.TlAwardingOrganisation>
    {
        private Domain.Models.TlAwardingOrganisation _data;
        private int _result;
        private EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;

        public override void Given()
        {
            _data = new TlAwardingOrganisationBuilder().Build(_awardingOrganisation);
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
