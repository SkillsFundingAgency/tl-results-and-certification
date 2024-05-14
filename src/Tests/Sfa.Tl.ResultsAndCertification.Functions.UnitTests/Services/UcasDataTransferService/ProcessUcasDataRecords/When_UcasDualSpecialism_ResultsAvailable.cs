using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.UcasDataTransferService.ProcessUcasDataRecords
{
    public class When_UcasDualSpecialism_ResultsAvailable : TestBase
    {
        private TqRegistrationPathway _mockExpectedUcasData;
        private readonly List<UcasDataComponent> _dataComponent = new();

        public override void Given()
        {
            UcasDataType = UcasDataType.Results;
            _mockExpectedUcasData = GetExpectedPathwayMockData();
        }

        [Fact]
        public void Then_Expected_DualSpecialsim_Result_Matches()
        {
            UcasRecordResultSegment.AddSpecialismSegment(_dataComponent, _mockExpectedUcasData);
            Assert.Equal("U", _dataComponent[0].Grade);
        }

        private TqRegistrationPathway GetExpectedPathwayMockData()
        {
            string overallDetails = "{\"TlevelTitle\": \"T Level in Building Services Engineering for Construction\",\"PathwayName\": \"Building Services Engineering\", \"PathwayLarId\": \"60369115\"," +
            "\"PathwayResult\": \"A*\",    \"SpecialismDetails\": [{ \"SpecialismName\": \"Heating Engineering\",\"SpecialismLarId\": \"10202101\",\"SpecialismResult\": \"Distinction\"}, " +
            "{\"SpecialismName\": \"Ventilation\",\"SpecialismLarId\": \"10202105\",\"SpecialismResult\": \"Distinction\"}],\"IndustryPlacementStatus\": \"Completed\",\"OverallResult\": \"Distinction\"}";

            return new TqRegistrationPathway()
            {

                OverallResults = new List<OverallResult>
                {
                    new()
                    {
                        Details = overallDetails,
                        Id = 1,
                        IsOptedin = true,
                        SpecialismResultAwarded="Unclassified"
                    }
                }
            };
        }
    }
}