using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_GetCertificateStatus_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private CertificateStatus? _actualResult;

        public override void Given()
        {
            CreateService();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(CalculationStatus calculationStatus)
        {
            await Task.CompletedTask;
            _actualResult = OverallResultCalculationService.GetCertificateStatus(calculationStatus);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(CalculationStatus calculationStatus, CertificateStatus? expectedResult)
        {
            await WhenAsync(calculationStatus);

            _actualResult.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { CalculationStatus.Unclassified, null },
                    new object[] { CalculationStatus.NoResult, null },
                    new object[] { CalculationStatus.PartiallyCompletedRommRaised, null },
                    new object[] { CalculationStatus.PartiallyCompletedAppealRaised, null },
                    new object[] { CalculationStatus.CompletedRommRaised, null },
                    new object[] { CalculationStatus.CompletedAppealRaised, null },
                    new object[] { CalculationStatus.PartiallyCompleted, CertificateStatus.AwaitingProcessing },
                    new object[] { CalculationStatus.Completed, CertificateStatus.AwaitingProcessing }
                };
            }
        }
    }
}
