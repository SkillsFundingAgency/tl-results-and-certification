using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.AddCoreAssessmentSeriesGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private AddAssessmentEntryViewModel _mockresult = null;

        public override void Given()
        {
            AssessmentLoader.GetAddAssessmentEntryAsync<AddAssessmentEntryViewModel>(AoUkprn, ProfileId, Arg.Any<ComponentType>()).Returns(_mockresult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}