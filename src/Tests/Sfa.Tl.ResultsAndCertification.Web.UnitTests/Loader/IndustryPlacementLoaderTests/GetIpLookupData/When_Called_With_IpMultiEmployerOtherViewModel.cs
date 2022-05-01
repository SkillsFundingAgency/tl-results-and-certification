using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetIpLookupData
{
    public class When_Called_With_IpMultiEmployerOtherViewModel : TestSetup
    {
        private IList<IpLookupData> _mockApiResult;
        private IList<IpLookupDataViewModel> _expectedResult;
        protected IpMultiEmployerOtherViewModel ActualResult;

        public override void Given()
        {
            PathwayId = 1;
            LearnerName = "First Last";

            _mockApiResult = new List<IpLookupData>
            {
                new IpLookupData { Id = 1, Name = "Relevant part-time work", StartDate = DateTime.UtcNow.AddYears(-1), EndDate = null, ShowOption = true },
                new IpLookupData { Id = 2, Name = "On-site facilities", StartDate = DateTime.UtcNow, EndDate = null, ShowOption = null },
                new IpLookupData { Id = 3, Name = "Route-level placements", StartDate = DateTime.UtcNow.AddYears(-1), EndDate = null, ShowOption = false },
                new IpLookupData { Id = 4, Name = "Commercial", StartDate = DateTime.UtcNow.AddYears(1), EndDate = null, ShowOption = true },
            };

            InternalApiClient.GetIpLookupDataAsync(IpLookupType.IndustryPlacementModel, PathwayId).Returns(_mockApiResult);

            _expectedResult = new List<IpLookupDataViewModel>
            {
                new IpLookupDataViewModel { Id = 1, Name = "Relevant part-time work", IsSelected = false },
                new IpLookupDataViewModel { Id = 2, Name = "On-site facilities", IsSelected = false },
                new IpLookupDataViewModel { Id = 4, Name = "Commercial", IsSelected = false },
            };
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetIpLookupDataAsync<IpMultiEmployerOtherViewModel>(IpLookupType.IndustryPlacementModel, LearnerName, PathwayId, true);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.LearnerName.Should().Be(LearnerName);
            ActualResult.OtherIpPlacementModels.Should().HaveCount(3);
            ActualResult.OtherIpPlacementModels.Should().BeEquivalentTo(_expectedResult);
        }
    }
}
