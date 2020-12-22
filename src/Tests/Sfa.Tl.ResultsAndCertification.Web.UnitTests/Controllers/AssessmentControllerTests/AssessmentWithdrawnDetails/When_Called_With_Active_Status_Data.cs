using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AssessmentWithdrawnDetails
{
    public class When_Called_With_Active_Status_Data : TestSetup
    {
        private AssessmentDetailsViewModel mockresult = null;
        public override void Given()
        {
            mockresult = new AssessmentDetailsViewModel
            {
                ProfileId = 1,
                Uln = 1234567890,
                Name = "Test",
                ProviderDisplayName = "Test Provider (1234567)",
                PathwayDisplayName = "Pathway (7654321)",
                PathwayAssessmentSeries = "Summer 2021",
                SpecialismDisplayName = "Specialism1 (2345678)",
                SpecialismAssessmentSeries = "Autumn 2022",
                PathwayStatus = RegistrationPathwayStatus.Active
            };

            AssessmentLoader.GetAssessmentDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(mockresult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
