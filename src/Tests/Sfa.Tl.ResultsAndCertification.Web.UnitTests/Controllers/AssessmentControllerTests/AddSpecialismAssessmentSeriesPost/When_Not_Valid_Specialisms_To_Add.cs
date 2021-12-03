using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AddSpecialismAssessmentSeriesPost
{
    public class When_Not_Valid_Specialisms_To_Add : TestSetup
    {
        private AddSpecialismAssessmentEntryViewModel _mockresult = null;

        public override void Given()
        {
            ViewModel = new AddSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2022",
                IsOpted = true,
                SpecialismId = 5,
                SpecialismDetails = new List<SpecialismViewModel>
                {
                    new SpecialismViewModel
                    {
                        Id = 5,
                        LarId = "ZT2158963",
                        Name = "Specialism Name1",
                        DisplayName = "Specialism Name1 (ZT2158963)",
                        Assessments = new List<SpecialismAssessmentViewModel>()
                    }
                }
            };

            _mockresult = new AddSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2022",
                SpecialismDetails = new List<SpecialismViewModel>
                {
                    new SpecialismViewModel
                    {
                        Id = 1,
                        LarId = "ZT2158777",
                        Name = "Specialism Name",
                        DisplayName = "Specialism Name (ZT2158777)",
                        Assessments = new List<SpecialismAssessmentViewModel>()
                    }
                }
            };

            AssessmentLoader.GetAddAssessmentEntryAsync<AddSpecialismAssessmentEntryViewModel>(AoUkprn, ProfileId, ComponentType.Specialism).Returns(_mockresult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
