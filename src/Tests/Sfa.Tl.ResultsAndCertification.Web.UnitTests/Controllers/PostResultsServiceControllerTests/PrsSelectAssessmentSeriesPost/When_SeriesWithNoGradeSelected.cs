using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsSelectAssessmentSeriesPost
{
    public class When_SeriesWithNoGradeSelected : TestSetup
    {
        private FindPrsLearnerRecord _findPrsLearnerRecordRecord;
        private PrsNoGradeRegisteredViewModel _prsNoGradeRegisteredViewModel;

        public override void Given()
        {
            var selectedAssessmentId = 22;
            ViewModel = new PrsSelectAssessmentSeriesViewModel { Uln = 1234567890, SelectedAssessmentSeries = selectedAssessmentId };
            _findPrsLearnerRecordRecord = new FindPrsLearnerRecord
            {
                ProfileId = 1,
                Uln = ViewModel.Uln,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Today.AddYears(-18),
                TlevelTitle = "Tlevel in Construction",
                ProviderName = "Barsley college",
                ProviderUkprn = 12345678,
                Status = RegistrationPathwayStatus.Active,
                PathwayAssessments = new List<PrsAssessment>
                {
                    new PrsAssessment { AssessmentId = 11, SeriesName = "Summer 2021", HasResult = true },
                    new PrsAssessment { AssessmentId = selectedAssessmentId, SeriesName = "Autumn 2021", HasResult = false }
                }
            };
            Loader.FindPrsLearnerRecordAsync(AoUkprn, ViewModel.Uln).Returns(_findPrsLearnerRecordRecord);

            _prsNoGradeRegisteredViewModel = new PrsNoGradeRegisteredViewModel
            {
                Uln = _findPrsLearnerRecordRecord.Uln,
                Firstname = _findPrsLearnerRecordRecord.Firstname,
                Lastname = _findPrsLearnerRecordRecord.Lastname,
                DateofBirth = _findPrsLearnerRecordRecord.DateofBirth,
                ProviderName = _findPrsLearnerRecordRecord.ProviderName,
                ProviderUkprn = _findPrsLearnerRecordRecord.ProviderUkprn,
                TlevelTitle = _findPrsLearnerRecordRecord.TlevelTitle,
                AssessmentSeries = _findPrsLearnerRecordRecord.PathwayAssessments.FirstOrDefault(x => !x.HasResult).SeriesName
            };
            Loader.TransformLearnerDetailsTo<PrsNoGradeRegisteredViewModel>(_findPrsLearnerRecordRecord).Returns(_prsNoGradeRegisteredViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).FindPrsLearnerRecordAsync(AoUkprn, ViewModel.Uln);
            Loader.Received(1).TransformLearnerDetailsTo<PrsNoGradeRegisteredViewModel>(_findPrsLearnerRecordRecord);
            CacheService.Received(1).SetAsync(CacheKey, _prsNoGradeRegisteredViewModel, CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_PrsNoGradeRegistered()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsNoGradeRegistered);
        }
    }
}
