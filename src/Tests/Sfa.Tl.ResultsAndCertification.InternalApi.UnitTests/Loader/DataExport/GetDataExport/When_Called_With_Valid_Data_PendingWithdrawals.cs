using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.DataExport.GetDataExport
{
    public class When_Called_With_Valid_Data_PendingWithdrawals : TestSetup
    {
        public override void Given()
        {
            DataExportType = DataExportType.PendingWithdrawals;

            IList<PendingWithdrawalsExport> pendingWithdrawals = new List<PendingWithdrawalsExport>
            {
                new()
                {
                    Uln = 1111111111,
                    FirstName = "John",
                    LastName = "Smith",
                    DateOfBirth = new DateTime(2000, 1, 1),
                    Ukprn = 10000536,
                    AcademicYear = 2022,
                    Core = "60358300",
                    SpecialismsList = new List<string>{ "ZTLOS001" },
                    CreatedOn = new DateTime(2024, 1, 1)
                }
            };

            DataExportRepository.GetDataExportPendingWithdrawalsAsync(AoUkprn).Returns(pendingWithdrawals);
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            Response.Should().HaveCount(1);

            Response[0].IsDataFound.Should().BeTrue();
            Response[0].ComponentType.Should().Be(ComponentType.NotSpecified);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            DataExportRepository.Received(1).GetDataExportPendingWithdrawalsAsync(AoUkprn);
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Is<BlobStorageData>(b =>
                b.ContainerName == DocumentType.DataExports.ToString()
                && b.SourceFilePath == $"{AoUkprn}/{DataExportType.PendingWithdrawals}"
                && b.BlobFileName.EndsWith($".{FileType.Csv}")
                && b.UserName == RequestedBy));
        }
    }
}