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
    // Pattern 1: TempFlexUsed  --> BlendUsed   --> TF List --> Check&Submit
    // Pattern 2: TempFlexUsed  --> TFList      --> Check&Submit
    // Pattern 3: BlendUsed     --> Check&Submit
    // Pattern 4: Check&Submit
    public class When_Called_With_Valid_Pattern3_Data : TestSetup
    {
        public List<SummaryItemModel> _expectedSummaryDetails;
        private Dictionary<string, string> _routeAttributes;

        public override void Given()
        {
            CacheModel = new IndustryPlacementViewModel
            {
                IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration, ProfileId = 1 },
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
                IpModelViewModel = new IpModelViewModel
                {
                    IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = true },
                    IpMultiEmployerUsed = new IpMultiEmployerUsedViewModel { IsMultiEmployerModelUsed = true },
                    IpMultiEmployerOther = new IpMultiEmployerOtherViewModel
                    {
                        OtherIpPlacementModels = new List<IpLookupDataViewModel>
                        {
                            new IpLookupDataViewModel { Name = "IpModel 1", IsSelected = true },
                            new IpLookupDataViewModel { Name = Constants.MultipleEmployer, IsSelected = true },
                            new IpLookupDataViewModel { Name = "IpModel 3", IsSelected = false }
                        }
                    }
                },
                TempFlexibility = new IpTempFlexibilityViewModel
                {
                    IpBlendedPlacementUsed = new IpBlendedPlacementUsedViewModel { IsBlendedPlacementUsed = true },
                    IpEmployerLedUsed = new IpEmployerLedUsedViewModel
                    {
                        TemporaryFlexibilities = new List<IpLookupDataViewModel>
                        {
                            new IpLookupDataViewModel { Name = "TF1", IsSelected = false },
                            new IpLookupDataViewModel { Name = "TF2", IsSelected = false }
                        }
                    }
                }
            };

            IpTempFlexNavigation = new IpTempFlexNavigation { AskTempFlexibility = false, AskBlendedPlacement = true };

            _routeAttributes = new Dictionary<string, string> { { Constants.ProfileId, CacheModel.IpCompletion.ProfileId.ToString() }, { Constants.IsChangeMode, "true" } };
            _expectedSummaryDetails = new List<SummaryItemModel>
            {
                new SummaryItemModel { Id = "ipstatus", Title = CheckAndSubmitContent.Title_IP_Status_Text, Value = CheckAndSubmitContent.Status_Completed_With_Special_Consideration, ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Ip_Status, RouteName = RouteConstants.IpCompletion, RouteAttributes = _routeAttributes },
                
                // SC
                new SummaryItemModel { Id = "hours", Title = CheckAndSubmitContent.Title_SpecialConsideration_Hours_Text, Value = CacheModel.SpecialConsideration.Hours.Hours, ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Special_Consideration_Hours },
                new SummaryItemModel { Id = "specialreasons", Title = CheckAndSubmitContent.Title_SpecialConsideration_Reasons_Text, Value = "<p>Reason 1</p><p>Reason 2</p>", ActionText = CheckAndSubmitContent.Link_Change, IsRawHtml = true, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Special_Consideration_Reasons },
                
                // Ip Model
                new SummaryItemModel { Id = "isipmodelused", Title = CheckAndSubmitContent.Title_IpModel_Text, Value = "Yes", ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_IpModel_Used },
                new SummaryItemModel { Id = "ismultiempmodel", Title = CheckAndSubmitContent.Title_IpModel_Multi_Emp_Text, Value = "Yes", ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_MultiEmp_Used },
                new SummaryItemModel { Id = "selectedothermodellist", Title = CheckAndSubmitContent.Title_IpModel_Selected_Other_List_Text, Value = "<p>IpModel 1</p>", ActionText = CheckAndSubmitContent.Link_Change, IsRawHtml = true, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Ipmodel_Others_list },

                // TF
                new SummaryItemModel { Id = "isblendedplacementused", Title = CheckAndSubmitContent.Title_BlendedPlacement_Used_Text, Value = "Yes", ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Tf_Blended_Used },
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
