using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpMultiEmployerUsedPost
{
    public class When_IsChangeMode_With_Option_NotChanged : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpModelUsedViewModel _ipModelUsedViewModel;

        public override void Given()
        {
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed };
            _ipModelUsedViewModel = new IpModelUsedViewModel { IsIpModelUsed = true };

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                IpModelViewModel = new IpModelViewModel 
                { 
                    IpModelUsed = _ipModelUsedViewModel, 
                    IpMultiEmployerUsed = new IpMultiEmployerUsedViewModel { IsMultiEmployerModelUsed = true },
                    IpMultiEmployerOther = new IpMultiEmployerOtherViewModel { OtherIpPlacementModels = new List<IpLookupDataViewModel> { new IpLookupDataViewModel { Name = "Item 1", IsSelected = true } } }
                },
            };

            ViewModel = new IpMultiEmployerUsedViewModel { IsMultiEmployerModelUsed = true, IsChangeMode = true };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<IndustryPlacementViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_IpCheckAndSubmit()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.IpCheckAndSubmit);
        }
    }
}
