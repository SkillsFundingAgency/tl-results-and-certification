using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeUlnGet
{
    public class When_Cache_Is_Available : TestSetup
    {
        private AdminChangeUlnViewModel CachedModel;

        public override void Given()
        {
            CachedModel = Entry();
            CacheService.GetAsync<AdminChangeUlnViewModel>(CacheKey).Returns(CachedModel);
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            var result = Result.Should().BeOfType<ViewResult>().Which;
            result.Model.Should().BeSameAs(CachedModel);
            await AdminDashboardLoader.DidNotReceive().GetAdminLearnerRecordAsync<AdminChangeUlnViewModel>(Arg.Any<int>());
        }
    }
}
