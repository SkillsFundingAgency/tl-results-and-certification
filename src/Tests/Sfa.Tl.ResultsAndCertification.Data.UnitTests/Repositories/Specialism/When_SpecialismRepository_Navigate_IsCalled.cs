using System.Linq;
using Xunit;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Specialism
{
    public class When_SpecialismRepository_Navigate_IsCalled : BaseTest<TlSpecialism>
    {
        private TlSpecialism _result;
        private TlSpecialism _data;
        private EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Ncfe;

        public override void Given()
        {
            var tlSpecialisms = new TlSpecialismBuilder().BuildList(_awardingOrganisation);
            DbContext.AddRange(tlSpecialisms);
            DbContext.SaveChanges();
            _data = tlSpecialisms.FirstOrDefault();
        }

        public override void When()
        {
            _result = Repository.GetFirstOrDefaultAsync(x => x.Id == _data.Id).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Results_Not_Null()
        {
            _result.Should().NotBeNull();
            _result.TlPathway.Should().NotBeNull();
        }

        [Fact]
        public void Then_Pathway_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.TlPathway;
            
            _result.TlPathway.Id.Should().Be(expectedResult.Id);
            _result.TlPathway.Name.Should().Be(expectedResult.Name);
            _result.TlPathway.LarId.Should().Be(expectedResult.LarId);
            _result.TlPathway.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            _result.TlPathway.CreatedOn.Should().Be(expectedResult.CreatedOn);
            _result.TlPathway.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            _result.TlPathway.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }        
    }
}
