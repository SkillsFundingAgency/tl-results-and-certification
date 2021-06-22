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
    public class When_Uln_IsWithdrawn : TestSetup
    {
        private FindPrsLearnerRecord _findPrsLearner = null;

        public override void Given()
        {
            _findPrsLearner = new FindPrsLearnerRecord
            {
                ProfileId = 1,
                Uln = 123456789,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = DateTime.UtcNow.AddYears(-30),
                ProviderName = "Provider",
                ProviderUkprn = 7894561231,
                TlevelTitle = "Title",
                Status = Common.Enum.RegistrationPathwayStatus.Withdrawn
            };

            ViewModel = new PrsSearchLearnerViewModel { SearchUln = _findPrsLearner.Uln.ToString() };
            Loader.FindPrsLearnerRecordAsync(AoUkprn, ViewModel.SearchUln.ToLong()).Returns(_findPrsLearner);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).FindPrsLearnerRecordAsync(AoUkprn, ViewModel.SearchUln.ToLong());
            CacheService.Received(1).SetAsync(CacheKey, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<PrsUlnWithdrawnViewModel>(x =>
                    x.Uln == _findPrsLearner.Uln &&
                    x.Firstname == _findPrsLearner.Firstname &&
                    x.Lastname == _findPrsLearner.Lastname &&
                    x.DateofBirth == _findPrsLearner.DateofBirth &&
                    x.ProviderName == _findPrsLearner.ProviderName &&
                    x.ProviderUkprn == _findPrsLearner.ProviderUkprn &&
                    x.TlevelTitle == _findPrsLearner.TlevelTitle),
                    Common.Enum.CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_PrsUlnWithdrawn()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsUlnWithdrawn);
        }
    }
}
