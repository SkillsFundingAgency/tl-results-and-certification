using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkRegistrationLoaderTests.ProcessBulkRegistrations
{
    public class When_Registrations_Stage4_Are_Validated : TestSetup
    {
        public override void Given()
        {
            var expectedStage2Response = new List<RegistrationCsvRecordResponse>
                {
                    new RegistrationCsvRecordResponse { RowNum = 1, Uln = 1111111111, FirstName = "First 1", LastName = "Last 1", DateOfBirth = "01/10/1980".ToDateTime(),  ProviderUkprn = 00000001, AcademicYear = DateTime.UtcNow.Year, CoreCode ="12333333", SpecialismCodes = new List<string> {"234567819"} }
                };

            var expectedStage3Response = new List<RegistrationRecordResponse>
                {
                    new RegistrationRecordResponse
                    {
                        Uln = 1111111111,
                        FirstName = "First 1",
                        LastName = "Last 1",
                        DateOfBirth = "01/10/1980".ToDateTime(),
                        AcademicYear = DateTime.UtcNow.Year,
                        TqProviderId = 1,
                        TlProviderId = 1,
                        TqAwardingOrganisationId = 1,
                        TlAwardingOrganisatonId = 1,
                        TlPathwayId = 4,
                        TlSpecialismLarIds = new List<KeyValuePair<int,string>> { new KeyValuePair<int, string>(5, "12345") }
                    }
                };

            var tqRegistrationProfiles = new List<TqRegistrationProfile> {
                new TqRegistrationProfile
                {
                    Id = 1,
                    UniqueLearnerNumber = 1111111111,
                    Firstname = "First 1",
                    Lastname = "Last 1",
                    DateofBirth = "01/10/1980".ToDateTime(),
                    CreatedBy = "System",
                    CreatedOn = DateTime.UtcNow,
                    TqRegistrationPathways = new List<TqRegistrationPathway>
                        {
                            new TqRegistrationPathway
                            {
                                Id = 1000,
                                TqProviderId = 1,
                                AcademicYear = "01/07/2020".ToDateTime().Year, // TODO: Need to calcualate based on the requirements
                                StartDate = DateTime.UtcNow,
                                Status = RegistrationPathwayStatus.Active,
                                IsBulkUpload = true,
                                TqRegistrationSpecialisms = new List<TqRegistrationSpecialism> { new TqRegistrationSpecialism
                                {
                                    Id =  1,
                                    TlSpecialismId = 5,
                                    StartDate = DateTime.UtcNow,
                                    IsOptedin = true,
                                    IsBulkUpload = true,
                                    CreatedBy = "System",
                                    CreatedOn = DateTime.UtcNow,
                                } },
                                TqProvider = new TqProvider
                                {
                                    TqAwardingOrganisationId = 1,
                                    TlProviderId = 1,
                                    TqAwardingOrganisation = new TqAwardingOrganisation
                                    {
                                        Id = 1,
                                        TlAwardingOrganisatonId = 1,
                                        TlPathwayId = 4,
                                    }
                                },
                                CreatedBy = "System",
                                CreatedOn = DateTime.UtcNow
                            }
                        }
                }
             };

            var expectedStage4Response = new RegistrationProcessResponse
            {
                IsSuccess = false,
                ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError
                    {
                        Uln = "1111111111",
                        ErrorMessage = "Active ULN with a different awarding organisation"
                    }
                }
            };

            var csvResponse = new CsvResponseModel<RegistrationCsvRecordResponse> { Rows = expectedStage2Response };
            var expectedWriteFileBytes = new byte[5];

            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            CsvService.ReadAndParseFileAsync(Arg.Any<RegistrationCsvRecordRequest>()).Returns(csvResponse);
            RegistrationService.ValidateRegistrationTlevelsAsync(AoUkprn, Arg.Any<IEnumerable<RegistrationCsvRecordResponse>>()).Returns(expectedStage3Response);
            RegistrationService.TransformRegistrationModel(Arg.Any<IList<RegistrationRecordResponse>>(), Arg.Any<string>()).Returns(tqRegistrationProfiles);
            RegistrationService.CompareAndProcessRegistrationsAsync(Arg.Any<IList<TqRegistrationProfile>>()).Returns(expectedStage4Response);
            CsvService.WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>()).Returns(expectedWriteFileBytes);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            BlobService.Received(1).DownloadFileAsync(Arg.Any<BlobStorageData>());
            CsvService.Received(1).ReadAndParseFileAsync(Arg.Any<RegistrationCsvRecordRequest>());
            RegistrationService.Received(1).ValidateRegistrationTlevelsAsync(AoUkprn, Arg.Any<IEnumerable<RegistrationCsvRecordResponse>>());
            RegistrationService.Received(1).TransformRegistrationModel(Arg.Any<IList<RegistrationRecordResponse>>(), Arg.Any<string>());
            RegistrationService.Received(1).CompareAndProcessRegistrationsAsync(Arg.Any<IList<TqRegistrationProfile>>());

            CsvService.Received(1).WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>());
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
            BlobService.Received(1).MoveFileAsync(Arg.Any<BlobStorageData>());
            DocumentUploadHistoryService.Received(1).CreateDocumentUploadHistory(Arg.Any<DocumentUploadHistoryDetails>());

            Response.IsSuccess.Should().BeFalse();
            Response.BlobUniqueReference.Should().Be(BlobUniqueRef);
        }
    }
}
