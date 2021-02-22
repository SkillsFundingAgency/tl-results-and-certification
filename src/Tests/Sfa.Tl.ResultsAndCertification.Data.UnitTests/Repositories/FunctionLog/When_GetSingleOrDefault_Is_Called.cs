using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.FunctionLog
{
    public class When_GetSingleOrDefault_Is_Called : BaseTest<Domain.Models.FunctionLog>
    {
        private Domain.Models.FunctionLog _result;
        private Domain.Models.FunctionLog _data;

        public override void Given()
        {
            _data = new FunctionLogBuilder().Build();
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
            _result.Name.Should().Be(_data.Name);
            _result.StartDate.Should().Be(_data.StartDate);
            _result.EndDate.Should().Be(_data.EndDate);
            _result.Status.Should().Be(_data.Status);
            _result.Message.Should().Be(_data.Message);
            _result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
