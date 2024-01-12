﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System.Threading.Tasks;
using Xunit;
namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementGet
{
    public class When_Called_With_Invalid_Data: TestSetup
    {
        protected AdminIpCompletionViewModel Mockresult = null;

        public override void Given()
        {
            RegistrationPathwayId = 0;
            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminIpCompletionViewModel>(RegistrationPathwayId).Returns(Mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<AdminIpCompletionViewModel>(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
