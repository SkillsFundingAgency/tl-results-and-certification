using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement.IpCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetIpSummaryDetailsList
{
    public class When_TempFlex_IpEmployerLedUsed_No : TestSetup
    {
        public List<SummaryItemModel> _expectedSummaryDetails;
        private Dictionary<string, string> _routeAttributes;

        public override void Given()
        {
            CacheModel = new IndustryPlacementViewModel
            {
                IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = IndustryPlacementStatus.Completed, ProfileId = 1 },
                IpModelViewModel = new IpModelViewModel
                {
                    IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = false },
                },
                TempFlexibility = new IpTempFlexibilityViewModel
                {
                    IpTempFlexibilityUsed = new IpTempFlexibilityUsedViewModel { IsTempFlexibilityUsed = true },
                    IpBlendedPlacementUsed = new IpBlendedPlacementUsedViewModel { IsBlendedPlacementUsed = true },
                    IpEmployerLedUsed = new IpEmployerLedUsedViewModel
                    {
                        TemporaryFlexibilities = new List<IpLookupDataViewModel>
                        {
                            new IpLookupDataViewModel { Name = "TF1", IsSelected = false },
                            new IpLookupDataViewModel { Name = Constants.BlendedPlacements, IsSelected = true },
                            new IpLookupDataViewModel { Name = "TF2", IsSelected = false }
                        }
                    }
                }
            };

            IpTempFlexNavigation = new IpTempFlexNavigation { AskTempFlexibility = true, AskBlendedPlacement = true };

            _routeAttributes = new Dictionary<string, string> { { Constants.ProfileId, CacheModel.IpCompletion.ProfileId.ToString() }, { Constants.IsChangeMode, "true" } };
            var isChangeModeRouteAttribute = new Dictionary<string, string> { { Constants.IsChangeMode, "true" } };
            _expectedSummaryDetails = new List<SummaryItemModel>
            {
                new SummaryItemModel { Id = "ipstatus", Title = CheckAndSubmitContent.Title_IP_Status_Text, Value = CheckAndSubmitContent.Status_Completed, ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Ip_Status, RouteName = RouteConstants.IpCompletion, RouteAttributes = _routeAttributes },
                
                // Ip Model
                new SummaryItemModel { Id = "isipmodelused", Title = CheckAndSubmitContent.Title_IpModel_Text, Value = "No", ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_IpModel_Used, RouteName = RouteConstants.IpModelUsed, RouteAttributes = isChangeModeRouteAttribute },

                // TF
                new SummaryItemModel { Id = "istempflexused", Title = CheckAndSubmitContent.Title_TempFlex_Used_Text, Value = "Yes", ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Tf_TempFlex_Used, RouteName = RouteConstants.IpTempFlexibilityUsed, RouteAttributes = isChangeModeRouteAttribute },
                new SummaryItemModel { Id = "isblendedplacementused", Title = CheckAndSubmitContent.Title_BlendedPlacement_Used_Text, Value = "Yes", ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Tf_Blended_Used, RouteName = RouteConstants.IpBlendedPlacementUsed, RouteAttributes = isChangeModeRouteAttribute },
                new SummaryItemModel { Id = "anyothertempflexlist", Title = CheckAndSubmitContent.Title_TempFlex_Emp_Led_Text, Value = "No", ActionText = CheckAndSubmitContent.Link_Change, IsRawHtml = true, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Tf_Employer_Led_List }
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
