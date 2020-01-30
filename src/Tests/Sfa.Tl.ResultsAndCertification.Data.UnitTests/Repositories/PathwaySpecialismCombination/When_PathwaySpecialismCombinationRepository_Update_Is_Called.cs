using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.PathwaySpecialismCombination
{
    public class When_PathwaySpecialismCombinationRepository_Update_Is_Called : BaseTest<TlPathwaySpecialismCombination>
    {
        private TlPathwaySpecialismCombination _result;
        private TlPathwaySpecialismCombination _data;
        
        private const string GroupName = "Gx Updated";
        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            _data = new TlPathwaySpecialismCombinationBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
            _data.PathwayId = 2;
            _data.SpecialismId = 2;
            _data.Group = GroupName;
            _data.ModifiedBy = ModifiedBy;
        }

        public override void When()
        {
            Repository.UpdateAsync(_data).GetAwaiter().GetResult();
            _result = Repository.GetFirstOrDefaultAsync(x => x.Id == _data.Id).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.PathwayId.Should().Be(_data.PathwayId);
            _result.SpecialismId.Should().Be(_data.SpecialismId);
            _result.Group.Should().Be(_data.Group);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
