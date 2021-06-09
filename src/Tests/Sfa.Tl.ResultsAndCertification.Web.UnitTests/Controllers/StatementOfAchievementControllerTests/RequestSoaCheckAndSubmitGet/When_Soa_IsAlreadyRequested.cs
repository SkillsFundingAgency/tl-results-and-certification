using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCheckAndSubmitGet
{
    public class When_Soa_IsAlreadyRequested : StatementOfAchievementControllerTestBase
    {
        private SoaLearnerRecordDetailsViewModel _mockLearnerDetails;
        private IActionResult _result;
        
        private int _profileId;

        public override void Given()
        {
            _profileId = 11;
            
            _mockLearnerDetails = new SoaLearnerRecordDetailsViewModel
            {
                ProfileId = _profileId,
                IsLearnerRegistered = true,
                IsNotWithdrawn = false,
                IsIndustryPlacementAdded = true,
                HasPathwayResult = true,
                IsIndustryPlacementCompleted = true,
                LastPrintRequestedDate = DateTime.UtcNow
            };

            StatementOfAchievementLoader.GetSoaLearnerRecordDetailsAsync(ProviderUkprn, _profileId).Returns(_mockLearnerDetails);
        }
        
        public async override Task When() { await Task.CompletedTask; }

        [Theory]
        // Below second param represent LastRequested Date. 0 -> Today, -1  -> Yesterday and so on. 
        [InlineData(1, 0)]  
        [InlineData(1, -1)]
        [InlineData(1, -2)]
        [InlineData(2, 0)]
        [InlineData(2, -1)]
        [InlineData(2, -2)]
        [InlineData(21, -20)]
        [InlineData(21, -21)]
        public async Task Then_Redirected_To_RequestSoaAlreadySubmitted(int reRequestAllowedInDays, int lastRequested_InDays)
        {
            // inputs
            _mockLearnerDetails.LastPrintRequestedDate = DateTime.Today.AddDays(lastRequested_InDays);
            ResultsAndCertificationConfiguration.SoaRerequestInDays = reRequestAllowedInDays;

            // When
            _result = await Controller.RequestSoaCheckAndSubmitAsync(_profileId);

            // Then
            Expected_Methods_AreCalled();

            if (reRequestAllowedInDays + lastRequested_InDays > 0)
            {
                var routeName = (_result as RedirectToRouteResult).RouteName;
                routeName.Should().Be(RouteConstants.RequestSoaAlreadySubmitted);
            }
            else 
            {
                var viewResult = _result as ViewResult;
                var model = viewResult.Model as SoaLearnerRecordDetailsViewModel;
                model.Should().NotBeNull();
            }
        }

        public void Expected_Methods_AreCalled()
        {
            StatementOfAchievementLoader.Received(1).GetSoaLearnerRecordDetailsAsync(ProviderUkprn, _profileId);
        }
    }
}
