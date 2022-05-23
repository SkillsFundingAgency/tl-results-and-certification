using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement.IpCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class IndustryPlacementLoader : IIndustryPlacementLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;

        public IndustryPlacementLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
        }

        public async Task<IList<IpLookupData>> GetIpLookupDataAsync(IpLookupType ipLookupType, int? pathwayId = null)
        {
            return await _internalApiClient.GetIpLookupDataAsync(ipLookupType, pathwayId);
        }

        public async Task<T> GetLearnerRecordDetailsAsync<T>(long providerUkprn, int profileId, int? pathwayId = null)
        {
            var response = await _internalApiClient.GetLearnerRecordDetailsAsync(providerUkprn, profileId, pathwayId);
            return _mapper.Map<T>(response);
        }

        public async Task<T> GetIpLookupDataAsync<T>(IpLookupType ipLookupType, string learnerName = null, int? pathwayId = null, bool showOption = false)
        {
            var lookupData = await _internalApiClient.GetIpLookupDataAsync(ipLookupType, pathwayId);

            if (lookupData == null)
                return default;

            lookupData = lookupData.Where(lkp => lkp.ShowOption == showOption || lkp.ShowOption == null).ToList();

            return _mapper.Map<T>(lookupData, opt => opt.Items["learnerName"] = learnerName);
        }

        public async Task<T> TransformIpCompletionDetailsTo<T>(IpCompletionViewModel model)
        {
            return await Task.FromResult(_mapper.Map<T>(model));
        }

        public async Task<IList<IpLookupDataViewModel>> GetSpecialConsiderationReasonsListAsync(int academicYear)
        {
            var scReasons = await GetIpLookupDataAsync(IpLookupType.SpecialConsideration);
            return _mapper.Map<IList<IpLookupDataViewModel>>(scReasons.Where(x => academicYear >= x.StartDate.Year && (x.EndDate == null || academicYear <= x.EndDate.Value.Year)));
        }

        public async Task<IList<IpLookupDataViewModel>> GetTemporaryFlexibilitiesAsync(int pathwayId, int academicYear, bool showOption = false)
        {
            var tempFlexibilities = await GetIpLookupDataAsync(IpLookupType.TemporaryFlexibility, pathwayId);
            return _mapper.Map<IList<IpLookupDataViewModel>>(tempFlexibilities?.Where(x => academicYear >= x.StartDate.Year && (x.EndDate == null || academicYear <= x.EndDate.Value.Year) && (x.ShowOption == showOption || x.ShowOption == null)));
        }

        public async Task<IpTempFlexNavigation> GetTempFlexNavigationAsync(int pathwayId, int academicYear)
        {
            return await _internalApiClient.GetTempFlexNavigationAsync(pathwayId, academicYear);
        }

        public async Task<bool> ProcessIndustryPlacementDetailsAsync(long providerUkprn, IndustryPlacementViewModel viewModel)
        {
            var request = _mapper.Map<IndustryPlacementRequest>(viewModel.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.NotCompleted
                                                                  ? viewModel.IpCompletion : viewModel, opt => opt.Items["providerUkprn"] = providerUkprn);
            return await _internalApiClient.ProcessIndustryPlacementDetailsAsync(request);
        }

        public (List<SummaryItemModel>, bool) GetIpSummaryDetailsListAsync(IndustryPlacementViewModel cacheModel, IpTempFlexNavigation ipTempFlexNavigation)
        {
            var detailsList = new List<SummaryItemModel>();

            // Validate Ip status
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.Completed &&
                cacheModel?.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration)
                return (null, false);

            var routeAttributes = new Dictionary<string, string> { { Constants.ProfileId, cacheModel.IpCompletion.ProfileId.ToString() }, { Constants.IsChangeMode, "true" } };
            // Status Row
            var statusValue = GetIpStatusValue(cacheModel.IpCompletion.IndustryPlacementStatus);
            detailsList.Add(new SummaryItemModel { Id = "ipstatus", Title = CheckAndSubmitContent.Title_IP_Status_Text, Value = statusValue, ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Ip_Status, RouteName = RouteConstants.IpCompletion, RouteAttributes = routeAttributes });

            // SpecialConsideration Rows
            if (cacheModel.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
            {
                var isScAdded = AddSummaryItemForSpecialConsideration(cacheModel, detailsList);
                if (!isScAdded)
                    return (null, false);
            }

            // IPModel Rows 
            var isIpModelAdded = AddSummaryItemForIpModel(cacheModel, detailsList);
            if (!isIpModelAdded)
                return (null, false);

            // Temp Flexibilities
            var isTempFlexAdded = AddSummaryItemForTempFlexbilities(cacheModel, detailsList, ipTempFlexNavigation);
            if (!isTempFlexAdded)
                return (null, false);

            return (detailsList, true);
        }

        private bool AddSummaryItemForSpecialConsideration(IndustryPlacementViewModel cacheModel, List<SummaryItemModel> detailsList)
        {
            // Load SpecialConsideration questions
            if (cacheModel.SpecialConsideration?.Hours == null || cacheModel.SpecialConsideration?.Reasons == null)
                return false;

            var routeAttributes = new Dictionary<string, string> { { Constants.IsChangeMode, "true" } };

            // Hours Row
            detailsList.Add(new SummaryItemModel { Id = "hours", Title = CheckAndSubmitContent.Title_SpecialConsideration_Hours_Text, Value = cacheModel.SpecialConsideration.Hours.Hours, 
                ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Special_Consideration_Hours, RouteName = RouteConstants.IpSpecialConsiderationHours, RouteAttributes = routeAttributes
            });

            // Reasons Row
            var selectedReasons = cacheModel.SpecialConsideration?.Reasons?.ReasonsList.Where(x => x.IsSelected).Select(x => x.Name);
            detailsList.Add(new SummaryItemModel 
            { 
                Id = "specialreasons",
                Title = CheckAndSubmitContent.Title_SpecialConsideration_Reasons_Text,
                Value = ConvertListToRawHtmlString(selectedReasons),
                ActionText = CheckAndSubmitContent.Link_Change,
                IsRawHtml = true,
                HiddenActionText = CheckAndSubmitContent.Hidden_Text_Special_Consideration_Reasons,
                RouteName = RouteConstants.IpSpecialConsiderationReasons,
                RouteAttributes = routeAttributes
            });

            return true;
        }

        private bool AddSummaryItemForIpModel(IndustryPlacementViewModel cacheModel, List<SummaryItemModel> detailsList)
        {
            if (cacheModel.IpModelViewModel?.IpModelUsed?.IsIpModelUsed == null)
                return false;

            var routeAttribute = new Dictionary<string, string> { { Constants.IsChangeMode, "true" } };

            // IpModelUsed Row
            detailsList.Add(new SummaryItemModel { Id = "isipmodelused", Title = CheckAndSubmitContent.Title_IpModel_Text, Value = cacheModel.IpModelViewModel.IpModelUsed.IsIpModelUsed.Value.ToYesOrNoString() , 
                ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_IpModel_Used, RouteName = RouteConstants.IpModelUsed, RouteAttributes = routeAttribute });

            if (cacheModel.IpModelViewModel.IpModelUsed.IsIpModelUsed == true)
            {
                // MultiEmp Row
                if (cacheModel.IpModelViewModel?.IpMultiEmployerUsed?.IsMultiEmployerModelUsed == null)
                    return false;
                detailsList.Add(new SummaryItemModel { Id = "ismultiempmodel", Title = CheckAndSubmitContent.Title_IpModel_Multi_Emp_Text, Value = cacheModel.IpModelViewModel.IpMultiEmployerUsed.IsMultiEmployerModelUsed.Value.ToYesOrNoString(), 
                    ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_MultiEmp_Used, RouteName = RouteConstants.IpMultiEmployerUsed, RouteAttributes = routeAttribute });

                // OtherIpModelList Row
                if (cacheModel.IpModelViewModel?.IpMultiEmployerUsed?.IsMultiEmployerModelUsed == true)
                {
                    if (cacheModel.IpModelViewModel?.IpMultiEmployerOther?.OtherIpPlacementModels?.Any(x => x.IsSelected) == false)
                        return false;

                    var selectedOtherModels = cacheModel.IpModelViewModel?.IpMultiEmployerOther?.OtherIpPlacementModels
                        .Where(x => x.IsSelected && !x.Name.Equals(Constants.MultipleEmployer, StringComparison.InvariantCultureIgnoreCase))
                        .Select(x => x.Name);
                    if (selectedOtherModels == null)
                        return false;

                    var selectedOtherModelsValue = selectedOtherModels.Any() ? ConvertListToRawHtmlString(selectedOtherModels) : false.ToYesOrNoString();
                    detailsList.Add(new SummaryItemModel { Id = "selectedothermodellist", Title = CheckAndSubmitContent.Title_IpModel_Selected_Other_List_Text, Value = selectedOtherModelsValue, 
                        ActionText = CheckAndSubmitContent.Link_Change, IsRawHtml = true, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Ipmodel_Others_list, RouteName = RouteConstants.IpMultiEmployerOther, RouteAttributes = routeAttribute
                    });
                }
                else
                {
                    // IpModelList Row
                    if (cacheModel.IpModelViewModel?.IpMultiEmployerSelect?.PlacementModels?.Any(x => x.IsSelected) == false)
                        return false;

                    var selectedPlacementModels = cacheModel.IpModelViewModel?.IpMultiEmployerSelect?.PlacementModels.Where(x => x.IsSelected).Select(x => x.Name);
                    if (selectedPlacementModels == null)
                        return false;

                    detailsList.Add(new SummaryItemModel { Id = "selectedplacementmodellist", Title = CheckAndSubmitContent.Title_IpModels_Selected_List_Text, Value = ConvertListToRawHtmlString(selectedPlacementModels), 
                        ActionText = CheckAndSubmitContent.Link_Change, IsRawHtml = true, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Ipmodel_List, RouteName = RouteConstants.IpMultiEmployerSelect, RouteAttributes = routeAttribute
                    });
                }
            }
            return true;
        }

        private bool AddSummaryItemForTempFlexbilities(IndustryPlacementViewModel cacheModel, List<SummaryItemModel> detailsList, IpTempFlexNavigation navigation)
        {
            if (navigation == null)
                return true; // return here for Academic years starting from 2022.

            var routeAttributes = new Dictionary<string, string> { { Constants.IsChangeMode, "true" } };

            if (navigation.AskTempFlexibility)
            {
                // IsTempFlexUsed Row
                if (cacheModel?.TempFlexibility?.IpTempFlexibilityUsed?.IsTempFlexibilityUsed == null)
                    return false;
                detailsList.Add(new SummaryItemModel { Id = "istempflexused", Title = CheckAndSubmitContent.Title_TempFlex_Used_Text, Value = cacheModel?.TempFlexibility?.IpTempFlexibilityUsed?.IsTempFlexibilityUsed.Value.ToYesOrNoString(), ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Tf_TempFlex_Used, RouteName = RouteConstants.IpTempFlexibilityUsed, RouteAttributes = routeAttributes });
            }

            if ((navigation.AskTempFlexibility && navigation.AskBlendedPlacement && cacheModel?.TempFlexibility?.IpTempFlexibilityUsed?.IsTempFlexibilityUsed == true) || // Coming from AskTempFlex
                (!navigation.AskTempFlexibility && navigation.AskBlendedPlacement))  // came directly to blended.
            {
                // IsBlendedPlacementUsed Row
                if (cacheModel?.TempFlexibility?.IpBlendedPlacementUsed?.IsBlendedPlacementUsed == null)
                    return false;
                detailsList.Add(new SummaryItemModel { Id = "isblendedplacementused", Title = CheckAndSubmitContent.Title_BlendedPlacement_Used_Text, Value = cacheModel?.TempFlexibility?.IpBlendedPlacementUsed?.IsBlendedPlacementUsed.Value.ToYesOrNoString(), ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Tf_Blended_Used, RouteName = RouteConstants.IpBlendedPlacementUsed, RouteAttributes = routeAttributes });

                // AnyOtherTempFlex Row (applies only for academicyear-2020 +  Tlevels 'Design,Surveying..' and 'Digital Production..' 
                if (cacheModel?.TempFlexibility?.IpBlendedPlacementUsed?.IsBlendedPlacementUsed == true)
                {
                    // Coming from AskTempFlex - If IsTempFlexibilityUsed == true then IpEmployerLedUsed should have value. If not return false
                    if (cacheModel?.TempFlexibility?.IpTempFlexibilityUsed?.IsTempFlexibilityUsed == true && cacheModel?.TempFlexibility?.IpEmployerLedUsed == null)
                        return false;
                    else if (cacheModel?.TempFlexibility?.IpTempFlexibilityUsed?.IsTempFlexibilityUsed == true && cacheModel?.TempFlexibility?.IpEmployerLedUsed != null)
                    {
                        var selectedTfList = cacheModel?.TempFlexibility?.IpEmployerLedUsed?.TemporaryFlexibilities
                        .Where(x => x.IsSelected && !x.Name.Equals(Constants.BlendedPlacements, StringComparison.InvariantCultureIgnoreCase))
                        .Select(x => x.Name);

                        if (selectedTfList == null)
                            return false;

                        detailsList.Add(new SummaryItemModel { Id = "anyothertempflexlist", Title = CheckAndSubmitContent.Title_TempFlex_Emp_Led_Text, Value = selectedTfList.Any().ToYesOrNoString(), ActionText = CheckAndSubmitContent.Link_Change, IsRawHtml = true, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Tf_Employer_Led_List, RouteName = RouteConstants.IpEmployerLedUsed, RouteAttributes = routeAttributes });
                    }
                }
                else
                {
                    // If IsBlendedPlacementUsed == false, we need to check if coming from AsTempFlex then IpGrantedTempFlexibility should not be null. If so return false
                    if (cacheModel?.TempFlexibility?.IpTempFlexibilityUsed?.IsTempFlexibilityUsed == true && cacheModel?.TempFlexibility?.IpGrantedTempFlexibility == null)
                        return false;

                    TempFlexUsedList(cacheModel, detailsList);
                }                    
            }

            if (navigation.AskTempFlexibility && !navigation.AskBlendedPlacement)
                TempFlexUsedList(cacheModel, detailsList);

            return true;
        }

        private static void TempFlexUsedList(IndustryPlacementViewModel cacheModel, List<SummaryItemModel> detailsList)
        {
            var routeAttributes = new Dictionary<string, string> { { Constants.IsChangeMode, "true" } };
            var selectedTfList = cacheModel?.TempFlexibility?.IpGrantedTempFlexibility?.TemporaryFlexibilities.Where(x => x.IsSelected).Select(x => x.Name);
            if (selectedTfList != null && selectedTfList.Any())
                detailsList.Add(new SummaryItemModel { Id = "tempflexusedlist", Title = CheckAndSubmitContent.Title_TempFlex_Selected_Text, Value = ConvertListToRawHtmlString(selectedTfList), ActionText = CheckAndSubmitContent.Link_Change, IsRawHtml = true, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Tf_Granted_List, RouteName = RouteConstants.IpGrantedTempFlexibility, RouteAttributes = routeAttributes });
        }

        private static string ConvertListToRawHtmlString(IEnumerable<string> selectedList)
        {
            var htmlRawList = selectedList.Select(x => string.Format(CheckAndSubmitContent.Para_Item, x));
            return string.Join(string.Empty, htmlRawList);
        }

        private static string GetIpStatusValue(IndustryPlacementStatus? industryPlacementStatus)
        {
            if (industryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
                return CheckAndSubmitContent.Status_Completed_With_Special_Consideration;

            if (industryPlacementStatus == IndustryPlacementStatus.Completed)
                return CheckAndSubmitContent.Status_Completed;

            return string.Empty;
        }
    }
}
