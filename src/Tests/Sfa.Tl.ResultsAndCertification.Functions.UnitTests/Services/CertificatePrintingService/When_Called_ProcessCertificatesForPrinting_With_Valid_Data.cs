using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.CertificatePrintingService
{
    public class When_Called_ProcessCertificatesForPrinting_With_Valid_Data : TestSetup
    {
        private List<CertificateResponse> _actualResults;
        private List<CertificateResponse> _expectedResults;

        public override void Given()
        {
            _expectedResults = new List<CertificateResponse>
            {
                new CertificateResponse
                {
                    IsSuccess = true,
                    BatchId = 1,
                    ProvidersCount = 1,
                    CertificatesCreated = 10,
                    OverallResultsUpdatedCount = 10
                },
                new CertificateResponse
                {
                    IsSuccess = true,
                    BatchId = 2,
                    ProvidersCount = 2,
                    CertificatesCreated = 20,
                    OverallResultsUpdatedCount = 20
                }
            };

            CertificateService.ProcessCertificatesForPrintingAsync().Returns(_expectedResults);
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
            _actualResults.Should().HaveCount(2);

            foreach (var (result, index) in _actualResults.Select((value, i) => (value, i)))
            {
                result.IsSuccess.Should().BeTrue();
                result.Message.Should().Be(_expectedResults[index].Message);
                result.BatchId.Should().Be(_expectedResults[index].BatchId);
                result.ProvidersCount.Should().Be(_expectedResults[index].ProvidersCount);
                result.CertificatesCreated.Should().Be(_expectedResults[index].CertificatesCreated);
                result.OverallResultsUpdatedCount.Should().Be(_expectedResults[index].OverallResultsUpdatedCount);
            }
        }
    }
}