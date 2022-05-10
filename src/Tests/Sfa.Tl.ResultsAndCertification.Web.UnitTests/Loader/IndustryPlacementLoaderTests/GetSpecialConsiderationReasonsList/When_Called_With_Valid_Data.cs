using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetSpecialConsiderationReasonsList
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private IList<IpLookupData> _mockApiResult;
        private IList<IpLookupDataViewModel> _expectedResult;
        
        public override void Given()
        {
            AcademicYear = DateTime.UtcNow.Year;

            _mockApiResult = new List<IpLookupData>
            {
                new IpLookupData { Id = 1, Name = "Medical Condition", StartDate = DateTime.UtcNow.AddYears(-1), EndDate = null },
                new IpLookupData { Id = 2, Name = "Placement Withdraw", StartDate = DateTime.UtcNow, EndDate = null },
                new IpLookupData { Id = 3, Name = "Bereavement", StartDate = DateTime.UtcNow.AddYears(-1), EndDate = DateTime.UtcNow.AddYears(2) },
                new IpLookupData { Id = 4, Name = "Crisis", StartDate = DateTime.UtcNow.AddYears(1), EndDate = null },
                new IpLookupData { Id = 5, Name = "Circumstancs", StartDate = DateTime.UtcNow.AddYears(1), EndDate = DateTime.UtcNow.AddYears(4) },
                new IpLookupData { Id = 6, Name = "Placement Withdraw", StartDate = DateTime.UtcNow.AddYears(1), EndDate = DateTime.UtcNow.AddYears(4) },
            };

            InternalApiClient.GetIpLookupDataAsync(IpLookupType.SpecialConsideration).Returns(_mockApiResult);

            _expectedResult = new List<IpLookupDataViewModel>
            {
                new IpLookupDataViewModel { Id = 1, Name = "Medical Condition", IsSelected = false },
                new IpLookupDataViewModel { Id = 2, Name = "Placement Withdraw", IsSelected = false },
                new IpLookupDataViewModel { Id = 3, Name = "Bereavement", IsSelected = false },
            };
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Should().NotBeNullOrEmpty();
            ActualResult.Should().HaveCount(3);
            ActualResult.Should().BeEquivalentTo(_expectedResult);
        }
    }
}
