using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.UcasDataTransferService.ProcessUcasEntries
{
    public class When_GetUcasEntries_IsNull : TestBase
    {
        private readonly UcasData _mockUcasData = null;

        public override void Given()
        {
            UcasDataType = UcasDataType.Entries;
            UcasDataService.GetUcasEntriesAsync(UcasDataType).Returns(_mockUcasData);
        }

        [Fact]
        public void Then_Expected_Response_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsSuccess.Should().BeTrue();
            ActualResult.Message.Should().Be("No entries are found. Method: GetUcasEntriesAsync()");
        }
    }
}
