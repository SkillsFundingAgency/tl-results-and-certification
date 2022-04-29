using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpModelUsedGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private IpModelUsedViewModel _ipModelUsedViewModel;

        public override void Given()
        {
            ProfileId = 1;

            _ipModelUsedViewModel = new IpModelUsedViewModel { ProfileId = ProfileId, LearnerName = "Test Test"};

            IndustryPlacementLoader.GetLearnerRecordDetailsAsync<IpModelUsedViewModel>(ProviderUkprn, ProfileId).Returns(_ipModelUsedViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.Received(1).GetLearnerRecordDetailsAsync<IpModelUsedViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as IpModelUsedViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_ipModelUsedViewModel.ProfileId);
            model.LearnerName.Should().Be(_ipModelUsedViewModel.LearnerName);
            model.IsIpModelUsed.Should().BeNull();
            model.IsValid.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpCompletion);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(ProfileId.ToString());
        }
    }
}
