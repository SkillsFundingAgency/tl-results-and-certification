using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.ProviderRegistrationsServiceTests
{
    public class When_GetAvailableStartYearsAsync_Is_Called : ProviderRegistrationsServiceBaseTest
    {
        private IList<int> _expectedResult;
        private IList<int> _actualResult;

        public override void Given()
        {
            _expectedResult = new List<int> { 2021, 2022 };
            ProviderRegistrationsService.GetAvailableStartYearsAsync(Arg.Any<Func<DateTime>>()).Returns(_expectedResult);
        }

        public override async Task When()
        {
            _actualResult = await ProviderRegistrationsService.GetAvailableStartYearsAsync(() => new DateTime(2023, 1, 1));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}