using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderRegistrations;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderRegistrationsControllerTests.DownloadRegistrationsData
{
    public class When_Returns_Years : TestSetup
    {
        private readonly List<AvailableStartYearViewModel> _years = new()
        {
            new()
            {
                Year = 2020,
                DisplayYear = "2020 to 2021"
            },
            new()
            {
                Year = 2021,
                DisplayYear = "2021 to 2022"
            }
        };

        public override void Given()
        {
            ProviderRegistrationsLoader.GetAvailableStartYearsAsync().Returns(_years);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewModel = Result.ShouldBeViewResult<DownloadRegistrationsDataViewModel>();

            viewModel.Should().NotBeNull();
            viewModel.AvailableStartYearViewModels.Should().BeEquivalentTo(_years);
        }
    }
}