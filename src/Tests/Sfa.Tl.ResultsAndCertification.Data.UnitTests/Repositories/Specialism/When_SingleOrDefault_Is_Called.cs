using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Specialism
{
    public class When_SingleOrDefault_Is_Called : BaseTest<TlSpecialism>
    {
        private TlSpecialism _result;
        private TlSpecialism _data;
        private readonly EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Ncfe;

        public override void Given()
        {
            var tlSpecialisms = new TlSpecialismBuilder().BuildList(_awardingOrganisation);
            DbContext.AddRange(tlSpecialisms);
            DbContext.SaveChanges();
            _data = tlSpecialisms.FirstOrDefault();
        }

        public async override Task When()
        {
            _result = await Repository.GetSingleOrDefaultAsync(x => x.Id == _data.Id);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _result.Should().NotBeNull();
            _result.Id.Should().Be(_data.Id);          
            _result.Name.Should().BeEquivalentTo(_data.Name);
            _result.LarId.Should().BeEquivalentTo(_data.LarId);
            _result.TlPathwayId.Should().Be(_data.TlPathwayId);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
