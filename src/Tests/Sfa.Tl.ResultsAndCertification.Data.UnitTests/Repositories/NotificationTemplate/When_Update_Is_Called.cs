using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Threading.Tasks;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.NotificationTemplate
{
    public class When_Update_Is_Called : BaseTest<Domain.Models.NotificationTemplate>
    {
        private Domain.Models.NotificationTemplate _result;
        private Domain.Models.NotificationTemplate _data;
        private const string UpdateTemplateName = "Template Name Updated";
        private const string ModifiedUserName = "Modified User";

        public override void Given()
        {
            _data = new NotificationTemplateBuilder().Build();
            DbContext.Add(_data);
            DbContext.SaveChanges();

            _data.TemplateName = UpdateTemplateName;
            _data.ModifiedOn = DateTime.UtcNow;
            _data.ModifiedBy = ModifiedUserName;
        }

        public async override Task When()
        {
            await Repository.UpdateAsync(_data);
            _result = await Repository.GetSingleOrDefaultAsync(x => x.Id == 1);
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TemplateId.Should().Be(_data.TemplateId);
            _result.TemplateName.Should().Be(_data.TemplateName);
            _result.CreatedBy.Should().Be(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
