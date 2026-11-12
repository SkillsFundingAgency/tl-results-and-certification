using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderRegistrations;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerRecordGet
{
    public class When_No_AcademicYears : TestSetup
    {
        public override void Given()
        {
            TrainingProviderLoader.GetAvailableAcademicYearsAsync().Returns(new List<AvailableStartYearViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            Result.ShouldBeRedirectToRouteResult(RouteConstants.ProblemWithService);
        }
    }
}
