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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsRommGradeChangePost
{
    public class When_Invalid_Grade_Selected_For_Core : TestSetup
    {
        private PrsRommCheckAndSubmitViewModel _prsRommCheckAndSubmitViewModel;
        private List<LookupViewModel> _grades;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

            _grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "C1", Value = "A" }, new LookupViewModel { Id = 2, Code = "C2", Value = "B" } };
            _prsRommCheckAndSubmitViewModel = new PrsRommCheckAndSubmitViewModel
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
                ExamPeriod = "Summer 2021",
                OldGrade = "B",
                IsGradeChanged = true,

                ProfileId = 1,
                AssessmentId = 2
            };

            Loader.GetPrsLearnerDetailsAsync<PrsRommCheckAndSubmitViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_prsRommCheckAndSubmitViewModel);
            ViewModel = new PrsRommGradeChangeViewModel { ProfileId = 1, AssessmentId = AssessmentId, ComponentType = ComponentType, SelectedGradeCode = "X", Grades = _grades };
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsRommCheckAndSubmitViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType);
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<PrsRommCheckAndSubmitViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
