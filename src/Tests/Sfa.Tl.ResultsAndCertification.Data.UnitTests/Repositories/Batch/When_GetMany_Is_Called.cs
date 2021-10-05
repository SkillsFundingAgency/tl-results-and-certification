using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.Batch
{
    public class When_GetMany_Is_Called : BaseTest<Domain.Models.Batch>
    {
        private IEnumerable<Domain.Models.Batch> _result;
        private IList<Domain.Models.Batch> _data;        

        public override void Given()
        {
            _data = new BatchBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetManyAsync().ToListAsync();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Records_Are_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_First_Record_Fields_Have_Expected_Values()
        {
            var testData = _data.FirstOrDefault();
            var result = _result.FirstOrDefault();
            testData.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Type.Should().Be(testData.Type);
            result.Status.Should().Be(testData.Status);
            result.Errors.Should().Be(testData.Errors);
            result.PrintingStatus.Should().Be(testData.PrintingStatus);
            result.RunOn.Should().Be(testData.RunOn);
            result.StatusChangedOn.Should().Be(testData.StatusChangedOn);
            result.ResponseStatus.Should().Be(testData.ResponseStatus);
            result.ResponseMessage.Should().Be(testData.ResponseMessage);
            result.CreatedBy.Should().Be(Constants.CreatedByUser);
            result.CreatedOn.Should().Be(Constants.CreatedOn);
            result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
