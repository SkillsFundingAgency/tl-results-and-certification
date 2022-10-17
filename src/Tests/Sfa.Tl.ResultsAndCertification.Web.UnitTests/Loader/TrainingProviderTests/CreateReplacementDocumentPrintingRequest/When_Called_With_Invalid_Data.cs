using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.CreateReplacementDocumentPrintingRequest
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private bool _expectedApiResult;

        public override void Given()
        {
            CreateMapper();
            ProviderUkprn = 987654321;

            RequestReplacementDocumentViewModel = new RequestReplacementDocumentViewModel
            {
                Uln = 1234567890,               
                ProviderAddress = new ViewModel.ProviderAddress.AddressViewModel { AddressId = 1 },
                PrintCertificateId = 1
            };

            _expectedApiResult = false;

            InternalApiClient.CreateReplacementDocumentPrintingRequestAsync(Arg.Any<ReplacementPrintRequest>()).Returns(_expectedApiResult);

            Loader = new TrainingProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeFalse();
        }
    }
}
