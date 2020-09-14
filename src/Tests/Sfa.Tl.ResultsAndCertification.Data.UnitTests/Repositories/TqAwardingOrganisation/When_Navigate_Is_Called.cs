using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.AwardingOrganisation
{
    public class When_Navigate_Is_Called : BaseTest<TqAwardingOrganisation>
    {
        private TqAwardingOrganisation _result;
        private TqAwardingOrganisation _data;
        private EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;

        public override void Given()
        {
            _data = new TqAwardingOrganisationBuilder().Build(_awardingOrganisation);
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
            _result.TlPathway.Should().NotBeNull();
        }

        [Fact]
        public void Then_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.TlPathway;
            
            _result.TlPathway.Id.Should().Be(expectedResult.Id);
            _result.TlPathway.Name.Should().Be(expectedResult.Name);
            _result.TlPathway.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            _result.TlPathway.CreatedOn.Should().Be(expectedResult.CreatedOn);
            _result.TlPathway.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            _result.TlPathway.ModifiedOn.Should().Be(expectedResult.ModifiedOn);

            var expectedAwardingOrg = _data.TlAwardingOrganisaton;
            _result.TlAwardingOrganisaton.Name.Should().Be(expectedAwardingOrg.Name);

            var expectedPathway = _data.TlPathway;
            _result.TlPathway.Name.Should().Be(expectedPathway.Name);
        }
    }
}
