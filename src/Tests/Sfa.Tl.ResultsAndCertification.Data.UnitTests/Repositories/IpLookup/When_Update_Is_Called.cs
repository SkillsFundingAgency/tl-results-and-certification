using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.IpLookup
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.IpLookup>
    {
        private Domain.Models.IpLookup _result;
        private Domain.Models.IpLookup _data;
        private const string ModifiedUserName = "Modified User";

        public override void Given()
        {
            _data = new IpLookupBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            _data.EndDate = DateTime.Today;
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
            _result.TlLookupId.Should().Be(_data.TlLookupId);
            _result.Name.Should().Be(_data.Name);
            _result.StartDate.Should().Be(_data.StartDate);
            _result.EndDate.Should().Be(_data.EndDate);
            _result.ShowOption.Should().Be(_data.ShowOption);
            _result.SortOrder.Should().Be(_data.SortOrder);
            _result.CreatedBy.Should().Be(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
