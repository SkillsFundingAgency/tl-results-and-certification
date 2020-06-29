using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationProfile
{
    public class When_TqRegistrationProfileRepository_GetFirstOrDefault_Is_Called : BaseTest<Domain.Models.TqRegistrationProfile>
    {
        private Domain.Models.TqRegistrationProfile _result;
        private Domain.Models.TqRegistrationProfile _data;

        public override void Given()
        {
            _data = new TqRegistrationProfileBuilder().Build();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public override void When()
        {
            _result = Repository.GetFirstOrDefaultAsync(x => x.Id == 1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.UniqueLearnerNumber.Should().Be(_data.UniqueLearnerNumber);
            _result.Firstname.Should().Be(_data.Firstname);
            _result.Lastname.Should().Be(_data.Lastname);
            _result.DateofBirth.Should().Be(_data.DateofBirth);
            _result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
