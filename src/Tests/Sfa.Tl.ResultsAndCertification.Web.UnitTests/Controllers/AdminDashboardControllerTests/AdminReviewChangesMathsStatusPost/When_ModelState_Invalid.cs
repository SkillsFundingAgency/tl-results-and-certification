using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesMathsStatusPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private AdminChangeMathsResultsViewModel _cacheModel;
        private const string ErrorKey = "AdminReviewChangesLevelTwoMaths";

        public override void Given()
        {
            ViewModel = CreateViewModel(SubjectStatus.Achieved);

            _cacheModel = new AdminChangeMathsResultsViewModel
            {
                RegistrationPathwayId = 1,
                MathsStatusTo = SubjectStatus.Achieved
            };

            Controller.ModelState.AddModelError(ErrorKey, ReviewChangesMathsStatus.Validation_Contact_Name_Blank_Text);
            CacheService.GetAsync<AdminChangeMathsResultsViewModel>(CacheKey).Returns(_cacheModel);
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            CacheService.Received(1).GetAsync<AdminChangeMathsResultsViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var model = ActualResult.ShouldBeViewResult<AdminReviewChangesMathsSubjectViewModel>();

            model.Should().NotBeNull();
        }
    }
}