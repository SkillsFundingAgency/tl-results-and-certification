using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpMultiEmployerOtherGet
{
    public class When_NoCache_Found_For_IpMultiEmployerOther : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpModelUsedViewModel _ipModelUsedViewModel;
        private IpMultiEmployerUsedViewModel _ipMultiEmployerUsedViewModel;
        private IpMultiEmployerOtherViewModel _ipMultiEmployerOtherViewModel;

        public override void Given()
        {
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed };
            _ipModelUsedViewModel = new IpModelUsedViewModel { IsIpModelUsed = true };
            _ipMultiEmployerUsedViewModel = new IpMultiEmployerUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName, IsMultiEmployerModelUsed = true };

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                IpModelViewModel = new IpModelViewModel { IpModelUsed = _ipModelUsedViewModel, IpMultiEmployerUsed = _ipMultiEmployerUsedViewModel }
            };

            
            _ipMultiEmployerOtherViewModel = new IpMultiEmployerOtherViewModel 
            { 
                LearnerName = _ipCompletionViewModel.LearnerName,
                OtherIpPlacementModels = new List<IpLookupDataViewModel> 
                { 
                    new IpLookupDataViewModel { Id = 1, Name = "Test", IsSelected = false } 
                }
            };
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);
            IndustryPlacementLoader.GetIpLookupDataAsync<IpMultiEmployerOtherViewModel>(IpLookupType.IndustryPlacementModel, _ipCompletionViewModel.LearnerName, _ipCompletionViewModel.PathwayId, true).Returns(_ipMultiEmployerOtherViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.Received(1).GetIpLookupDataAsync<IpMultiEmployerOtherViewModel>(Arg.Any<IpLookupType>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<bool>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IpMultiEmployerOtherViewModel));

            var model = viewResult.Model as IpMultiEmployerOtherViewModel;
            model.Should().NotBeNull();
            model.LearnerName.Should().Be(_ipCompletionViewModel.LearnerName);
            model.OtherIpPlacementModels.Should().BeEquivalentTo(_ipMultiEmployerOtherViewModel.OtherIpPlacementModels);
            model.IsChangeMode.Should().BeFalse();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpMultiEmployerUsed);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
