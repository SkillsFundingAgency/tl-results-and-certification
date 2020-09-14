using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessBulkRegistrations
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            BulkRegistrationResponse = new BulkRegistrationResponse
            {
                IsSuccess = true,
                Stats = new BulkUploadStats
                {
                    TotalRecordsCount = 10,
                    NewRecordsCount = 5,
                    AmendedRecordsCount = 3,
                    UnchangedRecordsCount = 2
                }
            };

            UploadRegistrationsRequestViewModel = new UploadRegistrationsRequestViewModel { AoUkprn = Ukprn, File = FormFile };

            UploadRegistrationsResponseViewModel = new UploadRegistrationsResponseViewModel
            {
                IsSuccess = true,
                Stats = new BulkUploadStatsViewModel
                {
                    TotalRecordsCount = 10,
                    NewRecordsCount = 5,
                    AmendedRecordsCount = 3,
                    UnchangedRecordsCount = 2
                }
            };

            Mapper.Map<BulkRegistrationRequest>(UploadRegistrationsRequestViewModel).Returns(BulkRegistrationRequest);
            Mapper.Map<UploadRegistrationsResponseViewModel>(BulkRegistrationResponse).Returns(UploadRegistrationsResponseViewModel);
            InternalApiClient.ProcessBulkRegistrationsAsync(BulkRegistrationRequest).Returns(BulkRegistrationResponse);
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).ProcessBulkRegistrationsAsync(BulkRegistrationRequest);
            BlobStorageService.Received(1).UploadFileAsync(Arg.Any<BlobStorageData>());
            Mapper.Received().Map<BulkRegistrationRequest>(UploadRegistrationsRequestViewModel);
        }
        
        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsSuccess.Should().Be(UploadRegistrationsResponseViewModel.IsSuccess);
            ActualResult.Stats.Should().NotBeNull();

            ActualResult.Stats.TotalRecordsCount.Should().Be(UploadRegistrationsResponseViewModel.Stats.TotalRecordsCount);
            ActualResult.Stats.NewRecordsCount.Should().Be(UploadRegistrationsResponseViewModel.Stats.NewRecordsCount);
            ActualResult.Stats.AmendedRecordsCount.Should().Be(UploadRegistrationsResponseViewModel.Stats.AmendedRecordsCount);
            ActualResult.Stats.UnchangedRecordsCount.Should().Be(UploadRegistrationsResponseViewModel.Stats.UnchangedRecordsCount);
        }
    }
}
