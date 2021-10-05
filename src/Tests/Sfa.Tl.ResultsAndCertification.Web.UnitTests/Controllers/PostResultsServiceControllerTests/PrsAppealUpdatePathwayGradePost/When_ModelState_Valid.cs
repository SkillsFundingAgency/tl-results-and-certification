using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealUpdatePathwayGradePost
{
    public class When_ModelState_Valid : TestSetup
    {
        private PrsPathwayGradeCheckAndSubmitViewModel _prsCheckAndSubmitViewModel;
        private List<LookupViewModel> _grades;

        public override void Given()
        {
            ProfileId = 1;
            AssessmentId = 7;
            ResultId = 9;

            _grades = new List<LookupViewModel> { new LookupViewModel { Id = 1, Code = "C1", Value = "A" }, new LookupViewModel { Id = 2, Code = "C2", Value = "B" } };
            _prsCheckAndSubmitViewModel = new PrsPathwayGradeCheckAndSubmitViewModel
            {
                ProfileId = ProfileId,
                AssessmentId = AssessmentId,
                ResultId = ResultId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Today.AddYears(-20),
                ProviderName = "Barnsley",
                ProviderUkprn = 912121212,
                TlevelTitle = "Tlevel in Health",
                OldGrade = "B",
                IsGradeChanged = true
            };

            Loader.GetPrsLearnerDetailsAsync<PrsPathwayGradeCheckAndSubmitViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_prsCheckAndSubmitViewModel);
            ViewModel = new AppealUpdatePathwayGradeViewModel { ProfileId = 1, PathwayAssessmentId = AssessmentId, PathwayResultId = ResultId, SelectedGradeCode = "C1", Grades = _grades };
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsPathwayGradeCheckAndSubmitViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId);

            CacheService.Received(1).SetAsync(CacheKey,
                Arg.Is<PrsPathwayGradeCheckAndSubmitViewModel>
                (x => x.ProfileId == _prsCheckAndSubmitViewModel.ProfileId &&                      
                      x.AssessmentId == _prsCheckAndSubmitViewModel.AssessmentId &&
                      x.ResultId == _prsCheckAndSubmitViewModel.ResultId &&
                      x.Uln == _prsCheckAndSubmitViewModel.Uln &&
                      x.Firstname == _prsCheckAndSubmitViewModel.Firstname &&
                      x.Lastname == _prsCheckAndSubmitViewModel.Lastname &&
                      x.DateofBirth == _prsCheckAndSubmitViewModel.DateofBirth &&
                      x.ProviderName == _prsCheckAndSubmitViewModel.ProviderName &&
                      x.ProviderUkprn == _prsCheckAndSubmitViewModel.ProviderUkprn &&
                      x.TlevelTitle == _prsCheckAndSubmitViewModel.TlevelTitle &&
                      x.OldGrade == _prsCheckAndSubmitViewModel.OldGrade &&
                      x.NewGrade == _grades.FirstOrDefault(g => g.Code == ViewModel.SelectedGradeCode).Value &&
                      x.IsGradeChanged == _prsCheckAndSubmitViewModel.IsGradeChanged));
        }

        [Fact]
        public void Then_Redirected_To_PrsPathwayGradeCheckAndSubmit()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsPathwayGradeCheckAndSubmit);
        }
    }
}