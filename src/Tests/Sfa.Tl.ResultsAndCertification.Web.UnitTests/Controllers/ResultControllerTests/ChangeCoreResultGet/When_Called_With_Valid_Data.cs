using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ChangeCoreResultGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private ManageCoreResultViewModel _mockResult = null;
        private Dictionary<string, string> _routeAttributes;
        private List<LookupViewModel> grades;

        public override void Given()
        {
            grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "C1", Value = "V1" }, new LookupViewModel { Id = 2, Code = "C2", Value = "V2" } };
            _mockResult = new ManageCoreResultViewModel
            {
                ProfileId = 1,
                PathwayDisplayName = "Pathway (7654321)",
                AssessmentSeries = "Summer 2021",
                AppealEndDate = DateTime.Today.AddDays(7),
                AssessmentId = 11,
                ResultId = 111,
                SelectedGradeCode = string.Empty,
                Grades = grades
            };

            _routeAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } };
            ResultLoader.GetManageCoreResultAsync(AoUkprn, ProfileId, AssessmentId, true).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).GetManageCoreResultAsync(AoUkprn, ProfileId, AssessmentId, true);
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

            model.ProfileId.Should().Be(_mockResult.ProfileId);
            model.PathwayDisplayName.Should().Be(_mockResult.PathwayDisplayName);
            model.AssessmentId.Should().Be(_mockResult.AssessmentId);
            model.AssessmentSeries.Should().Be(_mockResult.AssessmentSeries);
            model.AppealEndDate.Should().Be(_mockResult.AppealEndDate);
            model.ResultId.Should().Be(_mockResult.ResultId);
            model.SelectedGradeCode.Should().Be(_mockResult.SelectedGradeCode);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.ResultDetails);
            model.BackLink.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);
        }
    }
}
