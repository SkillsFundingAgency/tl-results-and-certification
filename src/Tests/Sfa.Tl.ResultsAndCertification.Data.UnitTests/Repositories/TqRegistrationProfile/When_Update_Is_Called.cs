using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationProfile
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.TqRegistrationProfile>
    {
        private Domain.Models.TqRegistrationProfile _result;
        private Domain.Models.TqRegistrationProfile _data;

        private const string ModifiedBy = "Modified User Updated";

        public override void Given()
        {
            _data = new TqRegistrationProfileBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            // Update data
            _data.Firstname = "First 11";
            _data.Lastname = "Last 11";
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
            _result.Firstname.Should().Be(_data.Firstname);
            _result.Lastname.Should().Be(_data.Lastname);
            _result.Gender.Should().Be(_data.Gender);
            _result.DateofBirth.Should().Be(_data.DateofBirth);
            _result.IsLearnerVerified.Should().Be(_data.IsLearnerVerified);
            _result.IsEnglishAndMathsAchieved.Should().Be(_data.IsEnglishAndMathsAchieved);
            _result.IsSendLearner.Should().Be(_data.IsSendLearner);
            _result.IsRcFeed.Should().Be(_data.IsRcFeed);
            _result.CreatedBy.Should().Be(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
