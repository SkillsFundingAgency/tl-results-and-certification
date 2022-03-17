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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsRommGradeChangePost
{
    public class When_ModelState_Valid : TestSetup
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
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,                
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                ProviderName = "Barnsley",
                ProviderUkprn = 912121212,
                TlevelTitle = "Tlevel in Health",
                CoreDisplayName = "Core (12345)",
                ExamPeriod = "Summer 2022",
                OldGrade = "B",
                IsGradeChanged = true,
                ComponentType = ComponentType
            };

            Loader.GetPrsLearnerDetailsAsync<PrsRommCheckAndSubmitViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType).Returns(_prsRommCheckAndSubmitViewModel);
            ViewModel = new PrsRommGradeChangeViewModel { ProfileId = ProfileId, AssessmentId = AssessmentId, ComponentType = ComponentType, SelectedGradeCode = "C1", Grades = _grades };
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsRommCheckAndSubmitViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType);

            CacheService.Received(1).SetAsync(CacheKey,
                Arg.Is<PrsRommCheckAndSubmitViewModel>
                (x => x.ProfileId == _prsRommCheckAndSubmitViewModel.ProfileId &&
                      x.AssessmentId == _prsRommCheckAndSubmitViewModel.AssessmentId &&                      
                      x.Uln == _prsRommCheckAndSubmitViewModel.Uln &&
                      x.Firstname == _prsRommCheckAndSubmitViewModel.Firstname &&
                      x.Lastname == _prsRommCheckAndSubmitViewModel.Lastname &&
                      x.DateofBirth == _prsRommCheckAndSubmitViewModel.DateofBirth &&
                      x.ProviderName == _prsRommCheckAndSubmitViewModel.ProviderName &&
                      x.ProviderUkprn == _prsRommCheckAndSubmitViewModel.ProviderUkprn &&
                      x.TlevelTitle == _prsRommCheckAndSubmitViewModel.TlevelTitle &&
                      x.CoreDisplayName == _prsRommCheckAndSubmitViewModel.CoreDisplayName &&
                      x.ExamPeriod == _prsRommCheckAndSubmitViewModel.ExamPeriod &&
                      x.ComponentType == _prsRommCheckAndSubmitViewModel.ComponentType &&
                      x.OldGrade == _prsRommCheckAndSubmitViewModel.OldGrade &&
                      x.NewGrade == _grades.FirstOrDefault(g => g.Code == ViewModel.SelectedGradeCode).Value &&
                      x.IsGradeChanged == _prsRommCheckAndSubmitViewModel.IsGradeChanged));
        }

        [Fact]
        public void Then_Redirected_To_PrsPathwayGradeCheckAndSubmit()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsRommCheckAndSubmit);
        }
    }
}
