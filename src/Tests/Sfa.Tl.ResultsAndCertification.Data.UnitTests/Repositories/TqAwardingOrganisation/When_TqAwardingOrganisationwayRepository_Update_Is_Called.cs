using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.AwardingOrganisation
{
    public class When_TqAwardingOrganisationwayRepository_Update_Is_Called : BaseTest<TqAwardingOrganisation>
    {
        private TqAwardingOrganisation _result;
        private TqAwardingOrganisation _data;
        private EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;
        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            _data = new TqAwardingOrganisationBuilder().Build(_awardingOrganisation);
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
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
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
