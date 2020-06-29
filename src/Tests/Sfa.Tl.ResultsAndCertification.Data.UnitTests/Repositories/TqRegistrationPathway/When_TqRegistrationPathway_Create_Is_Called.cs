using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;


namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqRegistrationPathway
{
    public class When_TqRegistrationPathway_Create_Is_Called : BaseTest<Domain.Models.TqRegistrationPathway>
    {
        private int _result;
        private Domain.Models.TqRegistrationPathway _data;

        public override void Given()
        {
            _data = new TqRegistrationPathwayBuilder().Build();

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
