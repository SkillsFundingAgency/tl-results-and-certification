using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsLearnerDetails
{
    public class When_PrsStatus_Is_Reviewed_For_Core : TestSetup
    {
        private PrsLearnerDetailsViewModel _mockResult;

        public override void Given()
        {
            ProfileId = 11;

            _mockResult = new PrsLearnerDetailsViewModel
            {
                // Core
                CoreComponentDisplayName = "Design, Surveying and Planning (123456)",
                PrsCoreComponentExams = new List<PrsComponentExamViewModel>
                {
                    new PrsComponentExamViewModel 
                    { 
                        AssessmentSeries = "Autumn 2021",
                        Grade = "B",
                        PrsStatus = PrsStatus.Reviewed,
                        LastUpdated = "5 June 2021",
                        UpdatedBy = "User 2",
                        AppealEndDate = DateTime.Today.AddDays(10),
                        AssessmentId = 1
                    }
                }
            };

            Loader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel>(AoUkprn, ProfileId).Returns(_mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel>(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as PrsLearnerDetailsViewModel;

            model.HasCoreResults.Should().BeTrue();
            model.CoreComponentDisplayName.Should().Be(_mockResult.CoreComponentDisplayName);
            model.PrsCoreComponentExams.Count.Should().Be(_mockResult.PrsCoreComponentExams.Count);

            foreach (var exam in model.PrsCoreComponentExams)
            {
                exam.IsAddRommAllowed.Should().BeFalse();
                exam.IsAddRommOutcomeAllowed.Should().BeFalse();
                exam.IsAddAppealAllowed.Should().BeTrue();
                exam.IsRequestChangeAllowed.Should().BeFalse();
            }            
        }
    }
}
