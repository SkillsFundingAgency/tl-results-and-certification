using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.Mapper;
using System;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.UcasDataTransferService.CsvExtensionTests
{
    public class When_One_Subject_With_Grade_PrevGrade : TestSetup
    {
        private const string fileName = "When_One_Subject_With_Grade_PrevGrade.txt";

        public When_One_Subject_With_Grade_PrevGrade()
        {
            var ucasData = GetUcasData();

            var componentGrade = ucasData.UcasDataRecords.First().UcasDataComponents.First();
            componentGrade.Grade = "A*";
            componentGrade.PreviousGrade = "B";

            PrepareUcasFileRecords(ucasData);

            ExpectedByteData = ReadAllBytesFromFile(fileName);
            ActualByteData = CsvExtensions.WriteFileAsync(UcasDataRecords, "|", false, typeof(CsvMapper)).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Expected_File_Data_Is_Created()
        {
            ActualByteData.Length.Should().Equals(ExpectedByteData.Length);
            ActualByteData.SequenceEqual(ExpectedByteData).Should().BeTrue();
        }
    }
}