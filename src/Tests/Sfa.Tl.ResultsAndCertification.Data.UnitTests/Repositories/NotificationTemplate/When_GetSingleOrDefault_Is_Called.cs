using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.NotificationTemplate
{
    public class When_GetSingleOrDefault_Is_Called : BaseTest<Domain.Models.NotificationTemplate>
    {
        private Domain.Models.NotificationTemplate _result;
        private Domain.Models.NotificationTemplate _data;

        public override void Given()
        {
            _data = new NotificationTemplateBuilder().Build();
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
            _result.TemplateId.Should().Be(_data.TemplateId);
            _result.TemplateName.Should().BeEquivalentTo(_data.TemplateName);
            _result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            _result.CreatedOn.Should().Be(Constants.CreatedOn);
            _result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            _result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
