using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.IpTempFlexNavigation
{
    public class When_SingleOrDefault_Is_Called : BaseTest<Domain.Models.IpTempFlexNavigation>
    {
        private Domain.Models.IpTempFlexNavigation _result;
        private Domain.Models.IpTempFlexNavigation _data;

        public override void Given()
        {
            _data = new IpTempFlexNavigationBuilder().Build(EnumAwardingOrganisation.Pearson);
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
            _result.AcademicYear.Should().Be(_data.AcademicYear);
            _result.AskTempFlexibility.Should().Be(_data.AskTempFlexibility);
            _result.AskBlendedPlacement.Should().Be(_data.AskBlendedPlacement);
            _result.IsActive.Should().Be(_data.IsActive);
            _result.TlPathwayId.Should().Be(_data.TlPathwayId);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
