﻿using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.RemoveAssessmentEntrySpecialismPost
{
    public class When_Answer_Is_No : TestSetup
    {
        private AdminRemoveSpecialismAssessmentEntryViewModel _viewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel(doYouWantToRemoveThisAssessmentEntry: false);
        }

        public async override Task When()
        {
            Result = await Controller.RemoveAssessmentEntrySpecialismAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).RemoveAsync<AdminRemoveSpecialismAssessmentEntryViewModel>(CacheKey);
            CacheService.DidNotReceive().SetAsync(CacheKey, _viewModel);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectToRouteResult(RouteConstants.AdminLearnerRecord, (Constants.PathwayId, RegistrationPathwayId));
        }
    }
}