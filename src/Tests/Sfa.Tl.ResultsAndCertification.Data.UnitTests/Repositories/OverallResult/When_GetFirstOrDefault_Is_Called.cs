using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.OverallResult
{
    public class When_GetFirstOrDefault_Is_Called : BaseTest<Domain.Models.OverallResult>
    {
        private Domain.Models.OverallResult _result;
        private Domain.Models.OverallResult _data;

        public override void Given()
        {
            _data = new OverallResultBuilder().Build();
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
            _result.CreatedBy.Should().Be(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
