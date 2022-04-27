using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.IpAchieved
{
    public class When_Create_Is_Called : BaseTest<Domain.Models.IpAchieved>
    {
        private Domain.Models.IpAchieved _data;
        private int _result;

        public override void Given()
        {
            _data = new IpAchievedBuilder().Build();
        }

        public async override Task When()
        {
            _result = await Repository.CreateAsync(_data);
        }

        [Fact]
        public void Then_Expected_Records_Should_Have_Been_Created() => _result.Should().Be(1);
    }
}
