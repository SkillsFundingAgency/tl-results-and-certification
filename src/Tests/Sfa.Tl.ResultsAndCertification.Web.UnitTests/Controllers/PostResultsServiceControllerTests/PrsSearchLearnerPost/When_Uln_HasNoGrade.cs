using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsSearchLearnerPost
{
    public class When_Uln_HasNoGrade : TestSetup
    {
        private FindPrsLearnerRecord _findPrsLearner = null;
        private readonly int _profileId = 1;

        public override void Given()
        {
            _findPrsLearner = new FindPrsLearnerRecord
            {
                ProfileId = _profileId,
                Uln = 123456789,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = DateTime.UtcNow.AddYears(-30),
                ProviderName = "Provider",
                ProviderUkprn = 7894561231,
                TlevelTitle = "Title",
                Status = Common.Enum.RegistrationPathwayStatus.Active,
                PathwayAssessments = new List<PrsAssessment> 
                {
                    new PrsAssessment { HasResult = false }
                }
            };

            ViewModel = new PrsSearchLearnerViewModel { SearchUln = _findPrsLearner.Uln.ToString() };
            Loader.FindPrsLearnerRecordAsync(AoUkprn, ViewModel.SearchUln.ToLong()).Returns(_findPrsLearner);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).FindPrsLearnerRecordAsync(AoUkprn, ViewModel.SearchUln.ToLong());
            CacheService.Received(1).SetAsync(CacheKey, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<PrsNoGradeRegisteredViewModel>(x =>
                    x.ProfileId == _findPrsLearner.ProfileId &&
                    x.Uln == _findPrsLearner.Uln &&
                    x.Firstname == _findPrsLearner.Firstname &&
                    x.Lastname == _findPrsLearner.Lastname &&
                    x.DateofBirth == _findPrsLearner.DateofBirth &&
                    x.ProviderName == _findPrsLearner.ProviderName &&
                    x.ProviderUkprn == _findPrsLearner.ProviderUkprn &&
                    x.TlevelTitle == _findPrsLearner.TlevelTitle &&
                    x.AssessmentSeries == _findPrsLearner.PathwayAssessments.FirstOrDefault().SeriesName),
                    Common.Enum.CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_PrsNoAssessmentEntry()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsNoGradeRegistered);
        }
    }
}