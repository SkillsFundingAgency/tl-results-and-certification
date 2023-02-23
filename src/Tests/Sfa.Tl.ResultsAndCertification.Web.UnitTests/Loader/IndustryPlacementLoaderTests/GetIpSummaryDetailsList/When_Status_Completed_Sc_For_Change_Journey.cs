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
    public class When_Status_Completed_Sc_For_Change_Journey : TestSetup
    {
        public List<SummaryItemModel> _expectedSummaryDetails;
        private Dictionary<string, string> _routeAttributes;

        public override void Given()
        {
            CacheModel = new IndustryPlacementViewModel
            {
                IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration, ProfileId = 1, IsChangeJourney = true },
                SpecialConsideration = new SpecialConsiderationViewModel
                {
                    Hours = new SpecialConsiderationHoursViewModel { Hours = "500" },
                    Reasons = new SpecialConsiderationReasonsViewModel
                    {
                        ReasonsList = new List<IpLookupDataViewModel>
                        {
                            new IpLookupDataViewModel { Name = "Reason 1", IsSelected = true },
                            new IpLookupDataViewModel { Name = "Reason 2", IsSelected = true },
                            new IpLookupDataViewModel { Name = "Reason 3", IsSelected = false }
                        }
                    }
                },
            };

            _routeAttributes = new Dictionary<string, string> { { Constants.ProfileId, CacheModel.IpCompletion.ProfileId.ToString() }, { Constants.IsChangeMode, "true" } };
            var isChangeModeRouteAttribute = new Dictionary<string, string> { { Constants.IsChangeMode, "true" } };
            _expectedSummaryDetails = new List<SummaryItemModel>
            {
                new SummaryItemModel { Id = "ipstatus", Title = CheckAndSubmitContent.Title_IP_Status_Text, Value = CheckAndSubmitContent.Status_Completed_With_Special_Consideration, ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Ip_Status, RouteName = RouteConstants.IpCompletionChange, RouteAttributes = _routeAttributes },
                
                // SC
                new SummaryItemModel { Id = "hours", Title = CheckAndSubmitContent.Title_SpecialConsideration_Hours_Text, Value = CacheModel.SpecialConsideration.Hours.Hours, ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Special_Consideration_Hours , RouteName = RouteConstants.IpSpecialConsiderationHours, RouteAttributes = isChangeModeRouteAttribute },
                new SummaryItemModel { Id = "specialreasons", Title = CheckAndSubmitContent.Title_SpecialConsideration_Reasons_Text, Value = "<p>Reason 1</p><p>Reason 2</p>", ActionText = CheckAndSubmitContent.Link_Change, IsRawHtml = true, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Special_Consideration_Reasons, RouteName = RouteConstants.IpSpecialConsiderationReasons, RouteAttributes = isChangeModeRouteAttribute },
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
