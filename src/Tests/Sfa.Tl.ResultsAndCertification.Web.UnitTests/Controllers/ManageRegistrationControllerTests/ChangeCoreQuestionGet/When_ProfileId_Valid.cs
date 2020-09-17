using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeCoreQuestionGet
{
    public class When_ProfileId_Valid : TestSetup
    {
        private ChangeCoreProviderDetailsViewModel cacheResult;
        private ChangeCoreQuestionViewModel mockresult = null;

        public override void Given()
        {
            cacheResult = new ChangeCoreProviderDetailsViewModel
            {
                ProviderDisplayName = "Test (12345678)"
            };

            CacheService.GetAndRemoveAsync<ChangeCoreProviderDetailsViewModel>(CacheKey).Returns(cacheResult);

            mockresult = new ChangeCoreQuestionViewModel
            {
                ProfileId = 1,
                CoreDisplayName = "Test core (987654321)"
            };
            RegistrationLoader.GetRegistrationChangeCoreQuestionDetailsAsync(AoUkprn, ProfileId).Returns(mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).GetRegistrationChangeCoreQuestionDetailsAsync(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeCoreQuestionViewModel));

            var model = viewResult.Model as ChangeCoreQuestionViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.CoreDisplayName.Should().Be(mockresult.CoreDisplayName);
            model.ProviderDisplayName.Should().Be(cacheResult.ProviderDisplayName);

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.ChangeRegistrationProvider);
            backLink.RouteAttributes.Count.Should().Be(1);
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(mockresult.ProfileId.ToString());
        }
    }
}
