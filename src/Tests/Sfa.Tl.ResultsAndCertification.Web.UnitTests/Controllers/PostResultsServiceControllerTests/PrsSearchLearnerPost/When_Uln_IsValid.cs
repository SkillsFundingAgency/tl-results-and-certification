using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsSearchLearnerPost
{
    public class When_Uln_IsValid : TestSetup
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
                Status = Common.Enum.RegistrationPathwayStatus.Active
            };

            ViewModel = new PrsSearchLearnerViewModel { SearchUln = _findPrsLearner.Uln.ToString() };
            Loader.FindPrsLearnerRecordAsync(AoUkprn, ViewModel.SearchUln.ToLong()).Returns(_findPrsLearner);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).FindPrsLearnerRecordAsync(AoUkprn, ViewModel.SearchUln.ToLong());
            CacheService.Received(1).SetAsync(CacheKey, ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_PrsLearnerDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            route.RouteValues.Count.Should().Be(1);
            route.RouteValues[Constants.ProfileId].Should().Be(_profileId);
        }
    }
}