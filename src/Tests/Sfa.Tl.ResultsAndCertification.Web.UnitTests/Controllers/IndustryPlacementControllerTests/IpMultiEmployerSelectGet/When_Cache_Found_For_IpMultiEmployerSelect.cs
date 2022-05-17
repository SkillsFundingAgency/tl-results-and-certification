using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpMultiEmployerSelectGet
{
    public class When_Cache_Found_For_IpMultiEmployerSelect : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpModelUsedViewModel _ipModelUsedViewModel;
        private IpMultiEmployerUsedViewModel _ipMultiEmployerUsedViewModel;
        private IpMultiEmployerSelectViewModel _ipMultiEmployerSelectViewModel;

        public override void Given()
        {
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed };
            _ipModelUsedViewModel = new IpModelUsedViewModel { ProfileId = 1, IsIpModelUsed = true };
            _ipMultiEmployerUsedViewModel = new IpMultiEmployerUsedViewModel { LearnerName = "First Last", IsMultiEmployerModelUsed = false };
            _ipMultiEmployerSelectViewModel = new IpMultiEmployerSelectViewModel
            {
                LearnerName = "First Last",
                PlacementModels = new List<IpLookupDataViewModel>
                {
                    new IpLookupDataViewModel { Id = 1, Name = "Test", IsSelected = true },
                    new IpLookupDataViewModel { Id = 2, Name = "New", IsSelected = true }
                }
            };

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                IpModelViewModel = new IpModelViewModel
                {
                    IpModelUsed = _ipModelUsedViewModel,
                    IpMultiEmployerUsed = _ipMultiEmployerUsedViewModel,
                    IpMultiEmployerSelect = _ipMultiEmployerSelectViewModel
                }
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<IndustryPlacementViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IpMultiEmployerSelectViewModel));
            var model = viewResult.Model as IpMultiEmployerSelectViewModel;

            model.Should().NotBeNull();
            model.LearnerName.Should().Be(_ipMultiEmployerSelectViewModel.LearnerName);
            model.PlacementModels.Should().BeEquivalentTo(_ipMultiEmployerSelectViewModel.PlacementModels);
            model.IsChangeMode.Should().BeFalse();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpMultiEmployerUsed);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
