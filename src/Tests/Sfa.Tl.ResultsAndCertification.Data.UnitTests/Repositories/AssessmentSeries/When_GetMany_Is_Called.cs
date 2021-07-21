using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.AssessmentSeries
{
    public class When_GetMany_Is_Called : BaseTest<Domain.Models.AssessmentSeries>
    {
        private IEnumerable<Domain.Models.AssessmentSeries> _result;
        private IList<Domain.Models.AssessmentSeries> _data;

        public override void Given()
        {
            _data = new AssessmentSeriesBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetManyAsync().ToListAsync();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Records_Are_Returned() =>
            _result.Count().Should().Be(_data.Count);

        [Fact]
        public void Then_First_Path_Fields_Have_Expected_Values()
        {
            var testData = _data.FirstOrDefault();
            var result = _result.FirstOrDefault();
            testData.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be(testData.Name);
            result.Description.Should().Be(testData.Description);
            result.Year.Should().Be(testData.Year);
            result.StartDate.Should().Be(testData.StartDate);
            result.EndDate.Should().Be(testData.EndDate);
            result.AppealEndDate.Should().Be(testData.AppealEndDate);
            result.CreatedBy.Should().Be(Constants.CreatedByUser);
            result.CreatedOn.Should().Be(Constants.CreatedOn);
            result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
