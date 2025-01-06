using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class DashboardBannerService : IDashboardBannerService
    {
        private readonly IRepository<Notification> _repository;

        public DashboardBannerService(IRepository<Notification> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<string>> GetAwardingOrganisationBanners(Func<DateTime> getToday)
            => GetBanners(NotificationTarget.AwardingOrganisation, getToday);

        public Task<IEnumerable<string>> GetProviderBanners(Func<DateTime> getToday)
            => GetBanners(NotificationTarget.Provider, getToday);

        private async Task<IEnumerable<string>> GetBanners(NotificationTarget target, Func<DateTime> getToday)
        {
            DateTime today = getToday();
            NotificationTarget[] targets = new[] { target, NotificationTarget.Both };

            IQueryable<string> query = _repository
                .GetManyAsync(b => targets.Contains(b.Target) && b.Start <= today && today <= b.End)
                .OrderBy(b => b.Start)
                .Select(b => b.Content);

            return await query.ToListAsync();
        }
    }
}