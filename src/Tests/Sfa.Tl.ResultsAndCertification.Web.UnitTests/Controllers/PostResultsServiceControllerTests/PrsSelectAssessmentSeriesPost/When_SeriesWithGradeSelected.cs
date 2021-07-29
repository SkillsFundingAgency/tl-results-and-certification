using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsSelectAssessmentSeriesPost
{
    public class When_SeriesWithGradeSelected : TestSetup
    {
        private FindPrsLearnerRecord _findPrsLearnerRecordRecord;
        private int _selectedAssessmentId;

        public override void Given()
        {
            _selectedAssessmentId = 11;
            ViewModel = new PrsSelectAssessmentSeriesViewModel { Uln = 1234567890, SelectedAssessmentId = _selectedAssessmentId };
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
                    new PrsAssessment { AssessmentId = _selectedAssessmentId, SeriesName = "Summer 2021", HasResult = true },
                    new PrsAssessment { AssessmentId = 22, SeriesName = "Autumn 2021", HasResult = false }
                }
            };
            Loader.FindPrsLearnerRecordAsync(AoUkprn, ViewModel.Uln).Returns(_findPrsLearnerRecordRecord);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).FindPrsLearnerRecordAsync(AoUkprn, ViewModel.Uln);
        }

        [Fact]
        public void Then_Redirected_To_PrsLearnerDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            route.RouteValues.Count.Should().Be(2);
            route.RouteValues[Constants.ProfileId].Should().Be(_findPrsLearnerRecordRecord.ProfileId);
            route.RouteValues[Constants.AssessmentId].Should().Be(_selectedAssessmentId);
        }
    }
}
