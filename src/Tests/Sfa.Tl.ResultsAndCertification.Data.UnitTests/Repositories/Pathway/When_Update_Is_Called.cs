using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Pathway
{
    public class When_Update_Is_Called : BaseTest<TlPathway>
    {
        private TlPathway _result;
        private TlPathway _data;
        private readonly EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;

        private const string PathwayName = "Pathway Updated";
        private const string LarId = "999";
        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            _data = new TlPathwayBuilder().Build(_awardingOrganisation);
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
            _data.Name = PathwayName;
            _data.LarId = LarId;
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
            _result.Name.Should().BeEquivalentTo(_data.Name);
            _result.StartYear.Should().Be(_data.StartYear);
            _result.CreatedBy.Should().Be(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
