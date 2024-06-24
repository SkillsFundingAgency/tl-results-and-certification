using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderRegistrations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderRegistrationsLoaderTests
{
    public class When_GetAvailableStartYears_IsCalled : ProviderRegistrationsLoaderBaseTest
    {
        private IList<AvailableStartYearViewModel> _expectedResult;
        private IList<AvailableStartYearViewModel> _actualResult;

        public override void Given()
        {
            ApiClient.GetProviderRegistrationsAvailableStartYearsAsync().Returns(new List<int> { 2020, 2021, 2022 });

            _expectedResult = new List<AvailableStartYearViewModel>
            {
                new() { Year = 2020, DisplayYear = "2020 to 2021" },
                new() { Year = 2021, DisplayYear = "2021 to 2022" },
                new() { Year = 2022, DisplayYear = "2022 to 2023" }
            };
        }

        public override async Task When()
        {
            _actualResult = await Loader.GetAvailableStartYearsAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}