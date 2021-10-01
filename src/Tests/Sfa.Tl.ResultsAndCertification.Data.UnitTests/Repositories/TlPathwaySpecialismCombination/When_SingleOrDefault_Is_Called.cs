using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.PathwaySpecialismCombination
{
    public class When_SingleOrDefault_Is_Called : BaseTest<Domain.Models.TlPathwaySpecialismCombination>
    {
        private Domain.Models.TlPathwaySpecialismCombination _result;
        private Domain.Models.TlPathwaySpecialismCombination _data;

        public override void Given()
        {
            _data = new TlPathwaySpecialismCombinationBuilder().Build(EnumAwardingOrganisation.Pearson);
            DbContext.Add(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetSingleOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TlPathwayId.Should().Be(_data.TlPathwayId);
            _result.TlSpecialismId.Should().Be(_data.TlSpecialismId);
            _result.GroupId.Should().Be(_data.GroupId);
            _result.IsActive.Should().Be(_data.IsActive);
            _result.TlPathwayId.Should().Be(_data.TlPathwayId);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
