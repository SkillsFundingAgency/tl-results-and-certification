using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkRegistrationLoaderTests.ProcessBulkRegistrations
{
    public class When_Registrations_Stage3_Are_Validated : TestSetup
    {
        public override void Given()
        {
            var expectedStage2Response = new List<RegistrationCsvRecordResponse>
                {
                    new RegistrationCsvRecordResponse { RowNum = 1, Uln = 1111111111, FirstName = "", LastName = "Last3", DateOfBirth = "01/01/1990".ToDateTime(),  ProviderUkprn = 33333333, AcademicYear = 2020, CoreCode ="12333333", SpecialismCodes = new List<string> {"234567819"}, ValidationErrors = new List<BulkProcessValidationError>
                    {
                        new BulkProcessValidationError { RowNum = "1", Uln = "1111111111", ErrorMessage = "First name required" }
                    } },
                    new RegistrationCsvRecordResponse { RowNum = 2, Uln = 1111111112, FirstName = "First2", LastName = "", DateOfBirth = "01/01/1990".ToDateTime(),  ProviderUkprn = 33333333, AcademicYear = 2020, CoreCode ="12333333", SpecialismCodes = new List<string> {"234567819"}, ValidationErrors = new List<BulkProcessValidationError>
                    {
                        new BulkProcessValidationError { RowNum = "2", Uln = "1111111112", ErrorMessage = "Last name required" }
                    } },
                    new RegistrationCsvRecordResponse { RowNum = 3, Uln = 1111111113, FirstName = "First3", LastName = "Last3", DateOfBirth = "01/01/1990".ToDateTime(),  ProviderUkprn = 00000000, AcademicYear = 2020, CoreCode ="12333333", SpecialismCodes = new List<string> {"234567819"} },
                    new RegistrationCsvRecordResponse { RowNum = 4, Uln = 1111111114, FirstName = "First4", LastName = "Last4", DateOfBirth = "01/01/1990".ToDateTime(),  ProviderUkprn = 33333333, AcademicYear = 2020, CoreCode ="00000000", SpecialismCodes = new List<string> {"234567819"} },
                    new RegistrationCsvRecordResponse { RowNum = 5, Uln = 1111111115, FirstName = "First5", LastName = "Last5", DateOfBirth = "01/01/1990".ToDateTime(),  ProviderUkprn = 33333333, AcademicYear = 2020, CoreCode ="12333333", SpecialismCodes = new List<string> {"000000000"} },
                };

            var expectedStage3Response = new List<RegistrationRecordResponse>
                {
                    new RegistrationRecordResponse { ValidationErrors = new List<BulkProcessValidationError>
                    {
                        new BulkProcessValidationError { RowNum = "3", Uln = "1111111113", ErrorMessage = "Provider not registered with awarding organisation" },
                    } },
                    new RegistrationRecordResponse { ValidationErrors = new List<BulkProcessValidationError>
                    {
                        new BulkProcessValidationError { RowNum = "4", Uln = "1111111114", ErrorMessage = "Core not registered with provider"}
                    } },
                    new RegistrationRecordResponse { ValidationErrors = new List<BulkProcessValidationError>
                    {
                        new BulkProcessValidationError { RowNum = "5", Uln = "1111111115", ErrorMessage = "Specialism not valid with core"}
                    } },
                };

            var csvResponse = new CsvResponseModel<RegistrationCsvRecordResponse> { Rows = expectedStage2Response };
            var expectedWriteFileBytes = new byte[5];

            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            CsvService.ReadAndParseFileAsync(Arg.Any<RegistrationCsvRecordRequest>()).Returns(csvResponse);
            RegistrationService.ValidateRegistrationTlevelsAsync(AoUkprn, Arg.Any<IEnumerable<RegistrationCsvRecordResponse>>()).Returns(expectedStage3Response);
            CsvService.WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>()).Returns(expectedWriteFileBytes);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            BlobService.Received(1).DownloadFileAsync(Arg.Any<BlobStorageData>());
            CsvService.Received(1).ReadAndParseFileAsync(Arg.Any<RegistrationCsvRecordRequest>());
            RegistrationService.Received(1).ValidateRegistrationTlevelsAsync(AoUkprn, Arg.Any<IEnumerable<RegistrationCsvRecordResponse>>());
            CsvService.Received(1).WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>());
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
            BlobService.Received(1).MoveFileAsync(Arg.Any<BlobStorageData>());
            DocumentUploadHistoryService.Received(1).CreateDocumentUploadHistory(Arg.Any<DocumentUploadHistoryDetails>());

            Response.IsSuccess.Should().BeFalse();
            Response.BlobUniqueReference.Should().Be(BlobUniqueRef);
        }
    }
}