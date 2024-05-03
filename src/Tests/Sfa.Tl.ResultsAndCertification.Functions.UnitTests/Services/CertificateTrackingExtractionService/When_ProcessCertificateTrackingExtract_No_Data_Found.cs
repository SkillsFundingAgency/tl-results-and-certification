using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.CertificateTrackingExtractionService
{
    public class When_ProcessCertificateTrackingExtract_No_Data_Found : TestSetup
    {
        private const string Message = "No entries are found. Method: ProcessCertificateTrackingExtractAsync()";

        private readonly DateTime _fromDay = new(2024, 1, 1);
        private FunctionResponse _result;

        public override void Given()
        {
            Repository.GetCertificateTrackingDataAsync(Arg.Any<Func<DateTime>>()).Returns(new List<PrintCertificate>());
        }

        public override async Task When()
        {
            _result = await Service.ProcessCertificateTrackingExtractAsync(() => _fromDay, () => string.Empty);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            Repository.Received(1).GetCertificateTrackingDataAsync(Arg.Any<Func<DateTime>>());
            Logger.Received(1).LogWarning(LogEvent.NoDataFound, Message);
            BlobStorageService.DidNotReceive().UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.IsSuccess.Should().BeTrue();
            _result.Message.Should().Be(Message);
        }
    }
}