using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.RequestReplacementDocumentGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private RequestReplacementDocumentViewModel _requestReplacementDocumentViewModel;

        public override void Given()
        {
            ProfileId = 1;
            _requestReplacementDocumentViewModel = null;

            TrainingProviderLoader.GetLearnerRecordDetailsAsync<RequestReplacementDocumentViewModel>(ProviderUkprn, ProfileId).Returns(_requestReplacementDocumentViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<RequestReplacementDocumentViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
