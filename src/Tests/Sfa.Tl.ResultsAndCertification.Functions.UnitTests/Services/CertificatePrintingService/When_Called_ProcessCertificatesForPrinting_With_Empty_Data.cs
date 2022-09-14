using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.CertificatePrintingService
{
    public class When_Called_ProcessCertificatesForPrinting_With_Empty_Data : TestSetup
    {
        private List<CertificateResponse> _actualResults;
        private CertificateResponse _expectedResult;

        public override void Given()
        {
            _expectedResult = new CertificateResponse
            {
                IsSuccess = true,
                Message = "No learners data retrieved to process certificates for printing. Method: ProcessCertificatesForPrintingAsync()",
                BatchId = 0,
                ProvidersCount = 0,
                CertificatesCreated = 0,
                OverallResultsUpdatedCount = 0
            };
            var expectedResults = new List<CertificateResponse> { _expectedResult };

            CertificateService.ProcessCertificatesForPrintingAsync().Returns(expectedResults);
        }

        public async override Task When()
        {
            _actualResults = await Service.ProcessCertificatesForPrintingAsync();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CertificateService.Received(1).ProcessCertificatesForPrintingAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResults.Should().NotBeNull();
            _actualResults.Should().HaveCount(1);

            var actualResult = _actualResults.First();
            actualResult.IsSuccess.Should().BeTrue();
            actualResult.Message.Should().Be(_expectedResult.Message);
            actualResult.BatchId.Should().Be(_expectedResult.BatchId);
            actualResult.ProvidersCount.Should().Be(_expectedResult.ProvidersCount);
            actualResult.CertificatesCreated.Should().Be(_expectedResult.CertificatesCreated);
            actualResult.OverallResultsUpdatedCount.Should().Be(_expectedResult.OverallResultsUpdatedCount);
        }
    }
}