﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ChangeSpecialismResultGet
{
    public class When_Result_Has_PrsStatus_BeingAppealed : TestSetup
    {
        private ManageSpecialismResultViewModel _mockresult = null;
        private List<LookupViewModel> grades;

        public override void Given()
        {
            grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "C1", Value = "V1" }, new LookupViewModel { Id = 2, Code = "C2", Value = "V2" } };
            _mockresult = new ManageSpecialismResultViewModel
            {
                ProfileId = 1,
                SpecialismDisplayName = "Specialism (7654321)",
                SpecialismName = "Specialism",
                AssessmentSeries = "Summer 2022",
                AssessmentId = 11,
                ResultId = 111,
                SelectedGradeCode = string.Empty,
                Grades = grades,
                ResultEndDate = DateTime.Today.AddDays(7),
                PrsStatus = Common.Enum.PrsStatus.BeingAppealed
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
