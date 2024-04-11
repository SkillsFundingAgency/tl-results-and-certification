using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenSpecialismAppealGet
{
    public class When_Cache_Empty_And_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            AdminOpenSpecialismAppealViewModel nullModel = null;

            CacheService.GetAsync<AdminOpenSpecialismAppealViewModel>(CacheKey).Returns(nullModel);
            AdminPostResultsLoader.GetAdminOpenSpecialismAppealAsync(RegistrationPathwayId, SpecialismAssessmentId).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminOpenSpecialismAppealViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminOpenSpecialismAppealAsync(RegistrationPathwayId, SpecialismAssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}