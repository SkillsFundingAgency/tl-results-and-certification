﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.Mapper;
using System;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.Extensions.CsvExtensionTests.WriteFile.UcasFileData
{
    public class When_One_Subject_No_Grade : TestSetup
    {
        private const string fileName = "When_One_Subject_No_Grade.txt";

        public When_One_Subject_No_Grade()
        {
            var ucasData = GetUcasData();
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
    }
}