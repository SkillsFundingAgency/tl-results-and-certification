using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCompletionGet
{
    public class When_Cache_Found : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;

        public override void Given()
        {
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted };
            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.DidNotReceive().GetLearnerRecordDetailsAsync<IpCompletionViewModel>(Arg.Any<long>(), Arg.Any<int>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as IpCompletionViewModel;

            model.Should().NotBeNull();
            model.ProfileId.Should().Be(_cacheResult.IpCompletion.ProfileId);
            model.PathwayId.Should().Be(_cacheResult.IpCompletion.PathwayId);
            model.AcademicYear.Should().Be(_cacheResult.IpCompletion.AcademicYear);
            model.LearnerName.Should().Be(_cacheResult.IpCompletion.LearnerName);
            model.IndustryPlacementStatus.Should().Be(_cacheResult.IpCompletion.IndustryPlacementStatus);
            model.IsValid.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.LearnerRecordDetails);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdRouteValue);
            profileIdRouteValue.Should().Be(_cacheResult.IpCompletion.ProfileId.ToString());
        }
    }
}
