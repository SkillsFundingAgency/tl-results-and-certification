using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.Mapper;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.Extensions.CsvExtensionTests.WriteFile.UcasFileData
{
    public class When_Two_Subjects_No_Grade : TestSetup
    {
        private const string fileName = "When_Two_Subject_No_Grade.txt";

        public When_Two_Subjects_No_Grade()
        {
            var ucasData = GetUcasData();
            AddUcasDataRecords(ucasData);
            PrepareUcasFileRecords(ucasData);

            ExpectedByteData = ReadAllBytesFromFile(fileName);
            ActualByteData = CsvExtensions.WriteFileAsync(UcasDataRecords, typeof(CsvMapper)).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Expected_File_Data_Is_Created()
        {
            ActualByteData.Length.Should().Equals(ExpectedByteData.Length);
            ActualByteData.SequenceEqual(ExpectedByteData).Should().BeTrue();
        }

        private void AddUcasDataRecords(UcasData ucasData)
        {
            ucasData.UcasDataRecords = new List<UcasDataRecord>
            {
                new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 99, ReceivingOrganisation = 90,
                CentreNumber = "CENTRE-2", CandidateNumber = "1234567891", CandidateName = "Smity:Jony2", CandidateDateofBirth = "20082000", Sex = (char)UcasGender.Female,
                    UcasDataComponents = new List<UcasDataComponent>
                {
                    new UcasDataComponent { SubjectCode = "Sub21", Grade = null, PreviousGrade = null },
                    new UcasDataComponent { SubjectCode = "Sub22", Grade = null, PreviousGrade = null },
                } },
            };
        }
    }
}