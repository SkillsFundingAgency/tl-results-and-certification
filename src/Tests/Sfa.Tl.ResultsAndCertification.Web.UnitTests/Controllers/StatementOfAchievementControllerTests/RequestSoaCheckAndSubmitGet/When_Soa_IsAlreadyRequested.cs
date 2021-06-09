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
        // Param 1 represents config value - SoaRerequestInDays
        // Param 2 represents LastRequested Date. 0 -> Today, -1 -> Yesterday and so on. 
        // Param 3 represents isAlreadyRequested
        [InlineData(0, 0, false)]
        [InlineData(1, 0, true)]  
        [InlineData(1, -1, false)]
        [InlineData(2, 0, true)]
        [InlineData(2, -1, true)]
        [InlineData(2, -2, false)]
        [InlineData(21, -20, true)]
        [InlineData(21, -21, false)]
        public async Task Then_Redirected_To_RequestSoaAlreadySubmitted(int reRequestAllowedInDays, int lastRequested_InDays, bool isAlreadyRequested)
        {
            // inputs
            _mockLearnerDetails.LastPrintRequestedDate = DateTime.Today.AddDays(lastRequested_InDays);
            ResultsAndCertificationConfiguration.SoaRerequestInDays = reRequestAllowedInDays;

            // When
            _result = await Controller.RequestSoaCheckAndSubmitAsync(_profileId);

            // Then
            Expected_Methods_AreCalled();

            if (isAlreadyRequested)
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
