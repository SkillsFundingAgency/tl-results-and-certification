using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.UcasDataTransferService.CsvExtensionTests
{
    public class TestSetup
    {
        protected byte[] ActualByteData;
        protected byte[] ExpectedByteData;

        protected List<dynamic> UcasDataRecords;
        private const string _filePath = @"Services\UcasDataTransferService\CsvExtensionTests\TestData";

        protected void PrepareUcasFileRecords(UcasData ucasData)
        {
            UcasDataRecords = new List<dynamic> { ucasData.Header };
            UcasDataRecords.AddRange(ucasData.UcasDataRecords);
            UcasDataRecords.Add(ucasData.Trailer);
        }

        protected virtual UcasData GetUcasData()
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
                    DateCreated = "18102021"
                },

                UcasDataRecords = new List<UcasDataRecord>
                {
                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "99", ReceivingOrganisation = "90",
                    CentreNumber = "CENTRE-1", CandidateNumber = "1234567890", CandidateName = "Smith:John1", CandidateDateofBirth = "20082000", Sex = "M",
                    UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "Sub1" },
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

        protected virtual byte[] ReadAllBytesFromFile(string filename)
        {
            var filePath = Path.Combine(Path.GetDirectoryName(GetCodeBaseAbsolutePath()), _filePath);
            return File.ReadAllBytes($@"{filePath}\{filename}");
        }

        private string GetCodeBaseAbsolutePath()
        {
            var codeBaseUri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            return Uri.UnescapeDataString(codeBaseUri.AbsolutePath);
        }
    }
}
