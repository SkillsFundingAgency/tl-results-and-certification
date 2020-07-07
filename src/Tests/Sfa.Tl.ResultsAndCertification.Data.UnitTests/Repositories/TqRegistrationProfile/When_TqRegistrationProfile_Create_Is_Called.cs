using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationProfile
{
    public class When_TqRegistrationProfile_Create_Is_Called : BaseTest<Domain.Models.TqRegistrationProfile>
    {
        private int _result;
        private Domain.Models.TqRegistrationProfile _data;

        public override void Given()
        {
            _data = new TqRegistrationProfileBuilder().Build();

        }
        public override void When()
        {
            _result = Repository.CreateAsync(_data).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_One_Record_Should_Have_Been_Created() =>
                _result.Should().Be(1);
    }
}
