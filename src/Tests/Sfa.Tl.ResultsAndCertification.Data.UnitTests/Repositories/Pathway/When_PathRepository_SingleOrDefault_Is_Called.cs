using Xunit;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Pathway
{
    public class When_PathRepository_SingleOrDefault_Is_Called : BaseTest<TlPathway>
    {
        private TlPathway _result;
        private TlPathway _data;
        private EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;

        public override void Given()
        {
            _data = new TlPathwayBuilder().Build(_awardingOrganisation);
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
            _result.Name.Should().BeEquivalentTo(_data.Name);
            _result.LarId.Should().BeEquivalentTo(_data.LarId);
            _result.TlRouteId.Should().Be(_data.TlRouteId);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
