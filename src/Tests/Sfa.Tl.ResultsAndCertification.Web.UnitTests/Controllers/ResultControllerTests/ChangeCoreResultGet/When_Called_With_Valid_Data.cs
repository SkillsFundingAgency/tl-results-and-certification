﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ChangeCoreResultGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private ManageCoreResultViewModel mockresult = null;
        private Dictionary<string, string> _routeAttributes;
        private List<LookupViewModel> grades;

        public override void Given()
        {
            grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "C1", Value = "V1" }, new LookupViewModel { Id = 2, Code = "C2", Value = "V2" } };
            mockresult = new ManageCoreResultViewModel
            {
                ProfileId = 1,
                PathwayDisplayName = "Pathway (7654321)",
                AssessmentSeries = "Summer 2021",
                AssessmentId = 11,
                ResultId = 111,
                SelectedGradeCode = string.Empty,
                Grades = grades
            };

            _routeAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } };
            ResultLoader.GetManageCoreResultAsync(AoUkprn, ProfileId, AssessmentId, true).Returns(mockresult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ManageCoreResultViewModel));

            var model = viewResult.Model as ManageCoreResultViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(mockresult.ProfileId);
            model.PathwayDisplayName.Should().Be(mockresult.PathwayDisplayName);
            model.AssessmentId.Should().Be(mockresult.AssessmentId);
            model.AssessmentSeries.Should().Be(mockresult.AssessmentSeries);
            model.ResultId.Should().Be(mockresult.ResultId);
            model.SelectedGradeCode.Should().Be(mockresult.SelectedGradeCode);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.ResultDetails);
            model.BackLink.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);
        }
    }
}
