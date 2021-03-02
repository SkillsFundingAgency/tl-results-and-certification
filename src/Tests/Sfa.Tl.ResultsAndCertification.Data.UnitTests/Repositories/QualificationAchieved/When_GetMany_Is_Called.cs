using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.QualificationAchieved
{
    public class When_GetMany_Is_Called : BaseTest<Domain.Models.QualificationAchieved>
    {
        private IEnumerable<Domain.Models.QualificationAchieved> _result;
        private IList<Domain.Models.QualificationAchieved> _data;

        public override void Given()
        {
            _data = new QualificationAchievedBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public async override Task When()
        {
            _result = await Repository.GetManyAsync().ToListAsync();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Records_Is_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_First_Record_Fields_Have_Expected_Values()
        {
            var testData = _data.FirstOrDefault();
            var result = _result.FirstOrDefault();
            testData.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.TqRegistrationProfileId.Should().Be(testData.TqRegistrationProfileId);
            result.QualificationId.Should().Be(testData.QualificationId);
            result.QualificationGradeId.Should().Be(testData.QualificationGradeId);
            result.IsAchieved.Should().Be(testData.IsAchieved);
            result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            result.CreatedOn.Should().Be(Constants.CreatedOn);
            result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
