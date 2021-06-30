using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.QualificationAchieved
{
    public class When_GetSingleOrDefault_Is_Called : BaseTest<Domain.Models.QualificationAchieved>
    {
        private Domain.Models.QualificationAchieved _result;
        private Domain.Models.QualificationAchieved _data;

        public override void Given()
        {
            _data = new QualificationAchievedBuilder().Build();
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
            _result.TqRegistrationProfileId.Should().Be(_data.TqRegistrationProfileId);
            _result.QualificationId.Should().Be(_data.QualificationId);
            _result.QualificationGradeId.Should().Be(_data.QualificationGradeId);
            _result.IsAchieved.Should().Be(_data.IsAchieved);
            _result.CreatedBy.Should().Be(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().Be(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
