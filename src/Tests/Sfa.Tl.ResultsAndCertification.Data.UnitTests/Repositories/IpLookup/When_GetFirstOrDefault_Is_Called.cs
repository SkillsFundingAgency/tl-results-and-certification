using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.IpLookup
{
    public class When_GetFirstOrDefault_Is_Called : BaseTest<Domain.Models.IpLookup>
    {
        private Domain.Models.IpLookup _result;
        private IEnumerable<Domain.Models.IpLookup> _data;

        public override void Given()
        {
            _data = new IpLookupBuilder().BuildList();
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
            _result.TlLookupId.Should().Be(expectedResult.TlLookupId);
            _result.Name.Should().Be(expectedResult.Name);
            _result.StartDate.Should().Be(expectedResult.StartDate);
            _result.EndDate.Should().Be(expectedResult.EndDate);
            _result.ShowOption.Should().Be(expectedResult.ShowOption);
            _result.SortOrder.Should().Be(expectedResult.SortOrder);
            _result.CreatedBy.Should().BeEquivalentTo(expectedResult.CreatedBy);
            _result.CreatedOn.Should().Be(expectedResult.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(expectedResult.ModifiedBy);
            _result.ModifiedOn.Should().Be(expectedResult.ModifiedOn);
        }
    }
}
