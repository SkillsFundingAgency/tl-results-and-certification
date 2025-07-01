using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesMathsStatusPost
{
    public class When_Failed : TestSetup
    {
        private AdminChangeMathsResultsViewModel _cacheModel;

        public override void Given()
        {
            var isSuccess = false;
            ViewModel = CreateViewModel(SubjectStatus.Achieved);

            _cacheModel = new AdminChangeMathsResultsViewModel
            {
                RegistrationPathwayId = 1,
                MathsStatusTo = SubjectStatus.Achieved
            };

            CacheService.GetAsync<AdminChangeMathsResultsViewModel>(CacheKey).Returns(_cacheModel);
            AdminDashboardLoader.ProcessChangeMathsStatusAsync(ViewModel).Returns(isSuccess);
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            AdminDashboardLoader.Received(1).ProcessChangeMathsStatusAsync(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = ActualResult as RedirectToActionResult;
            routeName.ActionName.Should().Be(RouteConstants.ProblemWithService);
        }
    }
}