using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAdminNotificationService
    {
        Task<PagedResponse<SearchNotificationDetail>> SearchNotificationsAsync(AdminSearchNotificationRequest request);

        Task<GetNotificationResponse> GetNotificationAsync(int notificationId);

        Task<AddNotificationResponse> AddNotificationAsync(AddNotificationRequest request);

        Task<bool> UpdateNotificationAsync(UpdateNotificationRequest request, Func<DateTime> getNow);
    }
}