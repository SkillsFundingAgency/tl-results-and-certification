using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Strategies;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_GetOverallSpecialismResult_IsCalled : OverallResultCalculationServiceBaseTest
    {
        public override void Given()
        {
            TlLookup = TlLookupDataProvider.CreateFullTlLookupList(DbContext);
            CreateService();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        [Theory]
        [InlineData(1, typeof(SingleSpecialismResultStrategy))]
        [InlineData(2, typeof(DualSpecialismResultStrategy))]
        public async Task Then_Correct_SpecialismResultStrategy_Is_Returned(int numberOfSpecialisms, Type expectedStrategy)
        {
            ISpecialismResultStrategy strategy = await SpecialismResultStrategyFactory.GetSpecialismResultStrategyAsync(TlLookup, numberOfSpecialisms);
            strategy.Should().BeOfType(expectedStrategy);
        }

        [Fact]
        public async Task If_TlLookup_Collection_Null_Then_ArgumentNullException_Is_Thrown()
        {
            Func<Task<ISpecialismResultStrategy>> getStrategy = () => SpecialismResultStrategyFactory.GetSpecialismResultStrategyAsync(null, 1);

            await getStrategy.Should()
                .ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("tlLookup");
        }

        [Fact]
        public async Task If_TlLookup_Collection_Empty_Then_ArgumentException_Is_Thrown()
        {
            Func<Task<ISpecialismResultStrategy>> getStrategy = () => SpecialismResultStrategyFactory.GetSpecialismResultStrategyAsync(Enumerable.Empty<TlLookup>(), 1);

            await getStrategy.Should()
                .ThrowExactlyAsync<ArgumentException>()
                .WithParameterName("tlLookup");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(10)]
        public async Task If_Number_Of_Specialisms_Invalid_Then_ArgumentException_Is_Thrown(int numberOfSpecialisms)
        {
            Func<Task<ISpecialismResultStrategy>> getStrategy = () => SpecialismResultStrategyFactory.GetSpecialismResultStrategyAsync(TlLookup, numberOfSpecialisms);

            await getStrategy.Should()
                .ThrowExactlyAsync<ArgumentException>()
                .WithParameterName("numberOfSpecialisms");
        }
    }
}