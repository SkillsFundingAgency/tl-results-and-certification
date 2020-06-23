using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.NotificationTemplate
{
    public class When_NotificationTemplateRepository_GetMany_Is_Called : BaseTest<Domain.Models.NotificationTemplate>
    {
        private IEnumerable<Domain.Models.NotificationTemplate> _result;
        private IList<Domain.Models.NotificationTemplate> _data;

        public override void Given()
        {
            _data = new NotificationTemplateBuilder().BuildList();
            DbContext.AddRange(_data);
            DbContext.SaveChanges();
        }

        public override void When()
        {
            _result = Repository.GetManyAsync().ToList();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Paths_Is_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_First_Path_Fields_Have_Expected_Values()
        {
            var testData = _data.FirstOrDefault();
            var result = _result.FirstOrDefault();
            testData.Should().NotBeNull();
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.TemplateId.Should().Be(testData.TemplateId);
            result.TemplateName.Should().BeEquivalentTo(testData.TemplateName);
            result.CreatedBy.Should().BeEquivalentTo(Constants.CreatedByUser);
            result.CreatedOn.Should().Be(Constants.CreatedOn);
            result.ModifiedBy.Should().BeEquivalentTo(Constants.ModifiedByUser);
            result.ModifiedOn.Should().Be(Constants.ModifiedOn);
        }
    }
}
