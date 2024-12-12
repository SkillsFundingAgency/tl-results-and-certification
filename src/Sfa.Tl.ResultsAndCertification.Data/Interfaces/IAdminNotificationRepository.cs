using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IAdminNotificationRepository
    {
        Task<PagedResponse<SearchNotificationDetail>> SearchNotificationsAsync(AdminSearchNotificationRequest request, Func<DateTime> getToday);
    }
}