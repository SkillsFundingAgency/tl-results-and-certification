using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TlPathwaySpecialismCombination
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.TlPathwaySpecialismCombination>
    {
        private Domain.Models.TlPathwaySpecialismCombination _result;
        private Domain.Models.TlPathwaySpecialismCombination _data;
        
        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            _data = new TlPathwaySpecialismCombinationBuilder().Build(EnumAwardingOrganisation.Pearson);
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
            _data.TlPathwayId = 2;
            _data.TlSpecialismId = 2;
            _data.IsActive = false;
            _data.ModifiedBy = ModifiedBy;
        }

        public async override Task When()
        {
            await Repository.UpdateAsync(_data);
            _result = await Repository.GetFirstOrDefaultAsync(x => x.Id == _data.Id);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TlPathwayId.Should().Be(_data.TlPathwayId);
            _result.TlSpecialismId.Should().Be(_data.TlSpecialismId);
            _result.IsActive.Should().Be(_data.IsActive);
            _result.GroupId.Should().Be(_data.GroupId);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
