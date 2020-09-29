using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Pathway
{
    public class When_Navigate_Is_Called : BaseTest<TlPathway>
    {
        private TlPathway _result;
        private TlPathway _data;
        private EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;

        public override void Given()
        {
            _data = new TlPathwayBuilder().Build(_awardingOrganisation);
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetFirstOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Results_Not_Null()
        {
            _result.Should().NotBeNull();
            _result.TlRoute.Should().NotBeNull();
        }

        [Fact]
        public void Then_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.TlRoute;
            
            _result.TlRoute.Id.Should().Be(expectedResult.Id);
            _result.TlRoute.Name.Should().Be(expectedResult.Name);
            _result.TlRoute.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            _result.TlRoute.CreatedOn.Should().Be(expectedResult.CreatedOn);
            _result.TlRoute.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            _result.TlRoute.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
