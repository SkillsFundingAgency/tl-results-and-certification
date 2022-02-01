using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqSpecialismResult
{
    public class When_GetMany_Is_Called : BaseTest<Domain.Models.TqSpecialismResult>
    {
        private IEnumerable<Domain.Models.TqSpecialismResult> _result;
        private IList<Domain.Models.TqSpecialismResult> _data;

        public override void Given()
        {
            _data = new TqSpecialismResultBuilder().BuildList();
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
        public void Then_First_Record_Fields_Have_Expected_Values()
        {
            var testData = _data.FirstOrDefault();
            var result = _result.FirstOrDefault();
            testData.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.TqSpecialismAssessmentId.Should().Be(testData.TqSpecialismAssessmentId);
            result.TlLookupId.Should().Be(testData.TlLookupId);
            result.StartDate.Should().Be(testData.StartDate);
            result.PrsStatus.Should().Be(testData.PrsStatus);
            result.IsOptedin.Should().Be(testData.IsOptedin);
            result.IsBulkUpload.Should().Be(testData.IsBulkUpload);
            result.CreatedBy.Should().Be(Constants.CreatedByUser);
            result.CreatedOn.Should().Be(Constants.CreatedOn);
            result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
