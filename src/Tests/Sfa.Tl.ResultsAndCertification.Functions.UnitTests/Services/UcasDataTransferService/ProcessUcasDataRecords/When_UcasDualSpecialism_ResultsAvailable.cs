using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Globalization;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.UcasDataTransferService.ProcessUcasDataRecords
{
    public class When_UcasDualSpecialism_ResultsAvailable : TestBase
    {

        private OverallResultDetail _mockUcasData;
        private TqRegistrationPathway _mockExpectedUcasData;
        private OverallResultDetail _details;
        private List<UcasDataComponent> _dataComponent;

        public override void Given()
        {
            UcasDataType = UcasDataType.Results;
            _mockUcasData = GetoveralldetailsMockData();
            _mockExpectedUcasData = GetExpectedPathwayMockData();
            _details = UcasRecordResultSegment.ReplaceDualSpecialismCode(_mockUcasData);
            _dataComponent = new List<UcasDataComponent>();

        }

     
        [Fact]
        public void Then_Expected_DualSpecialsim_Result_Matches()
        {
            UcasRecordResultSegment.AddSpecialismSegment(_dataComponent, _mockExpectedUcasData);
            Assert.Equal("U", _dataComponent.FirstOrDefault().Grade);         

        }

        private OverallResultDetail GetoveralldetailsMockData()
        {

            var detail = new OverallResultDetail()
            {
                SpecialismDetails =

                 new List<OverallSpecialismDetail>() {

                     new OverallSpecialismDetail() {

                        SpecialismLarId= "10202101",
                         SpecialismName="Heating Engineering",
                         SpecialismResult = "Pass"
                     },

                      new OverallSpecialismDetail() {

                        SpecialismLarId= "10202105",
                         SpecialismName="Ventilation",
                         SpecialismResult = "Pass"
                   }

                  }
            };

            return detail;

        }

        private TqRegistrationPathway GetExpectedPathwayMockData()
        {

            string overallDetails = "{\"TlevelTitle\": \"T Level in Building Services Engineering for Construction\",\"PathwayName\": \"Building Services Engineering\", \"PathwayLarId\": \"60369115\"," +
            "\"PathwayResult\": \"A*\",    \"SpecialismDetails\": [{ \"SpecialismName\": \"Heating Engineering\",\"SpecialismLarId\": \"10202101\",\"SpecialismResult\": \"Distinction\"}, " +
            "{\"SpecialismName\": \"Ventilation\",\"SpecialismLarId\": \"10202105\",\"SpecialismResult\": \"Distinction\"}],\"IndustryPlacementStatus\": \"Completed\",\"OverallResult\": \"Distinction\"}";
            var pathWayData = new TqRegistrationPathway()
            {

                OverallResults = new List<OverallResult>
                { new OverallResult()
                {


                    Details = overallDetails,
                    Id = 1,
                    IsOptedin = true,
                    SpecialismResultAwarded="Unclassified"
                   
                }
                }
            };

            return pathWayData;
        }
    }

}
   


           
        
    

