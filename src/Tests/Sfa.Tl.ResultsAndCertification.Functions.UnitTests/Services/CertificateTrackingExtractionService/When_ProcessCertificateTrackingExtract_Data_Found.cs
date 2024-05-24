using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.CertificateTrackingExtractionService
{
    public class When_ProcessCertificateTrackingExtract_Data_Found : TestSetup
    {
        private readonly DateTime _fromDay = new(2024, 1, 1);
        private readonly string _fileName = "file.txt";

        private FunctionResponse _result;

        public override void Given()
        {
            var learningDetails = new LearningDetails
            {
                TLevelTitle = "T Level in Onsite Construction"
            };

            var printCertificate = new PrintCertificate
            {
                Type = PrintCertificateType.Certificate,
                LearningDetails = JsonConvert.SerializeObject(learningDetails),
                TqRegistrationPathway = new TqRegistrationPathway
                {
                    TqRegistrationProfile = new TqRegistrationProfile
                    {
                        UniqueLearnerNumber = 1111111111,
                        Firstname = "Peter",
                        Lastname = "Williams"
                    }
                },
                PrintBatchItem = new PrintBatchItem
                {
                    TrackingId = "1234567890-ABC",
                    BatchId = 10,
                    Status = PrintingBatchItemStatus.AwaitingCollectionByCourier,
                    SignedForBy = "Anna Smith",
                    StatusChangedOn = new DateTime(2023, 5, 10),
                    Batch = new Batch
                    {
                        Type = BatchType.Printing,
                        Status = BatchStatus.Created
                    }
                }
            };

            Repository.GetCertificateTrackingDataAsync(Arg.Any<Func<DateTime>>()).Returns(new List<PrintCertificate> { printCertificate });
        }

        public override async Task When()
        {
            _result = await Service.ProcessCertificateTrackingExtractAsync(() => _fromDay, () => _fileName);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            Repository.Received(1).GetCertificateTrackingDataAsync(Arg.Any<Func<DateTime>>());

            BlobStorageService.Received(1).UploadFromByteArrayAsync(Arg.Is<BlobStorageData>(d =>
                d.ContainerName == DocumentType.CertificateTracking.ToString()
                && d.SourceFilePath == Common.Helpers.Constants.CertificateTrackingExtractsFolder
                && d.BlobFileName == _fileName
                && d.UserName == Common.Helpers.Constants.FunctionPerformedBy));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.IsSuccess.Should().BeTrue();
            _result.Message.Should().BeNull();
        }
    }
}