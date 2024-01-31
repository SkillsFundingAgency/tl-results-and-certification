using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeIndustryPlacementPost
{
    public class When_Failed : TestSetup
    {

        private AdminChangeIpViewModel _cacheModel;


        public override void Given()
        {
            var isSuccess = false;            
            ViewModel = CreateViewModel(IndustryPlacementStatus.Completed);
            _cacheModel = new AdminChangeIpViewModel()
            {
                AdminIpCompletion = new AdminIpCompletionViewModel()
                {
                    RegistrationPathwayId = 1,
                    IndustryPlacementStatusTo = IndustryPlacementStatus.Completed

                }
            };

            CacheService.GetAsync<AdminChangeIpViewModel>(CacheKey).Returns(_cacheModel);
            AdminDashboardLoader.ProcessChangeIndustryPlacementAsync(ViewModel).Returns(isSuccess);

        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            AdminDashboardLoader.Received(1).ProcessChangeIndustryPlacementAsync(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = ActualResult as RedirectToActionResult;
            routeName.ActionName.Should().Be(RouteConstants.ProblemWithService);
        }

    }
}

