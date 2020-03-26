using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.TqProvider
{
    public class When_TqProviderRepository_Create_Is_Called : BaseTest<Domain.Models.TqProvider>
    {
        private int _result;
        private Domain.Models.TqProvider _data;

        public override void Given()
        {
            _data = new TqProviderBuilder().Build();

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
