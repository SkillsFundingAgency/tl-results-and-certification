using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationLearnersNameGet
{
    public class When_NoCache_Found : TestSetup
    {
        private RegistrationViewModel cacheResult;
        private UlnViewModel _ulnViewModel;

        public override void Given()
        {
            _ulnViewModel = new UlnViewModel { Uln = "1234567890" };

            cacheResult = new RegistrationViewModel
            {
                Uln = _ulnViewModel
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(LearnersNameViewModel));

            var model = viewResult.Model as LearnersNameViewModel;
            model.Should().NotBeNull();
            model.Firstname.Should().BeNullOrEmpty();
            model.Lastname.Should().BeNullOrEmpty();
        }
    }
}
