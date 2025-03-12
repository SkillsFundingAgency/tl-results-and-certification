using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Help;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.HelpControllerTests.ServiceUnavailableProvider
{
    public class When_Action_IsCalled : TestSetup
    {
        private string _expectedValue;

        public override void Given()
        {
            _expectedValue = "01:01am on Monday 01 August 2022";
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ServiceUnavailableViewModel));

            var model = viewResult.Model as ServiceUnavailableViewModel;
            model.Should().NotBeNull();

            model.ServiceAvailableFrom.Should().Be(_expectedValue);
        }
    }
}
