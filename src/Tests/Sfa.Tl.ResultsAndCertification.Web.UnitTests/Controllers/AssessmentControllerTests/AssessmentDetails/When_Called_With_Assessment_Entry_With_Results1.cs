using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System;
using System.Collections.Generic;
using Xunit;
using AssessmentDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AssessmentDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentDetails
{
    public class When_Called_With_Assessment_Entry_With_Results1 : TestSetup
    {
        private AssessmentDetailsViewModel _mockresult = null;

        public override void Given()
        {
            _mockresult = new AssessmentDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "First",
                Lastname = "Last",
                DateofBirth = System.DateTime.UtcNow.AddYears(-30),
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567,
                TlevelTitle = "Tlevel Title",
                PathwayStatus = RegistrationPathwayStatus.Active,
                PathwayAssessment = new PathwayAssessmentViewModel
                {
                    SeriesName = "Summer 2021",
                    LastUpdatedBy = "Test User",
                    LastUpdatedOn = DateTime.Today,
                    Results = new List<ResultViewModel>
                    {
                        new ResultViewModel { Id = 111, Grade = "C" }
                    }
                }
            };

            AssessmentLoader.GetAssessmentDetailsAsync<AssessmentDetailsViewModel>(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(_mockresult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            AssessmentLoader.Received(1).GetAssessmentDetailsAsync<AssessmentDetailsViewModel>(AoUkprn, ProfileId, RegistrationPathwayStatus.Active);
        }

        [Fact]
        public void Then_Assessment_Entry_Details_Shown()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(AssessmentDetailsViewModel));

            var model = viewResult.Model as AssessmentDetailsViewModel;
            model.Should().NotBeNull();

            // Exam Period
            model.SummaryExamPeriod.Title.Should().Be(AssessmentDetailsContent.Title_Exam_Period);
            model.SummaryExamPeriod.Value.Should().Be(_mockresult.PathwayAssessment.SeriesName);
            model.SummaryExamPeriod.ActionText.Should().BeNull();

            // Last updated on 
            model.SummaryLastUpdatedOn.Title.Should().Be(AssessmentDetailsContent.Title_Last_Updated_On);
            model.SummaryLastUpdatedOn.Value.Should().Be(_mockresult.PathwayAssessment.LastUpdatedOn.ToDobFormat());

            // Last updated by 
            model.SummaryLastUpdatedBy.Title.Should().Be(AssessmentDetailsContent.Title_Last_Updated_By);
            model.SummaryLastUpdatedBy.Value.Should().Be(_mockresult.PathwayAssessment.LastUpdatedBy);
        }
    }
}
