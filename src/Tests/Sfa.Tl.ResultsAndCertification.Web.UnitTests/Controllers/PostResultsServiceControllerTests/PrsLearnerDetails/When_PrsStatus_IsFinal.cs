using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsLearnerDetails
{
    public class When_PrsStatus_IsFinal : TestSetup
    {
        private PrsLearnerDetailsViewModel _mockLearnerDetails;

        public override void Given()
        {
            ProfileId = 11;
            AssessmentId = 1;

            _mockLearnerDetails = new PrsLearnerDetailsViewModel
            {
                Status = RegistrationPathwayStatus.Active,
                PathwayPrsStatus = PrsStatus.Final,
            };

            Loader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel>(AoUkprn, ProfileId, AssessmentId).Returns(_mockLearnerDetails);
        }


        [Fact]
        public void Then_IsFinalOutcomeRegistered_Is_True()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsLearnerDetailsViewModel;

            model.Should().NotBeNull();
            model.IsFinalOutcomeRegistered.Should().BeTrue();
        }
    }
}