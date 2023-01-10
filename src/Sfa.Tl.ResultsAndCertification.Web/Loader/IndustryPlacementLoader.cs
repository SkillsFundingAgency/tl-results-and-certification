using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement.IpCheckAndSubmit;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.IO;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class IndustryPlacementLoader : IIndustryPlacementLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;
        private readonly IMapper _mapper;
        private readonly IBlobStorageService _blobStorageService;

        public IndustryPlacementLoader(IResultsAndCertificationInternalApiClient internalApiClient, IMapper mapper, IBlobStorageService blobStorageService = null)
        {
            _internalApiClient = internalApiClient;
            _mapper = mapper;
            _blobStorageService = blobStorageService;
        }

        public async Task<UploadIndustryPlacementsResponseViewModel> ProcessBulkIndustryPlacementsAsync(UploadIndustryPlacementsRequestViewModel viewModel)
        {
            var bulkIndustryPlacementsRequest = _mapper.Map<BulkProcessRequest>(viewModel);

            using (var fileStream = viewModel.File.OpenReadStream())
            {
                await _blobStorageService.UploadFileAsync(new BlobStorageData
                {
                    ContainerName = bulkIndustryPlacementsRequest.DocumentType.ToString(),
                    BlobFileName = bulkIndustryPlacementsRequest.BlobFileName,
                    SourceFilePath = $"{bulkIndustryPlacementsRequest.AoUkprn}/{BulkProcessStatus.Processing}",
                    FileStream = fileStream,
                    UserName = bulkIndustryPlacementsRequest.PerformedBy
                });
            }

            var bulkIndustryPlacementsResponse = await _internalApiClient.ProcessBulkIndustryPlacementsAsync(bulkIndustryPlacementsRequest);
            return _mapper.Map<UploadIndustryPlacementsResponseViewModel>(bulkIndustryPlacementsResponse);
        }

        public async Task<Stream> GetIndustryPlacementValidationErrorsFileAsync(long aoUkprn, Guid blobUniqueReference)
        {
            var documentInfo = await _internalApiClient.GetDocumentUploadHistoryDetailsAsync(aoUkprn, blobUniqueReference);

            if (documentInfo != null && documentInfo.Status == (int)DocumentUploadStatus.Failed)
            {
                var fileStream = await _blobStorageService.DownloadFileAsync(new BlobStorageData
                {
                    ContainerName = DocumentType.IndustryPlacements.ToString(),
                    BlobFileName = documentInfo.BlobFileName,
                    SourceFilePath = $"{aoUkprn}/{BulkProcessStatus.ValidationErrors}"
                });

                if (fileStream == null)
                {
                    var blobReadError = $"No FileStream found to download assessment validation errors. Method: DownloadFileAsync(ContainerName: {DocumentType.IndustryPlacements}, BlobFileName = {documentInfo.BlobFileName}, SourceFilePath = {aoUkprn}/{BulkProcessStatus.ValidationErrors})";
                    //_logger.LogWarning(LogEvent.FileStreamNotFound, blobReadError);
                }
                return fileStream;
            }
            else
            {
                //_logger.LogWarning(LogEvent.NoDataFound, $"No DocumentUploadHistoryDetails found or the request is not valid. Method: GetDocumentUploadHistoryDetailsAsync(AoUkprn: {aoUkprn}, BlobUniqueReference = {blobUniqueReference})");
                return null;
            }
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

        public async Task<bool> ProcessIndustryPlacementDetailsAsync(long providerUkprn, IndustryPlacementViewModel viewModel)
        {
            var request = _mapper.Map<IndustryPlacementRequest>
                (viewModel.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.NotCompleted || viewModel.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.WillNotComplete
                    ? viewModel.IpCompletion : viewModel,  // If CompletedWithSpecialConsideration then map to store json data.
                opt => opt.Items["providerUkprn"] = providerUkprn);
            return await _internalApiClient.ProcessIndustryPlacementDetailsAsync(request);
        }

        public (List<SummaryItemModel>, bool) GetIpSummaryDetailsListAsync(IndustryPlacementViewModel cacheModel)
        {
            var detailsList = new List<SummaryItemModel>();

            // Validate Ip status
            if (!EnumExtensions.IsValidValue<IndustryPlacementStatus>(cacheModel?.IpCompletion?.IndustryPlacementStatus, exclNotSpecified: true))
                return (null, false);

            // Status Row
            var statusValue = GetIpStatusValue(cacheModel.IpCompletion.IndustryPlacementStatus);
            var routeAttributes = new Dictionary<string, string> { { Constants.ProfileId, cacheModel.IpCompletion.ProfileId.ToString() }, { Constants.IsChangeMode, "true" } };
            detailsList.Add(new SummaryItemModel { Id = "ipstatus", Title = CheckAndSubmitContent.Title_IP_Status_Text, Value = statusValue, ActionText = CheckAndSubmitContent.Link_Change, HiddenActionText = CheckAndSubmitContent.Hidden_Text_Ip_Status, RouteName = RouteConstants.IpCompletion, RouteAttributes = routeAttributes });

            // SpecialConsideration Rows
            if (cacheModel.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
            {
                var isScAdded = AddSummaryItemForSpecialConsideration(cacheModel, detailsList);
                if (!isScAdded)
                    return (null, false);
            }

            return (detailsList, true);
        }

        public NotificationBannerModel GetSuccessNotificationBanner(IndustryPlacementStatus? industryPlacementStatus)
        {
            string message;
            if (industryPlacementStatus == null)
                message = string.Empty;
            else
            {
                message = industryPlacementStatus.Value switch
                {
                    IndustryPlacementStatus.Completed => IndustryPlacementBanner.Success_Message_Completed,
                    IndustryPlacementStatus.CompletedWithSpecialConsideration => IndustryPlacementBanner.Success_Message_Completed_With_Special_Consideration,
                    IndustryPlacementStatus.NotCompleted => IndustryPlacementBanner.Success_Message_Still_Need_To_Complete,
                    IndustryPlacementStatus.WillNotComplete => IndustryPlacementBanner.Success_Message_Will_Not_Complete,
                    _ => string.Empty,
                };
            }
            
            var notificationBanner = new NotificationBannerModel
            {
                HeaderMessage = IndustryPlacementBanner.Banner_HeaderMesage,
                Message = message,
                DisplayMessageBody = true,
                IsRawHtml = true
            };

            return notificationBanner;
        }

        private bool AddSummaryItemForSpecialConsideration(IndustryPlacementViewModel cacheModel, List<SummaryItemModel> detailsList)
        {
            // Load SpecialConsideration questions
            if (cacheModel.SpecialConsideration?.Hours == null || cacheModel.SpecialConsideration?.Reasons == null)
                return false;

            var routeAttributes = new Dictionary<string, string> { { Constants.IsChangeMode, "true" } };

            // Hours Row
            detailsList.Add(new SummaryItemModel
            {
                Id = "hours",
                Title = CheckAndSubmitContent.Title_SpecialConsideration_Hours_Text,
                Value = cacheModel.SpecialConsideration.Hours.Hours,
                ActionText = CheckAndSubmitContent.Link_Change,
                HiddenActionText = CheckAndSubmitContent.Hidden_Text_Special_Consideration_Hours,
                RouteName = RouteConstants.IpSpecialConsiderationHours,
                RouteAttributes = routeAttributes
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

        private static string ConvertListToRawHtmlString(IEnumerable<string> selectedList)
        {
            var htmlRawList = selectedList.Select(x => string.Format(CheckAndSubmitContent.Para_Item, x));
            return string.Join(string.Empty, htmlRawList);
        }

        private static string GetIpStatusValue(IndustryPlacementStatus? industryPlacementStatus)
        {
            if (!industryPlacementStatus.HasValue)
                return string.Empty;

            return industryPlacementStatus.Value switch
            {
                IndustryPlacementStatus.Completed => CheckAndSubmitContent.Status_Completed,
                IndustryPlacementStatus.CompletedWithSpecialConsideration => CheckAndSubmitContent.Status_Completed_With_Special_Consideration,
                IndustryPlacementStatus.NotCompleted => CheckAndSubmitContent.Status_Not_Completed,
                IndustryPlacementStatus.WillNotComplete => CheckAndSubmitContent.Status_Will_Not_Complete,
                _ => string.Empty,
            };
        }
    }
}
