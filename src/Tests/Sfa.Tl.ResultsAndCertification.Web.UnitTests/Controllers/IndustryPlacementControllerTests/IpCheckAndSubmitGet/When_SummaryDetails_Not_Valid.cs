using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCheckAndSubmitGet
{
    public class When_SummaryDetails_Not_Valid : TestSetup
    {
        private IndustryPlacementViewModel _cacheModel;
        private IpCheckAndSubmitViewModel _learnerDetails;
        private IpTempFlexNavigation _tempFlexNavigation;
        private (List<SummaryItemModel>, bool) _summaryDetailsList;

        public override void Given()
        {
            // Cache object
            _cacheModel = new IndustryPlacementViewModel { IpCompletion = new IpCompletionViewModel { ProfileId = 1, PathwayId = 11, AcademicYear = 2020 } };
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheModel);

            // LearnerDetails
            _learnerDetails = new IpCheckAndSubmitViewModel();
            IndustryPlacementLoader.GetLearnerRecordDetailsAsync<IpCheckAndSubmitViewModel>(ProviderUkprn, _cacheModel.IpCompletion.ProfileId).Returns(_learnerDetails);

            // TempFlexNavigation
            _tempFlexNavigation = new IpTempFlexNavigation();
            IndustryPlacementLoader.GetTempFlexNavigationAsync(_cacheModel.IpCompletion.PathwayId, _cacheModel.IpCompletion.AcademicYear).Returns(_tempFlexNavigation);

            // SummaryDetails 
            _summaryDetailsList = (new List<SummaryItemModel>(), false); // isValid flag
            IndustryPlacementLoader.GetIpSummaryDetailsListAsync(_cacheModel, _tempFlexNavigation).Returns(_summaryDetailsList);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.Received(1).GetLearnerRecordDetailsAsync<IpCheckAndSubmitViewModel>(ProviderUkprn, _cacheModel.IpCompletion.ProfileId);
            IndustryPlacementLoader.Received(1).GetTempFlexNavigationAsync(_cacheModel.IpCompletion.PathwayId, _cacheModel.IpCompletion.AcademicYear);
            IndustryPlacementLoader.Received(1).GetIpSummaryDetailsListAsync(_cacheModel, _tempFlexNavigation);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
