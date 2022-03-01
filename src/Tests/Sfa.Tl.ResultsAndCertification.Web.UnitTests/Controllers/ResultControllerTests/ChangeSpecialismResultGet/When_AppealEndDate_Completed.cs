using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ChangeSpecialismResultGet
{
    public class When_AppealEndDate_Completed : TestSetup
    {
        private ManageSpecialismResultViewModel _mockresult = null;

        public override void Given()
        {
            _mockresult = new ManageSpecialismResultViewModel
            {
                AppealEndDate = DateTime.Today.AddDays(-7),
                PrsStatus = null
            };

            ResultLoader.GetManageSpecialismResultAsync(AoUkprn, ProfileId, AssessmentId, true).Returns(_mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).GetManageSpecialismResultAsync(AoUkprn, ProfileId, AssessmentId, true);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
