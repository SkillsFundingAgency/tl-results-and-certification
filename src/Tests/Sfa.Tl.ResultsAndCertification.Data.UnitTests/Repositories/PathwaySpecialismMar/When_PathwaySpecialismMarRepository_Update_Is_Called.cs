using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.PathwaySpecialismMar
{
    public class When_PathwaySpecialismMarRepository_Update_Is_Called : BaseTest<TlPathwaySpecialismMar>
    {
        private TlPathwaySpecialismMar _result;
        private TlPathwaySpecialismMar _data;
        
        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            _data = new TlPathwaySpecialismMarBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
            _data.TlMandatoryAdditionalRequirementId = 2;
            _data.TlPathwayId = 2;
            _data.TlSpecialismId = 2;
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
            _result.TlMandatoryAdditionalRequirementId.Should().Be(_data.TlMandatoryAdditionalRequirementId);
            _result.TlPathwayId.Should().Be(_data.TlPathwayId);
            _result.TlSpecialismId.Should().Be(_data.TlSpecialismId);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
