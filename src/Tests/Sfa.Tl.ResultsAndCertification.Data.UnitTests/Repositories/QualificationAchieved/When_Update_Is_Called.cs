using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.QualificationAchieved
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.QualificationAchieved>
    {
        private Domain.Models.QualificationAchieved _result;
        private Domain.Models.QualificationAchieved _data;
        private const string ModifiedUserName = "Modified User";

        public override void Given()
        {
            _data = new QualificationAchievedBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            _data.IsAchieved = false;
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
            _result.TqRegistrationProfileId.Should().Be(_data.TqRegistrationProfileId);
            _result.QualificationId.Should().Be(_data.QualificationId);
            _result.QualificationGradeId.Should().Be(_data.QualificationGradeId);
            _result.IsAchieved.Should().Be(_data.IsAchieved);
            _result.CreatedBy.Should().Be(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
