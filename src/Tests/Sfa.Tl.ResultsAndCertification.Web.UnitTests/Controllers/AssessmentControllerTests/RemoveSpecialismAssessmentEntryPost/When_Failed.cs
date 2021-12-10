using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveSpecialismAssessmentEntryPost
{
    public class When_Failed : TestSetup
    {
        private RemoveSpecialismAssessmentEntryViewModel _mockresult = null;

        public override void Given()
        {
            ViewModel = new RemoveSpecialismAssessmentEntryViewModel
            {
                ProfileId = ProfileId,
                SpecialismAssessmentIds = "1|2",
                CanRemoveAssessmentEntry = true
            };

            _mockresult = new RemoveSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                SpecialismAssessmentIds = "1|2",
                SpecialismDetails = new List<SpecialismViewModel>
                {
                    new SpecialismViewModel
                    {
                        Id = 1,
                        LarId = "ZT1234567",
                        Name = "Specialism 1",
                        DisplayName = "Specialism 1 (ZT1234567)",
                        Assessments = new List<SpecialismAssessmentViewModel>{ new SpecialismAssessmentViewModel { AssessmentId = 1 } }

                    },
                    new SpecialismViewModel
                    {
                        Id = 2,
                        LarId = "ZO565745",
                        Name = "Specialism 2",
                        DisplayName = "Specialism 2 (ZO565745)",
                        Assessments = new List<SpecialismAssessmentViewModel>{ new SpecialismAssessmentViewModel { AssessmentId = 2 } }
                    },
                }
            };

            _mockresult.SpecialismAssessmentIds = string.Join(Constants.PipeSeperator, _mockresult.SpecialismDetails.SelectMany(s => s.Assessments).Select(a => a.AssessmentId));
            AssessmentLoader.GetRemoveSpecialismAssessmentEntriesAsync(AoUkprn, ViewModel.ProfileId, ViewModel.SpecialismAssessmentIds).Returns(_mockresult);
            AssessmentLoader.RemoveSpecialismAssessmentEntryAsync(AoUkprn, ViewModel).Returns(false);
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
