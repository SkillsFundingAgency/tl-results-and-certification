using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnterUniqueLearnerReferenceGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given() { }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as EnterUlnViewModel;
            model.BackLink.Should().NotBeNull();
            model.EnterUln.Should().BeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.ManageLearnerRecordsDashboard);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
