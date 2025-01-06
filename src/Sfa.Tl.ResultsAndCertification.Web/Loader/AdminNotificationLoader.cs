using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AdminNotificationLoader : IAdminNotificationLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _apiClient;
        private readonly IMapper _mapper;

        public AdminNotificationLoader(IResultsAndCertificationInternalApiClient apiClient, IMapper mapper)
        {
            _apiClient = apiClient;
            _mapper = mapper;
        }

        public AdminFindNotificationCriteriaViewModel LoadFilters()
            => new()
            {
                ActiveFilters = new List<FilterLookupData>
                {
                    new()
                    {
                        Id = (int)ActiveFilter.Yes,
                        Name = ActiveFilter.Yes.ToString(),
                        IsSelected = false
                    },
                    new()
                    {
                        Id = (int)ActiveFilter.No,
                        Name = ActiveFilter.No.ToString(),
                        IsSelected = false
                    }
                },
                PageNumber = 1
            };

        public async Task<AdminFindNotificationViewModel> SearchNotificationAsync(AdminFindNotificationCriteriaViewModel criteria)
        {
            AdminSearchNotificationRequest searchRequest = _mapper.Map<AdminSearchNotificationRequest>(criteria);

            PagedResponse<SearchNotificationDetail> banners = await _apiClient.SearchNotificationsAsync(searchRequest);
            AdminFindNotificationViewModel result = _mapper.Map<AdminFindNotificationViewModel>(banners);

            result.SearchCriteria = criteria;
            return result;
        }

        public async Task<AdminNotificationDetailsViewModel> GetNotificationDetailsViewModel(int notificationId)
        {
            GetNotificationResponse response = await _apiClient.GetNotificationAsync(notificationId);
            return _mapper.Map<AdminNotificationDetailsViewModel>(response);
        }

        public async Task<AdminEditNotificationViewModel> GetEditNotificationViewModel(int notificationId)
        {
            GetNotificationResponse response = await _apiClient.GetNotificationAsync(notificationId);
            return _mapper.Map<AdminEditNotificationViewModel>(response);
        }

        public Task<bool> SubmitUpdateNotificationRequest(AdminEditNotificationViewModel viewModel)
        {
            UpdateNotificationRequest request = _mapper.Map<UpdateNotificationRequest>(viewModel);
            return _apiClient.UpdateNotificationAsync(request);
        }

        public Task<AddNotificationResponse> SubmitAddNotificationRequest(AdminAddNotificationViewModel viewModel)
        {
            AddNotificationRequest request = _mapper.Map<AddNotificationRequest>(viewModel);
            return _apiClient.AddNotificationAsync(request);
        }
    }
}