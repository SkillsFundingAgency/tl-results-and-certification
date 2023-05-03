using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Ucas;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.UcasDataTransferService.ProcessUcasDataRecords
{
    public class When_UcasDualDataRecords_AreFound : TestBase
    {
        private TqRegistrationPathway _mockUcasData;
        private TqRegistrationPathway _mockExpectedUcasData;
        private string _dualSpecialismCode;

        public override void Given()
        {
            UcasDataType = UcasDataType.Entries;
            _mockUcasData = GetPathwayMockData();
            _mockExpectedUcasData = GetExpectedPathwayMockData();
            _dualSpecialismCode = UcasRecordEntrySegment.ReplaceDualSpecialismCodes(_mockUcasData);

        }

        [Fact]
        public void Then_Expected_DualSpecialism_Matches()
        {

            Assert.True(_dualSpecialismCode ==
                _mockExpectedUcasData.TqRegistrationSpecialisms.First().TlSpecialism.LarId);

        }

        [Fact]
        public void Then_Expected_DualSpecialism_Contains_Code()
        {
            Assert.Contains("ZTLOS", _mockExpectedUcasData.TqRegistrationSpecialisms.First().TlSpecialism.LarId);
        }

        private TqRegistrationPathway GetPathwayMockData()
        {
            var pathWayData = new TqRegistrationPathway()
            {
                TqRegistrationProfileId = 51368,
                AcademicYear = 2021,
                IndustryPlacements = new List<IndustryPlacement>() { new IndustryPlacement() { Id = 1, Status = (IndustryPlacementStatus)1 } },
                TqRegistrationSpecialisms = new List<TqRegistrationSpecialism>() { new TqRegistrationSpecialism() {
                   TqRegistrationPathwayId = 24414463,
                   TlSpecialismId=11,
                    StartDate = new DateTime(2023,4,26),
                    IsOptedin = true,
                    Id = 22801838,
                   TlSpecialism = new TlSpecialism()
                   {
                        Id = 11,
                        LarId= "10202101",
                         Name="Heating Engineering"
                   }

                  }
                ,new TqRegistrationSpecialism() {
                   TqRegistrationPathwayId = 24414463,
                   TlSpecialismId=16,
                    StartDate = new DateTime(2023,4,26),
                    IsOptedin = true,
                    Id = 22801839,
                     TlSpecialism = new TlSpecialism()
                   {
                        Id = 16,
                        LarId= "10202105",
                         Name="Ventilation"
                   }

                  } }
            };

            return pathWayData;

        }

        private TqRegistrationPathway GetExpectedPathwayMockData()
        {
            var pathWayData = new TqRegistrationPathway()
            {
                TqRegistrationProfileId = 51368,
                AcademicYear = 2021,
                IndustryPlacements = new List<IndustryPlacement>() { new IndustryPlacement() { Id = 1, Status = (IndustryPlacementStatus)1 } },
                TqRegistrationSpecialisms = new List<TqRegistrationSpecialism>() { new TqRegistrationSpecialism() {
                   TqRegistrationPathwayId = 24414463,
                    TlSpecialismId=11,
                    StartDate = new DateTime(2023,4,26),
                    IsOptedin = true,
                    Id = 22801838,
                     TlSpecialism = new TlSpecialism()
                   {
                        Id = 16,
                        LarId= "ZTLOS031",
                         Name="Ventilation"
                   }

                  }
                }
            };

            return pathWayData;
        }
    }
}
