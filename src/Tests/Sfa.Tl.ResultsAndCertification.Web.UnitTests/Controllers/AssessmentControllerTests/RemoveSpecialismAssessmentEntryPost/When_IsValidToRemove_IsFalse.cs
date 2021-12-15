using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.RemoveSpecialismAssessmentEntryPost
{
    public class When_IsValidToRemove_IsFalse : TestSetup
    {
        private RemoveSpecialismAssessmentEntryViewModel _mockresult = null;

        public override void Given()
        {
            ViewModel = new RemoveSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                Uln = 12345678,
                SpecialismAssessmentIds = "1|2",
                CanRemoveAssessmentEntry = true,
            };

            _mockresult = new RemoveSpecialismAssessmentEntryViewModel
            {
                ProfileId = 1,
                SpecialismAssessmentIds = "1|2",
                AssessmentSeriesName = "summer 2022",
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

            _mockresult.SpecialismAssessmentIds = "1|99";
            AssessmentLoader.GetRemoveSpecialismAssessmentEntriesAsync(AoUkprn, ViewModel.ProfileId, _mockresult.SpecialismAssessmentIds).Returns(_mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AssessmentLoader.Received(1).GetRemoveSpecialismAssessmentEntriesAsync(AoUkprn, ViewModel.ProfileId, ViewModel.SpecialismAssessmentIds);
            AssessmentLoader.DidNotReceive().RemoveSpecialismAssessmentEntryAsync(Arg.Any<long>(), Arg.Any<RemoveSpecialismAssessmentEntryViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
