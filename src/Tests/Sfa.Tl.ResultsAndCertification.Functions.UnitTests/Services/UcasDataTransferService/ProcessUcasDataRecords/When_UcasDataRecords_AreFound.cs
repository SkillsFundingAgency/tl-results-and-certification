using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Ucas;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.UcasDataTransferService.ProcessUcasDataRecords
{
    public class When_UcasDataRecords_AreFound : TestBase
    {
        private UcasData _mockUcasData;
        private const string ucasFileId = "ucasFileId123";

        public override void Given()
        {
            UcasDataType = UcasDataType.Entries;
            _mockUcasData = GetUcasMockData();
            UcasDataService.ProcessUcasDataRecordsAsync(UcasDataType).Returns(_mockUcasData);
            UcasApiClient.SendDataAsync(Arg.Any<UcasDataRequest>()).Returns(ucasFileId);
        }

        [Fact(Skip = "TODO")]
        public void Then_Expected_Methods_Are_Called()
        {
            UcasApiClient.Received(1).SendDataAsync(Arg.Is<UcasDataRequest>(x => x.FileName.EndsWith(Common.Helpers.Constants.FileExtensionTxt) &&
                                                                            x.FileName.Length == 40 && x.FileData.Length == 271 &&
                                                                            !string.IsNullOrEmpty(x.FileHash)));

            BlobStorageService.Received(1).UploadFromByteArrayAsync(Arg.Is<BlobStorageData>(x => x.ContainerName.Equals(DocumentType.Ucas.ToString().ToLower()) &&
               x.SourceFilePath == UcasDataType.Entries.ToString().ToLower() &&
               x.BlobFileName.StartsWith($"{ucasFileId }-") &&
               x.FileData != null &&
               x.UserName.Equals(Common.Helpers.Constants.FunctionPerformedBy)));
        }

        private UcasData GetUcasMockData()
        {
            return new UcasData
            {
                Header = new UcasDataHeader
                {
                    UcasRecordType = (char)UcasRecordType.Header,
                    SendingOrganisation = "99",
                    ReceivingOrganisation = "90",
                    UcasDataType = (char)UcasDataType.Entries,
                    ExamMonth = "09",
                    ExamYear = "2021",
                    DateCreated = DateTime.Today.ToString("ddMMyyyy", CultureInfo.InvariantCulture)
                },

                UcasDataRecords = new List<UcasDataRecord>
                {
                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "99", ReceivingOrganisation = "90",
                    CentreNumber = "CENTRE-1", CandidateNumber = "1234567890", CandidateName = "Smith:John1", CandidateDateofBirth = "20082000", Sex = "M",
                    UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "Sub1" },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "99", ReceivingOrganisation = "90",
                    CentreNumber = "CENTRE-2", CandidateNumber = "1234567891", CandidateName = "Smity:Jony2", CandidateDateofBirth = "20082000", Sex = "F",
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "Sub21", Grade = "A*", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "Sub22", Grade = "A*", PreviousGrade = null },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "99", ReceivingOrganisation = "90",
                    CentreNumber = "CENTRE-3", CandidateNumber = "1234567892", CandidateName = "Smith:John3", CandidateDateofBirth = "20082000", Sex = "M",
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "Sub31", Grade = "A*", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "Sub32", Grade = "A*", PreviousGrade = "B" }
                    } },
                },

                Trailer = new UcasDataTrailer
                {
                    UcasRecordType = (char)UcasRecordType.Trailer,
                    SendingOrganisation = "99",
                    ReceivingOrganisation = "90",
                    Count = 3,
                    ExamDate = "092021"
                }
            };
        }
    }
}
