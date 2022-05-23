using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpModelUsedPost
{
    public class When_IsChangeMode_With_Option_NotChanged : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;

        public override void Given()
        {
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed };
            ViewModel = new IpModelUsedViewModel { IsIpModelUsed = true, IsChangeMode = true };

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                IpModelViewModel = new IpModelViewModel
                {
                    IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = true },
                    IpMultiEmployerUsed = new IpMultiEmployerUsedViewModel { IsMultiEmployerModelUsed = false },
                    IpMultiEmployerSelect = new IpMultiEmployerSelectViewModel { PlacementModels = new List<IpLookupDataViewModel> { new IpLookupDataViewModel { Name = "Item1", IsSelected = true } } }
                }
            };

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
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.IpCheckAndSubmit);
        }
    }
}