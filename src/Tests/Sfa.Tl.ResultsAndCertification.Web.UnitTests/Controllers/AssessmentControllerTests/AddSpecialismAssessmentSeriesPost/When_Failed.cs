using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AddSpecialismAssessmentSeriesPost
{
    public class When_Failed : TestSetup
    {
        private AddAssessmentEntryResponse AddAssessmentEntryResponse;
        private AddSpecialismAssessmentEntryViewModel _mockresult = null;

        public override void Given()
        {
            ViewModel = new AddSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2022",
                IsOpted = true,
                SpecialismsId = "5",
                SpecialismDetails = new List<SpecialismViewModel>
                {
                    new SpecialismViewModel
                    {
                        Id = 5,
                        LarId = "ZT2158963",
                        Name = "Specialism Name1",
                        Assessments = new List<SpecialismAssessmentViewModel>()
                    }
                }
            };

            _mockresult = new AddSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2022",
                SpecialismsId = "5",
                SpecialismDetails = new List<SpecialismViewModel>
                {
                    new SpecialismViewModel
                    {
                        Id = 5,
                        LarId = "ZT2158963",
                        Name = "Specialism Name1",
                        Assessments = new List<SpecialismAssessmentViewModel>()
                    }
                }
            };

            ComponentIds = string.Join(Constants.PipeSeperator, _mockresult.SpecialismDetails.Select(s => s.Id));
            AddAssessmentEntryResponse = new AddAssessmentEntryResponse { IsSuccess = false };
            AssessmentLoader.GetAddAssessmentEntryAsync<AddSpecialismAssessmentEntryViewModel>(AoUkprn, ProfileId, ComponentType.Specialism, ComponentIds).Returns(_mockresult);
            AssessmentLoader.AddSpecialismAssessmentEntryAsync(AoUkprn, _mockresult).Returns(AddAssessmentEntryResponse);
        }

        [Fact]
        public void Then_Redirected_To_Error()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            var routeValue = (Result as RedirectToRouteResult).RouteValues["StatusCode"];
            routeName.Should().Be(RouteConstants.Error);
            routeValue.Should().Be(500);
        }
    }
}
