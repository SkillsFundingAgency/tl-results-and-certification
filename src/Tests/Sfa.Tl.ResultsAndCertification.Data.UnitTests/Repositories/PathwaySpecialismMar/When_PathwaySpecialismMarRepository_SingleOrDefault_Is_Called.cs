using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.PathwaySpecialismMar
{
    public class When_PathwaySpecialismMarRepository_SingleOrDefault_Is_Called : BaseTest<TlPathwaySpecialismMar>
    {
        private TlPathwaySpecialismMar _result;
        private TlPathwaySpecialismMar _data;

        public override void Given()
        {
            _data = new TlPathwaySpecialismMarBuilder().Build();
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
            _result.TlMandatoryAdditionalRequirementId.Should().Be(_data.TlMandatoryAdditionalRequirementId);
            _result.TlPathwayId.Should().Be(_data.TlPathwayId);
            _result.TlSpecialismId.Should().Be(_data.TlSpecialismId);
            _result.TlPathwayId.Should().Be(_data.TlPathwayId);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
