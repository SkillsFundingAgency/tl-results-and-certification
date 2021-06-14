using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaUniqueLearnerNumberPost
{
    public class When_IndustryPlacement_NotAdded : TestSetup
    {
        private FindSoaLearnerRecord _soaLearnerRecord = null;

        public override void Given()
        {
            ViewModel = new RequestSoaUniqueLearnerNumberViewModel { SearchUln = "1234567891" };
            _soaLearnerRecord = new FindSoaLearnerRecord { Uln = 1234567891, LearnerName = "Test Name", DateofBirth = DateTime.UtcNow.AddYears(-30), ProviderName = "Provider (1234567)", TlevelTitle = "Title", Status = Common.Enum.RegistrationPathwayStatus.Withdrawn, IsIndustryPlacementAdded = false };
            StatementOfAchievementLoader.FindSoaLearnerRecordAsync(ProviderUkprn, ViewModel.SearchUln.ToLong()).Returns(_soaLearnerRecord);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            StatementOfAchievementLoader.Received(1).FindSoaLearnerRecordAsync(ProviderUkprn, ViewModel.SearchUln.ToLong());
            CacheService.Received(1).SetAsync(CacheKey, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<RequestSoaNotAvailableNoIpStatusViewModel>(x =>
                    x.Uln == _soaLearnerRecord.Uln &&
                    x.LearnerName == _soaLearnerRecord.LearnerName &&
                    x.DateofBirth == _soaLearnerRecord.DateofBirth &&
                    x.ProviderName == _soaLearnerRecord.ProviderName &&
                    x.TLevelTitle == _soaLearnerRecord.TlevelTitle
                    ), Common.Enum.CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_RequestSoaNotAvailableNoIpStatus()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.RequestSoaNotAvailableNoIpStatus);
        }
    }
}
