using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetIpSummaryDetailsList
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public List<SummaryItemModel> _expectedSummaryDetails;

        public override void Given()
        {
            CacheModel = new IndustryPlacementViewModel
            {
                IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = IndustryPlacementStatus.Completed },
                IpModelViewModel = new IpModelViewModel(),
                TempFlexibility = new IpTempFlexibilityViewModel()
            };

            IpTempFlexNavigation = new IpTempFlexNavigation { AskTempFlexibility = true, AskBlendedPlacement = true };
        }
        
        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            var actualSummaryDetails = ActualResult.Item1;
            var actualIsValid = ActualResult.Item2;

            actualIsValid.Should().BeFalse();
            actualSummaryDetails.Should().BeNull();
        }
    }
}
