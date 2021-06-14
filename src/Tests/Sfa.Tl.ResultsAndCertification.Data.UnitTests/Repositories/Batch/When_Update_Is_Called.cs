using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Batch
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.Batch>
    {
        private Domain.Models.Batch _result;
        private Domain.Models.Batch _data;
        private const string ModifiedUserName = "Modified User";

        public override void Given()
        {
            _data = new BatchBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            _data.Status = BatchStatus.Accepted;
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
            _result.Type.Should().Be(_data.Type);
            _result.Status.Should().Be(_data.Status);
            _result.CreatedBy.Should().Be(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
