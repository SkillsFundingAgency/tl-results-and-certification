using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealGradeChangePost
{
    public class When_ModelState_Valid_For_Core : TestSetup
    {
        private PrsAppealCheckAndSubmitViewModel _prsAppealCheckAndSubmitViewModel;
        private List<LookupViewModel> _grades;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ComponentType = ComponentType.Core;

            _grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "C1", Value = "A" }, new LookupViewModel { Id = 2, Code = "C2", Value = "B" } };
            _prsAppealCheckAndSubmitViewModel = new PrsAppealCheckAndSubmitViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                ProviderName = "Barnsley",
                ProviderUkprn = 912121212,
                TlevelTitle = "Tlevel in Health",
                CoreName = "Childcare",
                CoreLarId = "12121212",
                ExamPeriod = "Summer 2022",
                OldGrade = "B",
                IsGradeChanged = true,
                ComponentType = ComponentType
            };

            Loader.GetPrsLearnerDetailsAsync<PrsAppealCheckAndSubmitViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_prsAppealCheckAndSubmitViewModel);
            ViewModel = new PrsAppealGradeChangeViewModel { ProfileId = ProfileId, AssessmentId = AssessmentId, ComponentType = ComponentType, SelectedGradeCode = "C1", Grades = _grades };
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAppealCheckAndSubmitViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType);

            CacheService.Received(1).SetAsync(CacheKey,
                Arg.Is<PrsAppealCheckAndSubmitViewModel>
                (x => x.ProfileId == _prsAppealCheckAndSubmitViewModel.ProfileId &&
                      x.AssessmentId == _prsAppealCheckAndSubmitViewModel.AssessmentId &&
                      x.Uln == _prsAppealCheckAndSubmitViewModel.Uln &&
                      x.Firstname == _prsAppealCheckAndSubmitViewModel.Firstname &&
                      x.Lastname == _prsAppealCheckAndSubmitViewModel.Lastname &&
                      x.DateofBirth == _prsAppealCheckAndSubmitViewModel.DateofBirth &&
                      x.ProviderName == _prsAppealCheckAndSubmitViewModel.ProviderName &&
                      x.ProviderUkprn == _prsAppealCheckAndSubmitViewModel.ProviderUkprn &&
                      x.TlevelTitle == _prsAppealCheckAndSubmitViewModel.TlevelTitle &&
                      x.CoreName == _prsAppealCheckAndSubmitViewModel.CoreName &&
                      x.CoreLarId == _prsAppealCheckAndSubmitViewModel.CoreLarId &&
                      x.ExamPeriod == _prsAppealCheckAndSubmitViewModel.ExamPeriod &&
                      x.ComponentType == _prsAppealCheckAndSubmitViewModel.ComponentType &&
                      x.OldGrade == _prsAppealCheckAndSubmitViewModel.OldGrade &&
                      x.NewGrade == _grades.FirstOrDefault(g => g.Code == ViewModel.SelectedGradeCode).Value &&
                      x.IsGradeChanged == _prsAppealCheckAndSubmitViewModel.IsGradeChanged));
        }

        [Fact]
        public void Then_Redirected_To_PrsAppealCheckAndSubmit()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsAppealCheckAndSubmit);
        }
    }
}
