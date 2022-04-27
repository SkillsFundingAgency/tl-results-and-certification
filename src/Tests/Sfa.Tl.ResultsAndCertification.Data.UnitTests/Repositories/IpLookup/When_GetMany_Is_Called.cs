using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.IpLookup
{
    public class When_GetMany_Is_Called : BaseTest<Domain.Models.IpLookup>
    {
        private IEnumerable<Domain.Models.IpLookup> _result;
        private IEnumerable<Domain.Models.IpLookup> _data;

        public override void Given()
        {
            _data = new IpLookupBuilder().BuildList();
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
            actualResult.TlLookupId.Should().Be(expectedResult.TlLookupId);
            actualResult.Name.Should().Be(expectedResult.Name);
            actualResult.StartDate.Should().Be(expectedResult.StartDate);
            actualResult.EndDate.Should().Be(expectedResult.EndDate);
            actualResult.ShowOption.Should().Be(expectedResult.ShowOption);
            actualResult.SortOrder.Should().Be(expectedResult.SortOrder);
            actualResult.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            actualResult.CreatedOn.Should().Be(expectedResult.CreatedOn);
            actualResult.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            actualResult.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
