using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.FunctionLog
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.FunctionLog>
    {
        private Domain.Models.FunctionLog _result;
        private Domain.Models.FunctionLog _data;
        private const string ModifiedUserName = "Modified User";

        public override void Given()
        {
            _data = new FunctionLogBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            _data.Name = "Test";
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
            _result.Name.Should().Be(_data.Name);
            _result.StartDate.Should().Be(_data.StartDate);
            _result.EndDate.Should().Be(_data.EndDate);
            _result.Status.Should().Be(_data.Status);
            _result.Message.Should().Be(_data.Message);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
