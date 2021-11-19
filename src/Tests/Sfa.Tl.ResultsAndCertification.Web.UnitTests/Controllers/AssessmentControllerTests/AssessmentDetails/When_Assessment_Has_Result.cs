using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentDetails
{
    public class When_Assessment_Has_Result : TestSetup
    {
        private AssessmentDetailsViewModel mockresult = null;

        public override void Given()
        {
            mockresult = new AssessmentDetailsViewModel
            {
                ProfileId = 1,
                PathwayAssessmentSeries = "Summer 2021",
                IsCoreResultExist = true,
            };

            AssessmentLoader.GetAssessmentDetailsAsync<AssessmentDetailsViewModel>(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(mockresult);
        }

        [Fact]
        public void Then_ActionText_IsEmpty()
        {
            Result.Should().NotBeNull();
            
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as AssessmentDetailsViewModel;
            
            model.Should().NotBeNull();
            model.IsCoreResultExist.Should().BeTrue();
            
            //TODO: Rajesh

            //model.SummaryCoreAssessmentEntry.Should().NotBeNull();
            //model.SummaryCoreAssessmentEntry.ActionText.Should().BeEmpty();
        }
    }
}
