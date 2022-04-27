using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.IpLookup
{
    public class When_GetSingleOrDefault_Is_Called : BaseTest<Domain.Models.IpLookup>
    {
        private Domain.Models.IpLookup _result;
        private Domain.Models.IpLookup _data;

        public override void Given()
        {
            _data = new IpLookupBuilder().Build();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetSingleOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TlLookupId.Should().Be(_data.TlLookupId);
            _result.Name.Should().Be(_data.Name);
            _result.StartDate.Should().Be(_data.StartDate);
            _result.EndDate.Should().Be(_data.EndDate);
            _result.ShowOption.Should().Be(_data.ShowOption);
            _result.SortOrder.Should().Be(_data.SortOrder);
            _result.CreatedBy.Should().Be(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
