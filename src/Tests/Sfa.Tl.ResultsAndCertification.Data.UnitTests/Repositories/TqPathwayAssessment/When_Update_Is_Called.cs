using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqPathwayAssessment
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.TqPathwayAssessment>
    {
        private Domain.Models.TqPathwayAssessment _result;
        private Domain.Models.TqPathwayAssessment _data;

        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            _data = new TqPathwayAssessmentBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
            _data.EndDate = DateTime.UtcNow;
            _data.IsOptedin = false;
            _data.ModifiedBy = ModifiedBy;
        }

        public async override Task When()
        {
            await Repository.UpdateAsync(_data);
            _result = await Repository.GetFirstOrDefaultAsync(x => x.Id == _data.Id);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TqRegistrationPathwayId.Should().Be(_data.TqRegistrationPathwayId);
            _result.AssessmentSeriesId.Should().Be(_data.AssessmentSeriesId);
            _result.StartDate.Should().Be(_data.StartDate);
            _result.IsOptedin.Should().Be(_data.IsOptedin);
            _result.IsBulkUpload.Should().Be(_data.IsBulkUpload);
            _result.CreatedBy.Should().Be(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
