using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;
using NSubstitute;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.RequestReplacementDocumentPost
{
    public class When_Failed : TestSetup
    {
        public override void Given()
        {
            ViewModel = new RequestReplacementDocumentViewModel { Uln = 987456123, PrintCertificateId = 1, ProviderAddress = new ViewModel.ProviderAddress.AddressViewModel { AddressId = 1 } };
            TrainingProviderLoader.CreateReplacementDocumentPrintingRequestAsync(ProviderUkprn, ViewModel).Returns(false);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).CreateReplacementDocumentPrintingRequestAsync(ProviderUkprn, ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            Result.Should().BeOfType(typeof(RedirectToRouteResult));

            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }
    }
}
