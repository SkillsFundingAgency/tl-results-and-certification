using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AddCoreAssessmentSeriesPost
{
    public class When_Success : TestSetup
    {
        private AddAssessmentEntryResponse AddAssessmentEntryResponse;

        public override void Given()
        {
            ViewModel = new AddAssessmentEntryViewModel
            {
                ProfileId = 1,
                AssessmentSeriesId = 11,
                AssessmentSeriesName = "Summer 2021",
                IsOpted = true
            };

            AddAssessmentEntryResponse = new AddAssessmentEntryResponse 
            {
                IsSuccess = true,
                Uln = 1234567890
            };

            AssessmentLoader.AddAssessmentEntryAsync(AoUkprn, ViewModel).Returns(AddAssessmentEntryResponse);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AssessmentLoader.Received(1).AddAssessmentEntryAsync(AoUkprn, ViewModel);
            CacheService.Received(1).SetAsync(string.Concat(CacheKey, Constants.AddAssessmentEntryConfirmationViewModel), 
                Arg.Is<AddAssessmentEntryConfirmationViewModel>
                (x => x.ProfileId == ViewModel.ProfileId &&
                      x.Uln == AddAssessmentEntryResponse.Uln),
                 CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_AssessmentEntryAddedConfirmation()
        {
            var route = (Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.AssessmentDetails);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
