using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.AddSpecialismResultGet
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private ManageSpecialismResultViewModel _mockresult = null;
        private Dictionary<string, string> _routeAttributes;
        private List<LookupViewModel> grades;

        public override void Given()
        {
            grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "C1", Value = "V1" }, new LookupViewModel { Id = 2, Code = "C2", Value = "V2" } };
            _mockresult = new ManageSpecialismResultViewModel
            {
                ProfileId = 1,
                SpecialismName = "Specialism",
                SpecialismDisplayName = "Specialism (7654321)",
                AssessmentSeries = "Summer 2021",
                AssessmentId = 11,
                ResultId = null,
                SelectedGradeCode = string.Empty,
                AppealEndDate = DateTime.Now.AddDays(7),
                Grades = grades
            };

            _routeAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } };
            ResultLoader.GetManageSpecialismResultAsync(AoUkprn, ProfileId, AssessmentId, false).Returns(_mockresult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ManageSpecialismResultViewModel));

            var model = viewResult.Model as ManageSpecialismResultViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_mockresult.ProfileId);
            model.SpecialismName.Should().Be(_mockresult.SpecialismName);
            model.SpecialismDisplayName.Should().Be(_mockresult.SpecialismDisplayName);
            model.AssessmentId.Should().Be(_mockresult.AssessmentId);
            model.AssessmentSeries.Should().Be(_mockresult.AssessmentSeries);
            model.ResultId.Should().Be(_mockresult.ResultId);
            model.SelectedGradeCode.Should().Be(_mockresult.SelectedGradeCode);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.ResultDetails);
            model.BackLink.RouteAttributes.Should().BeEquivalentTo(_routeAttributes);
        }
    }
}
