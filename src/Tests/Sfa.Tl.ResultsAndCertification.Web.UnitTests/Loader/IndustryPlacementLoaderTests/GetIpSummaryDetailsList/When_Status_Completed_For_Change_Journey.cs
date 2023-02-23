using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement.IpCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetIpSummaryDetailsList
{
    public class When_Status_Completed_For_Change_Journey : TestSetup
    {
        public List<SummaryItemModel> _expectedSummaryDetails;
        private Dictionary<string, string> _routeAttributes;

        public override void Given()
        {
            CacheModel = new IndustryPlacementViewModel
            {
                IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = IndustryPlacementStatus.Completed, ProfileId = 1, IsChangeJourney = true },
            };

            _routeAttributes = new Dictionary<string, string> { { Constants.ProfileId, CacheModel.IpCompletion.ProfileId.ToString() }, { Constants.IsChangeMode, "true" } };

            _expectedSummaryDetails = new List<SummaryItemModel>
            {
                new SummaryItemModel { Id = "ipstatus", Title = CheckAndSubmitContent.Title_IP_Status_Text, Value = CheckAndSubmitContent.Status_Completed, ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Ip_Status, RouteName = RouteConstants.IpCompletionChange, RouteAttributes = _routeAttributes  },
            };
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            var actualSummaryDetails = ActualResult.Item1;
            var actualIsValid = ActualResult.Item2;

            actualIsValid.Should().BeTrue();
            actualSummaryDetails.Should().BeEquivalentTo(_expectedSummaryDetails);
        }
    }
}
