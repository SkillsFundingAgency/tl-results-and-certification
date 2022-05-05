using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetTemporaryFlexibilities
{
    public class When_Called_With_Valid_Data_ShowOption_True : TestSetup
    {
        private IList<IpLookupData> _mockApiResult;
        private IList<IpLookupDataViewModel> _expectedResult;

        public override void Given()
        {
            PathwayId = 1;
            AcademicYear = DateTime.UtcNow.Year;
            ShowOption = true;

            _mockApiResult = new List<IpLookupData>
            {
                new IpLookupData { Id = 1, Name = "JABQG risk-rated approach", StartDate = DateTime.UtcNow.AddYears(-1), EndDate = null, ShowOption = null },
                new IpLookupData { Id = 2, Name = "Reduction in hours", StartDate = DateTime.UtcNow, EndDate = null, ShowOption = false },
                new IpLookupData { Id = 3, Name = "Employer led activities/projects", StartDate = DateTime.UtcNow.AddYears(-1), EndDate = DateTime.UtcNow.AddYears(2), ShowOption = true },
                new IpLookupData { Id = 4, Name = "Up to 100% remote", StartDate = DateTime.UtcNow.AddYears(1), EndDate = null, ShowOption = null },
                new IpLookupData { Id = 5, Name = "Pathway level placements", StartDate = DateTime.UtcNow.AddYears(1), EndDate = DateTime.UtcNow.AddYears(4), ShowOption = null },
                new IpLookupData { Id = 6, Name = "Blended placements", StartDate = DateTime.UtcNow.AddYears(1), EndDate = DateTime.UtcNow.AddYears(4), ShowOption = true },
            };

            InternalApiClient.GetIpLookupDataAsync(IpLookupType.TemporaryFlexibility, PathwayId).Returns(_mockApiResult);

            _expectedResult = new List<IpLookupDataViewModel>
            {
                new IpLookupDataViewModel { Id = 1, Name = "JABQG risk-rated approach", IsSelected = false },
                new IpLookupDataViewModel { Id = 3, Name = "Employer led activities/projects", IsSelected = false }
            };
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Should().NotBeNullOrEmpty();
            ActualResult.Should().HaveCount(2);
            ActualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}
