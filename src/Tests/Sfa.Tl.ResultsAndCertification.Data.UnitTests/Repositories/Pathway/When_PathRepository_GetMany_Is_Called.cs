using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Pathway
{
    public class When_PathRepository_GetMany_Is_Called : BaseTest<TlPathway>
    {
        private IEnumerable<TlPathway> _result;
        private IEnumerable<TlPathway> _data;
        private EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;

        public override void Given()
        {
            _data = new TlPathwayBuilder().BuildList(_awardingOrganisation);
            DbContext.TlPathway.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetManyAsync().ToListAsync();
        }

        [Fact]
        public void Then_Results_Not_Null()
        {
            _result.Should().NotBeNull();
        }


        [Fact]
        public void Then_The_Expected_Number_Of_Paths_Is_Returned()
        {
            _result.Count().Should().Be(2);
        }

        [Fact]
        public void Then_EntityFields_Are_As_Expected()
        {
            var expectedResult = _data.FirstOrDefault();
            var actualResult = _result.FirstOrDefault();
            
            expectedResult.Should().NotBeNull();
            actualResult.Should().NotBeNull();
            actualResult.Id.Should().Be(1);
            actualResult.Name.Should().BeEquivalentTo(expectedResult.Name);
            actualResult.LarId.Should().BeEquivalentTo(expectedResult.LarId);
            actualResult.TlRouteId.Should().Be(expectedResult.TlRouteId);
            actualResult.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            actualResult.CreatedOn.Should().Be(expectedResult.CreatedOn);
            actualResult.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            actualResult.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
