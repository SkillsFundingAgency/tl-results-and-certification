using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.IpTempFlexNavigation
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.IpTempFlexNavigation>
    {
        private Domain.Models.IpTempFlexNavigation _result;
        private Domain.Models.IpTempFlexNavigation _data;

        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            _data = new IpTempFlexNavigationBuilder().Build(EnumAwardingOrganisation.Pearson);
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
            _data.TlPathwayId = 2;
            _data.AskTempFlexibility = false;
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
            _result.AcademicYear.Should().Be(_data.AcademicYear);
            _result.AskTempFlexibility.Should().Be(_data.AskTempFlexibility);
            _result.AskBlendedPlacement.Should().Be(_data.AskBlendedPlacement);
            _result.IsActive.Should().Be(_data.IsActive);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
