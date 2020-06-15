using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories.DocumentUploadHistory
{
    public class When_DocumentUploadHistoryRepository_Create_Is_Called : BaseTest<Domain.Models.DocumentUploadHistory>
    {
        private int _result;
        private Domain.Models.DocumentUploadHistory _data;

        public override void Given()
        {
            _data = new DocumentUploadHistoryBuilder().Build();

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
