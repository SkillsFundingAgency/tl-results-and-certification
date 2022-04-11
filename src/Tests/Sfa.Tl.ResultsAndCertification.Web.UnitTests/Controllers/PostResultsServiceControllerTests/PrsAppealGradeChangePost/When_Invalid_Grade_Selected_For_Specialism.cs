using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealGradeChangePost
{
    public class When_Invalid_Grade_Selected_For_Specialism : TestSetup
    {
        private PrsAppealCheckAndSubmitViewModel _prsAppealCheckAndSubmitViewModel;
        private List<LookupViewModel> _grades;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Specialism;

            _grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "M", Value = "Merit" }, new LookupViewModel { Id = 2, Code = "P", Value = "Pass" } };
            _prsAppealCheckAndSubmitViewModel = new PrsAppealCheckAndSubmitViewModel
            {
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                TlevelTitle = "Tlevel in Education",
                ProviderName = "Barsley College",
                ProviderUkprn = 87654321,
                CoreName = "Childcare",
                CoreLarId = "12121212",
                SpecialismName = "Plumbing",
                SpecialismLarId = "Z1234567",
                ExamPeriod = "Summer 2021",
                OldGrade = "B",
                IsGradeChanged = true,

                ProfileId = 1,
                AssessmentId = 2
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAppealCheckAndSubmitViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_prsAppealCheckAndSubmitViewModel);
            ViewModel = new PrsAppealGradeChangeViewModel { ProfileId = 1, AssessmentId = AssessmentId, ComponentType = ComponentType, SelectedGradeCode = "X", Grades = _grades };
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAppealCheckAndSubmitViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType);
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<PrsAppealCheckAndSubmitViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
