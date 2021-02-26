using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TlLookup
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.TlLookup>
    {
        private Domain.Models.TlLookup _result;
        private Domain.Models.TlLookup _data;
        private const string ModifiedUserName = "Modified User";

        public override void Given()
        {
            _data = new TlLookupBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            _data.Category = "xyz";
            _data.ModifiedOn = DateTime.UtcNow;
            _data.ModifiedBy = ModifiedUserName;
        }

        public async override Task When()
        {
            await Repository.UpdateAsync(_data);
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
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
