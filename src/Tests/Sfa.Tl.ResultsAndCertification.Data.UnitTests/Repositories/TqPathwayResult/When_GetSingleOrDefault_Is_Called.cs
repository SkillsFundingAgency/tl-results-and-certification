using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqPathwayResult
{
    public class When_GetSingleOrDefault_Is_Called : BaseTest<Domain.Models.TqPathwayResult>
    {
        private Domain.Models.TqPathwayResult _result;
        private Domain.Models.TqPathwayResult _data;

        public override void Given()
        {
            _data = new TqPathwayResultBuilder().Build();
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
            _result.TqPathwayAssessmentId.Should().Be(_data.TqPathwayAssessmentId);
            _result.TlLookupId.Should().Be(_data.TlLookupId);
            _result.StartDate.Should().Be(_data.StartDate);
            _result.PrsStatus.Should().Be(_data.PrsStatus);
            _result.IsOptedin.Should().Be(_data.IsOptedin);
            _result.IsBulkUpload.Should().Be(_data.IsBulkUpload);
            _result.CreatedBy.Should().Be(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
