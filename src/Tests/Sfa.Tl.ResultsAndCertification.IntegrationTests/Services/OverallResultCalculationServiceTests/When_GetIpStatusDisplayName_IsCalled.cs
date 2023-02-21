using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_GetIpStatusDisplayName_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private string _actualResult;

        public override void Given()
        {
            // Dependencies
            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
            {
                OverallResultBatchSettings = new OverallResultBatchSettings
                {
                    BatchSize = 10,
                    NoOfAcademicYearsToProcess = 4
                }
            };

            CreateService();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(IndustryPlacementStatus ipStatus)
        {
            await Task.CompletedTask;
            _actualResult = OverallResultCalculationService.GetIndustryPlacementStatusDisplayName(ipStatus);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(IndustryPlacementStatus ipStatus, string ipStatusDisplayName)
        {
            await WhenAsync(ipStatus);

            _actualResult.Should().Be(ipStatusDisplayName);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { IndustryPlacementStatus.Completed, "Completed" },
                    new object[] { IndustryPlacementStatus.CompletedWithSpecialConsideration, "Completed with special consideration" },
                    new object[] { IndustryPlacementStatus.NotCompleted, "Not completed" },
                    new object[] { IndustryPlacementStatus.WillNotComplete, "Will not complete" },
                    new object[] { IndustryPlacementStatus.NotSpecified, "Not completed" },
                };
            }
        }
    }
}
