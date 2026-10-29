using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeUlnGet
{
    public class When_Cache_Is_Available : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminChangeUlnViewModel>(CacheKey).Returns(Entry());
            var learner = Learner();
            learner.UlnTo = null;
            AdminDashboardLoader.GetAdminLearnerRecordAsync<ReviewChangeUlnViewModel>(3).Returns(learner);
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            var result = Result.Should().BeOfType<ViewResult>().Which;
            var model = result.Model.Should().BeOfType<ReviewChangeUlnViewModel>().Which;
            model.UlnTo.Should().Be("9999999991");
            model.Uln.Should().Be(9999999990);
            await CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<ReviewChangeUlnViewModel>(), Arg.Any<CacheExpiryTime>());
        }
    }
}
