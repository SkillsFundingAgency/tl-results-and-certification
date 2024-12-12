using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class AdminNotificationRepository : IAdminNotificationRepository
    {
        private readonly ResultsAndCertificationDbContext _dbContext;

        public AdminNotificationRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<SearchNotificationDetail>> SearchNotificationsAsync(AdminSearchNotificationRequest request, Func<DateTime> getToday)
        {
            DateTime today = getToday();

            IQueryable<Notification> query = _dbContext.Notification.AsQueryable();

            int totalCount = query.Count();

            if (!request.SelectedActive.IsNullOrEmpty() && request.SelectedActive.Count() < 2)
            {
                if (request.SelectedActive.Contains((int)ActiveFilter.Yes))
                {
                    query = query.Where(p => p.Start <= today && today <= p.End);
                }

                if (request.SelectedActive.Contains((int)ActiveFilter.No))
                {
                    query = query.Where(b => b.Start > today || today > b.End);
                }
            }

            int filteredRecordsCount = await query.CountAsync();
            var pager = new Pager(filteredRecordsCount, request.PageNumber, 10);

            IQueryable<SearchNotificationDetail> bannerQueryable = query
                .Select(x => new SearchNotificationDetail
                {
                    Id = x.Id,
                    Title = x.Title,
                    Target = x.Target,
                    End = x.End,
                    IsActive = x.Start <= today && today <= x.End
                })
                .OrderByDescending(x => x.Id)
                .Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

            List<SearchNotificationDetail> banners = await bannerQueryable.ToListAsync();
            return new PagedResponse<SearchNotificationDetail> { Records = banners, TotalRecords = totalCount, PagerInfo = pager };
        }
    }
}