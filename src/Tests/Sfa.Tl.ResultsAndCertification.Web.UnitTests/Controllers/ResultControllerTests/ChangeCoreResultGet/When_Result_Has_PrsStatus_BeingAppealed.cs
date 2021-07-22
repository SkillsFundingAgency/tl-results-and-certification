using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ChangeCoreResultGet
{
    public class When_Result_Has_PrsStatus_BeingAppealed : TestSetup
    {
        private ManageCoreResultViewModel _mockresult = null;
        private List<LookupViewModel> grades;

        public override void Given()
        {
            grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "C1", Value = "V1" }, new LookupViewModel { Id = 2, Code = "C2", Value = "V2" } };
            _mockresult = new ManageCoreResultViewModel
            {
                ProfileId = 1,
                PathwayDisplayName = "Pathway (7654321)",
                AssessmentSeries = "Summer 2021",
                AssessmentId = 11,
                ResultId = 111,
                SelectedGradeCode = string.Empty,
                Grades = grades,
                PathwayPrsStatus = Common.Enum.PrsStatus.BeingAppealed
            };

            ResultLoader.GetManageCoreResultAsync(AoUkprn, ProfileId, AssessmentId, true).Returns(_mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).GetManageCoreResultAsync(AoUkprn, ProfileId, AssessmentId, true);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
