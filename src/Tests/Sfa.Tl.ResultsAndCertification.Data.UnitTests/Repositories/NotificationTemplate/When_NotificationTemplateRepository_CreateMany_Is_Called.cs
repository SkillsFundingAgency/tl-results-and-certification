using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.NotificationTemplate
{
    public class When_NotificationTemplateRepository_CreateMany_Is_Called : BaseTest<Domain.Models.NotificationTemplate>
    {
        private int _result;
        private IList<Domain.Models.NotificationTemplate> _data;

        public override void Given()
        {
            _data = new NotificationTemplateBuilder().BuildList();
        }

        public async override Task When()
        {
            _result = await Repository.CreateManyAsync(_data);
        }

        [Fact]
        public void Then_Two_Records_Should_Have_Been_Created() =>
            _result.Should().Be(2);
    }
}
