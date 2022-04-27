using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.IpModelTlevelCombination
{
    public class When_GetFirstOrDefault_Is_Called : BaseTest<Domain.Models.IpModelTlevelCombination>
    {
        private Domain.Models.IpModelTlevelCombination _result;
        private IEnumerable<Domain.Models.IpModelTlevelCombination> _data;

        public override void Given()
        {
            _data = new IpModelTlevelCombinationBuilder().BuildList(EnumAwardingOrganisation.Pearson);
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetFirstOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            var expectedResult = _data.FirstOrDefault(x => x.Id == 1);

            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TlPathwayId.Should().Be(expectedResult.TlPathwayId);
            _result.IpLookupId.Should().Be(expectedResult.IpLookupId);
            _result.IsActive.Should().Be(expectedResult.IsActive);
            _result.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            _result.CreatedOn.Should().Be(expectedResult.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            _result.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
