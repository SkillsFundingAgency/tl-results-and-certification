using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderRegistrations;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderRegistrationsControllerTests.DownloadRegistrationsData
{
    public class When_Returns_Null_Years : TestSetup
    {
        public override void Given()
        {
            ProviderRegistrationsLoader.GetAvailableStartYearsAsync().Returns(null as IList<AvailableStartYearViewModel>);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.ShouldBeRedirectToRouteResult(RouteConstants.ProblemWithService);
        }
    }
}