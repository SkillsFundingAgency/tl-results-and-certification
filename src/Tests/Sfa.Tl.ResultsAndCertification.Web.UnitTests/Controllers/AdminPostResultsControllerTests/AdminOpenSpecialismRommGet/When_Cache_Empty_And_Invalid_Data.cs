using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenSpecialismRommGet
{
    public class When_Cache_Empty_And_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            AdminOpenSpecialismRommViewModel nullModel = null;

            CacheService.GetAsync<AdminOpenSpecialismRommViewModel>(CacheKey).Returns(nullModel);
            AdminPostResultsLoader.GetAdminOpenSpecialismRommAsync(RegistrationPathwayId, SpecialismAssessmentId).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminOpenSpecialismRommViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminOpenSpecialismRommAsync(RegistrationPathwayId, SpecialismAssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}