using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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

        public async Task<bool> ProcessIndustryPlacementDetailsAsync(IpCheckAndSubmitViewModel viewModel)
        {
            var request = _mapper.Map<IndustryPlacementRequest>(viewModel);
            return await _internalApiClient.ProcessIndustryPlacementDetailsAsync(request);
        }


        public async Task<(List<SummaryItemModel>, bool)> GetIpSummaryDetailsListAsync(IndustryPlacementViewModel cacheModel, int pathwayId, int academicYear)
        {
            var detailsList = new List<SummaryItemModel>();

            // Validate Ip status
            if (cacheModel?.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.Completed &&
                cacheModel?.IpCompletion?.IndustryPlacementStatus != IndustryPlacementStatus.CompletedWithSpecialConsideration)
                return (detailsList, false);

            // Status Row
            var statusValue = GetIpStatusValue(cacheModel.IpCompletion.IndustryPlacementStatus);
            detailsList.Add(new SummaryItemModel { Id = "ipstatus", Title = CheckAndSubmitContent.Title_IP_Status_Text, Value = statusValue, ActionText = CheckAndSubmitContent.Link_Change });

            // SpecialConsideration Rows
            if (cacheModel.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
            {
                // Load SpecialConsideration questions
                if (cacheModel.SpecialConsideration?.Hours == null && cacheModel.SpecialConsideration?.Reasons == null)
                    return (detailsList, false);

                // Hours Row
                detailsList.Add(new SummaryItemModel { Id = "hours", Title = CheckAndSubmitContent.Title_SpecialConsideration_Hours_Text, Value = cacheModel.SpecialConsideration.Hours.Hours, ActionText = CheckAndSubmitContent.Link_Change });

                // Reasons Row
                var selectedReasons = cacheModel.SpecialConsideration.Reasons.ReasonsList.Where(x => x.IsSelected).Select(x => x.Name);
                detailsList.Add(new SummaryItemModel { Id = "specialreasons", Title = CheckAndSubmitContent.Title_SpecialConsideration_Reasons_Text, Value = ConvertListToRawHtmlString(selectedReasons), ActionText = CheckAndSubmitContent.Link_Change, IsRawHtml = true });
            }

            // IPModel Rows 
            if (cacheModel.IpModelViewModel?.IpModelUsed?.IsIpModelUsed == null)
                return (detailsList, false);
            detailsList.Add(new SummaryItemModel { Id = "isipmodelused", Title = CheckAndSubmitContent.Title_IpModel_Text, Value = cacheModel.IpModelViewModel.IpModelUsed.IsIpModelUsed.Value.ToString(), ActionText = CheckAndSubmitContent.Link_Change });

            if (cacheModel.IpModelViewModel.IpModelUsed.IsIpModelUsed == true)
            {
                // MultiEmp Row
                if (cacheModel.IpModelViewModel?.IpMultiEmployerUsed?.IsMultiEmployerModelUsed == null)
                    return (detailsList, false);
                detailsList.Add(new SummaryItemModel { Id = "ismultiempmodel", Title = CheckAndSubmitContent.Title_IpModel_Multi_Emp_Text, Value = cacheModel.IpModelViewModel.IpMultiEmployerUsed.IsMultiEmployerModelUsed.Value.ToString(), ActionText = CheckAndSubmitContent.Link_Change });

                // OtherIpModelList Row
                if (cacheModel.IpModelViewModel?.IpMultiEmployerUsed?.IsMultiEmployerModelUsed == true)
                {
                    if (cacheModel.IpModelViewModel?.IpMultiEmployerOther?.OtherIpPlacementModels.Any(x => x.IsSelected) == false)
                        return (detailsList, false);

                    var selectedOtherModels = cacheModel.IpModelViewModel?.IpMultiEmployerOther?.OtherIpPlacementModels.Where(x => x.IsSelected).Select(x => x.Name);
                    detailsList.Add(new SummaryItemModel { Id = "selectedothermodellist", Title = CheckAndSubmitContent.Title_IpModel_Selected_Other_List_Text, Value = ConvertListToRawHtmlString(selectedOtherModels), ActionText = CheckAndSubmitContent.Link_Change, IsRawHtml = true });
                }
                else
                {
                    // IpModelList Row
                    if (cacheModel.IpModelViewModel?.IpMultiEmployerSelect?.PlacementModels.Any(x => x.IsSelected) == false)
                        return (detailsList, false);

                    var selectedPlacementModels = cacheModel.IpModelViewModel?.IpMultiEmployerSelect?.PlacementModels.Where(x => x.IsSelected).Select(x => x.Name);
                    detailsList.Add(new SummaryItemModel { Id = "selectedplacementmodellist", Title = CheckAndSubmitContent.Title_IpModels_Selected_List_Text, Value = ConvertListToRawHtmlString(selectedPlacementModels), ActionText = CheckAndSubmitContent.Link_Change, IsRawHtml = true });
                }
            }

            // TODO: TempFlexibilities Rows
            var navigation = await GetTempFlexNavigationAsync(pathwayId, academicYear);

            return (detailsList, true);
        }

        private static string ConvertListToRawHtmlString(IEnumerable<string> selectedList)
        {
            var htmlRawList = selectedList.Select(x => $"<p>{x}</p>");
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
