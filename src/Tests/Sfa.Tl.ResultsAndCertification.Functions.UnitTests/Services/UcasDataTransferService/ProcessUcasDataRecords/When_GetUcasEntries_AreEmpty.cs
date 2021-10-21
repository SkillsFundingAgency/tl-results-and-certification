using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.UcasDataTransferService.ProcessUcasDataRecords
{
    public class When_GetUcasEntries_AreEmpty : TestBase
    {
        private UcasData _mockUcasData;

        public override void Given()
        {
            _mockUcasData = new UcasData
            {
                Header = new UcasDataHeader(),
                Trailer = new UcasDataTrailer(),
                UcasDataRecords = new List<UcasDataRecord>()
            };

            UcasDataService.GetUcasEntriesAsync(UcasDataType.Entries).Returns(_mockUcasData);
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
