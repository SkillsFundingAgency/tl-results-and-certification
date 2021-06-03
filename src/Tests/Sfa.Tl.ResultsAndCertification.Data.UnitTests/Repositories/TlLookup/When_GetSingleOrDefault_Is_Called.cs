using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TlLookup
{
    public class When_GetSingleOrDefault_Is_Called : BaseTest<Domain.Models.TlLookup>
    {
        private Domain.Models.TlLookup _result;
        private Domain.Models.TlLookup _data;

        public override void Given()
        {
            _data = new TlLookupBuilder().Build();
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
            _result.Category.Should().Be(_data.Category);
            _result.Code.Should().Be(_data.Code);
            _result.Value.Should().Be(_data.Value);
            _result.IsActive.Should().Be(_data.IsActive);
            _result.SortOrder.Should().Be(_data.SortOrder);
            _result.CreatedBy.Should().Be(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
