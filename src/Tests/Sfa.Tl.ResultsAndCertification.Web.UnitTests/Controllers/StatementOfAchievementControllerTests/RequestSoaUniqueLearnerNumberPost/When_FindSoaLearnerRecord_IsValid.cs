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
    public class When_FindSoaLearnerRecord_IsValid : TestSetup
    {
        private FindSoaLearnerRecord _soaLearnerRecord;

        public override void Given()
        {
            ViewModel = new RequestSoaUniqueLearnerNumberViewModel { SearchUln = "1234567891" };
            _soaLearnerRecord = new FindSoaLearnerRecord { ProfileId = 11, Uln = 1234567891, LearnerName = "Test Name", DateofBirth = DateTime.UtcNow.AddYears(-30), ProviderName = "Provider (1234567)", TlevelTitle = "Title", Status = Common.Enum.RegistrationPathwayStatus.Withdrawn, HasPathwayResult = true, IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed, IsIndustryPlacementAdded = true };
            StatementOfAchievementLoader.FindSoaLearnerRecordAsync(ProviderUkprn, ViewModel.SearchUln.ToLong()).Returns(_soaLearnerRecord);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            StatementOfAchievementLoader.Received(1).FindSoaLearnerRecordAsync(ProviderUkprn, ViewModel.SearchUln.ToLong());
            CacheService.Received(1).SetAsync(CacheKey, ViewModel);
            CacheService.Received(1).RemoveAsync<RequestSoaUniqueLearnerNumberViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_RequestSoaCheckAndSubmit()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.RequestSoaCheckAndSubmit);
        }
    }
}
