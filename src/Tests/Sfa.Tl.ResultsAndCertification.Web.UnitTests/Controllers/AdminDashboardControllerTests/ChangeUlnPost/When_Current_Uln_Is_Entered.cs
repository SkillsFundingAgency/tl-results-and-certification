using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeUlnPost
{
    public class When_Current_Uln_Is_Entered : TestSetup
    {
        public override void Given()
        {
            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminChangeUlnViewModel>(3).Returns(Entry());
            Model = Entry();
            Model.Uln = 1111111111;
            Model.UlnTo = "9999999990";
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            var result = Result.Should().BeOfType<ViewResult>().Which;
            var model = result.Model.Should().BeOfType<AdminChangeUlnViewModel>().Which;
            model.Uln.Should().Be(9999999990);
            model.UlnTo.Should().Be("9999999990");
            Controller.ModelState.IsValid.Should().BeFalse();
            Controller.ModelState["UlnTo"].Errors[0].ErrorMessage.Should().Be("Enter a ULN that is different from the current ULN");
            await AdminDashboardLoader.DidNotReceive().IsLearnerUlnAvailableAsync(Arg.Any<long>());
            await CacheService.DidNotReceive().SetAsync(Arg.Any<string>(), Arg.Any<AdminChangeUlnViewModel>(), Arg.Any<CacheExpiryTime>());
        }
    }
}
