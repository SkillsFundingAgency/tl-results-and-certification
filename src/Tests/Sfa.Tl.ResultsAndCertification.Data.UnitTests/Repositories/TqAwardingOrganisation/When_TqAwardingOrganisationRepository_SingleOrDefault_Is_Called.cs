using Xunit;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.AwardingOrganisation
{
    public class When_TqAwardingOrganisationRepository_SingleOrDefault_Is_Called : BaseTest<TqAwardingOrganisation>
    {
        private TqAwardingOrganisation _result;
        private TqAwardingOrganisation _data;
        private EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;

        public override void Given()
        {
            _data = new TqAwardingOrganisationBuilder().Build(_awardingOrganisation);
            DbContext.Add(_data);
            DbContext.SaveChanges();
        }

        public override void When()
        {
            _result = Repository.GetSingleOrDefaultAsync(x => x.Id == 1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TlAwardingOrganisatonId.Should().Be(_data.TlAwardingOrganisaton.Id);
            _result.TlRouteId.Should().Be(_data.TlRoute.Id);
            _result.TlPathwayId.Should().Be(_data.TlPathway.Id);
            _result.TlRouteId.Should().Be(_data.TlRouteId);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
