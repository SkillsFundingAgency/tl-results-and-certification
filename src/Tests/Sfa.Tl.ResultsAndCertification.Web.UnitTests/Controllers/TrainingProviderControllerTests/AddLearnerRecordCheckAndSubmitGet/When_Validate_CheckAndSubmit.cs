using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddLearnerRecordCheckAndSubmitGet
{
    public class When_Validate_CheckAndSubmit : TestSetup
    {
        public override void Given() { }

        [Theory]
        [ClassData(typeof(CheckAndSubmitTestDataGenerator))]
        public void Then_Returns_Expected_Results(CheckAndSubmitTestDataModel data)
        {
            CacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey).Returns(data.AddLearnerRecordViewModel);

            Result = Controller.AddLearnerRecordCheckAndSubmitAsync().Result;

            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(data.RouteName);
        }
    }
}