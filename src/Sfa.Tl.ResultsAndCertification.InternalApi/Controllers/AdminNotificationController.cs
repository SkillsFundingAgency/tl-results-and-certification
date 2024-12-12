using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminNotificationController : ControllerBase
    {
        private readonly IAdminNotificationService _adminNotificationService;

        public AdminNotificationController(IAdminNotificationService adminBannerService)
        {
            _adminNotificationService = adminBannerService;
        }

        [HttpPost]
        [Route("SearchNotifications")]
        public Task<PagedResponse<SearchNotificationDetail>> SearchBannersAsync(AdminSearchNotificationRequest request)
            => _adminNotificationService.SearchNotificationsAsync(request);

        [HttpGet]
        [Route("GetNotification/{notificationId}")]
        public Task<GetNotificationResponse> GetBannerAsync(int notificationId)
             => _adminNotificationService.GetNotificationAsync(notificationId);

        [HttpPost]
        [Route("AddNotification")]
        public Task<AddNotificationResponse> AddBannerAsync(AddNotificationRequest request)
            => _adminNotificationService.AddNotificationAsync(request);

        [HttpPut]
        [Route("UpdateNotification")]
        public Task<bool> UpdateBannerAsync(UpdateNotificationRequest request)
            => _adminNotificationService.UpdateNotificationAsync(request, () => DateTime.UtcNow);
    }
}