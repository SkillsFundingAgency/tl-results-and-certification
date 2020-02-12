using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.AwardingOrganisation
{
    public class When_TqAwardingOrganisationRepository_Navigate_Is_Called : BaseTest<TqAwardingOrganisation>
    {
        private TqAwardingOrganisation _result;
        private TqAwardingOrganisation _data;

        public override void Given()
        {
            _data = new TqAwardingOrganisationBuilder().Build();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public override void When()
        {
            _result = Repository.GetFirstOrDefaultAsync(x => x.Id == 1).GetAwaiter().GetResult();
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

            var expectedAwardingOrg = _data.TlAwardingOrganisaton;
            _result.TlAwardingOrganisaton.Name.Should().Be(expectedAwardingOrg.Name);

            var expectedPathway = _data.TlPathway;
            _result.TlPathway.Name.Should().Be(expectedPathway.Name);
        }
    }
}
