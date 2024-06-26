﻿using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminChangeSpecialismResultPost
{
    public class When_Grade_Selected : TestSetup
    {
        private AdminChangeSpecialismResultViewModel _viewModel;

        public override void Given()
        {
            _viewModel = CreateViewModel(selectedGradeId: 1);
        }

        public async override Task When()
        {
            Result = await Controller.AdminChangeSpecialismResultAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).LoadAdminChangeSpecialismResultGrades(_viewModel);
            CacheService.Received(1).SetAsync(CacheKey, _viewModel);
        }

        [Fact]
        public void Then_Redirected_To_ReviewChangeSpecialismResult()
        {
            Result.ShouldBeRedirectToRouteResult(RouteConstants.AdminChangeSpecialismResultReviewChanges);
        }
    }
}