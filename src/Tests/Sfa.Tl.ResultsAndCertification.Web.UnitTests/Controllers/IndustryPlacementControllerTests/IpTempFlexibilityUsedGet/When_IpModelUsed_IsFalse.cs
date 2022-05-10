using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpTempFlexibilityUsedGet
{
    public class When_IpModelUsed_IsFalse : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpTempFlexibilityUsedViewModel _ipTempFlexibilityUsedViewModel;
        private IpTempFlexNavigation _ipTempFlexNavigation;

        public override void Given()
        {

            // Cache object
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = IndustryPlacementStatus.Completed };
            _ipTempFlexibilityUsedViewModel = new IpTempFlexibilityUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName };
            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                IpModelViewModel = new IpModelViewModel
                {
                    IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = false },
                    IpMultiEmployerUsed = new IpMultiEmployerUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName },
                    IpMultiEmployerOther = new IpMultiEmployerOtherViewModel { LearnerName = _ipCompletionViewModel.LearnerName },
                    IpMultiEmployerSelect = new IpMultiEmployerSelectViewModel { LearnerName = _ipCompletionViewModel.LearnerName }
                },
                TempFlexibility = new IpTempFlexibilityViewModel { IpTempFlexibilityUsed = _ipTempFlexibilityUsedViewModel }
            };
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);

            // Loader navigation
            _ipTempFlexNavigation = new IpTempFlexNavigation { AskBlendedPlacement = true, AskTempFlexibility = true };
            IndustryPlacementLoader.GetTempFlexNavigationAsync(_cacheResult.IpCompletion.PathwayId, _cacheResult.IpCompletion.AcademicYear).Returns(_ipTempFlexNavigation);
        }

        [Fact]
        public void Then_MultiEmp_CacheData_Is_Cleared()
        {
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<IndustryPlacementViewModel>(x =>
            x.IpModelViewModel.IpModelUsed.IsIpModelUsed == false &&
            x.IpModelViewModel.IpMultiEmployerUsed == null &&
            x.IpModelViewModel.IpMultiEmployerOther == null &&
            x.IpModelViewModel.IpMultiEmployerSelect == null));
        }
    }
}
