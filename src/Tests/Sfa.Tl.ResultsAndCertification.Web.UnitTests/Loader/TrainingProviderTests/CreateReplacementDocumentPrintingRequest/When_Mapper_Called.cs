using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.CreateReplacementDocumentPrintingRequest
{
    public class When_Mapper_Called : TestSetup
    {
        private ReplacementPrintRequest _expectedReplacementPrintRequest;

        public override void Given()
        {
            CreateMapper();
            ProviderUkprn = 987654321;

            RequestReplacementDocumentViewModel = new RequestReplacementDocumentViewModel
            {
                Uln = 1234567890,
                ProviderAddress = new ViewModel.ProviderAddress.AddressViewModel { AddressId = 1 },
                PrintCertificateId = 1,
                LearnerName = "Test test"
            };

            _expectedReplacementPrintRequest = new ReplacementPrintRequest
            {
                Uln = 1234567890,
                ProviderUkprn = 987654321,
                ProviderAddressId = 1,
                PrintCertificateId = 1
            }; 
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var result = Mapper.Map<ReplacementPrintRequest>(RequestReplacementDocumentViewModel, opt => opt.Items["providerUkprn"] = ProviderUkprn);

            result.Should().NotBeNull();
            result.Uln.Should().Be(_expectedReplacementPrintRequest.Uln);
            result.ProviderUkprn.Should().Be(_expectedReplacementPrintRequest.ProviderUkprn);
            result.ProviderAddressId.Should().Be(_expectedReplacementPrintRequest.ProviderAddressId);
            result.PrintCertificateId.Should().Be(_expectedReplacementPrintRequest.PrintCertificateId);
            result.PerformedBy.Should().Be($"{Givenname} {Surname}");
        }
    }
}
