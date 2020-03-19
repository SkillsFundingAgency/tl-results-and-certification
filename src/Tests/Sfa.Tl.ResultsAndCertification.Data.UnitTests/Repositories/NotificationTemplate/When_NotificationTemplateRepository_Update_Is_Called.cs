using System;
using Xunit;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.NotificationTemplate
{
    public class When_NotificationTemplateRepository_Update_Is_Called : BaseTest<Domain.Models.NotificationTemplate>
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

        public override void When()
        {
            Repository.UpdateAsync(_data).GetAwaiter().GetResult();
            _result = Repository.GetSingleOrDefaultAsync(x => x.Id == 1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Fields_Are_As_Expected()
        {
            _data.Should().NotBeNull();
            _result.Should().NotBeNull();
            _result.Id.Should().Be(1);
            _result.TemplateId.Should().Be(_data.TemplateId);
            _result.TemplateName.Should().BeEquivalentTo(_data.TemplateName);
            _result.CreatedBy.Should().BeEquivalentTo(_data.CreatedBy);
            _result.CreatedOn.Should().Be(_data.CreatedOn);
            _result.ModifiedBy.Should().Be(_data.ModifiedBy);
            _result.ModifiedOn.Should().Be(_data.ModifiedOn);
        }
    }
}
