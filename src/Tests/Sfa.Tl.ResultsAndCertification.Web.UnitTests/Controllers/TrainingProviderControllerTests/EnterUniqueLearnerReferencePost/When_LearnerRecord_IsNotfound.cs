using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnterUniqueLearnerReferencePost
{
    public class When_LearnerRecord_IsNotfound : TestSetup
    {
        private readonly long uln = 123456789;
        private FindLearnerRecord mockResult;

        public override void Given()
        {
            EnterUlnViewModel = new EnterUlnViewModel { EnterUln = uln.ToString() };
            mockResult = new FindLearnerRecord { IsLearnerRegistered = false };
            TrainingProviderLoader.FindLearnerRecordAsync(providerUkprn, uln).Returns(mockResult);
        }

        [Fact]
        public void Then_Redirected_To_EnterUniqueLearnerNumberNotFound()
        {
            var route = (Result as RedirectToRouteResult);
            route.RouteName.Should().Be(RouteConstants.EnterUniqueLearnerNumberNotFound);
        }
    }
}