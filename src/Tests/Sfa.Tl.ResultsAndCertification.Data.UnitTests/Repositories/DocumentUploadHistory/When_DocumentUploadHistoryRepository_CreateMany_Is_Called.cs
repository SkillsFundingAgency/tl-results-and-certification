using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.DocumentUploadHistory
{
    public class When_DocumentUploadHistoryRepository_CreateMany_Is_Called : BaseTest<Domain.Models.DocumentUploadHistory>
    {
        private int _result;
        private IList<Domain.Models.DocumentUploadHistory> _data;

        public override void Given()
        {
            _data = new DocumentUploadHistoryBuilder().BuildList();
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
