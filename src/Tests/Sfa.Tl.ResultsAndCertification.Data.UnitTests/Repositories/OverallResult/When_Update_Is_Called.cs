using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.OverallResult
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.OverallResult>
    {
        private Domain.Models.OverallResult _result;
        private Domain.Models.OverallResult _data;
        private const string ModifiedUserName = "Modified User";

        public override void Given()
        {
            _data = new OverallResultBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            _data.Details = "New Text";
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
            _result.TqRegistrationPathwayId.Should().Be(_data.TqRegistrationPathwayId);
            _result.Details.Should().Be(_data.Details);
            _result.ResultAwarded.Should().Be(_data.ResultAwarded);
            _result.CalculationStatus.Should().Be(_data.CalculationStatus);
            _result.PublishDate.Should().Be(_data.PublishDate);
            _result.PrintAvailableFrom.Should().Be(_data.PrintAvailableFrom);
            _result.StartDate.Should().Be(_data.StartDate);
            _result.EndDate.Should().Be(_data.EndDate);
            _result.CreatedBy.Should().Be(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
