using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.PathwaySpecialismCombination
{
    public class When_GetMany_Is_Called : BaseTest<Domain.Models.TlPathwaySpecialismCombination>
    {
        private IEnumerable<Domain.Models.TlPathwaySpecialismCombination> _result;
        private IEnumerable<Domain.Models.TlPathwaySpecialismCombination> _data;

        public override void Given()
        {
            _data = new TlPathwaySpecialismCombinationBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetManyAsync().ToListAsync();
        }
                
        [Fact]
        public void Then_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.FirstOrDefault();
            var actualResult = _result.FirstOrDefault();
            
            expectedResult.Should().NotBeNull();
            actualResult.Should().NotBeNull();
            actualResult.Id.Should().Be(1);
            actualResult.TlPathwayId.Should().Be(expectedResult.TlPathwayId);
            actualResult.TlSpecialismId.Should().Be(expectedResult.TlSpecialismId);
            actualResult.GroupId.Should().Be(expectedResult.GroupId);
            actualResult.IsActive.Should().Be(expectedResult.IsActive);
            actualResult.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            actualResult.CreatedOn.Should().Be(expectedResult.CreatedOn);
            actualResult.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            actualResult.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
