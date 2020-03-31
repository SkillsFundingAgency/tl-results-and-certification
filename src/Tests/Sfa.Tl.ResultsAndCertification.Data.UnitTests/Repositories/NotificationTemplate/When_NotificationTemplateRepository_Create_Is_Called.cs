using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.NotificationTemplate
{
    public class When_NotificationTemplateRepository_Create_Is_Called : BaseTest<Domain.Models.NotificationTemplate>
    {
        private int _result;
        private Domain.Models.NotificationTemplate _data;

        public override void Given()
        {
            _data = new NotificationTemplateBuilder().Build();

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
