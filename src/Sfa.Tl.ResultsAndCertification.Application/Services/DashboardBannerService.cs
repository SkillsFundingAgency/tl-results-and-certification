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
        private readonly IRepository<Banner> _repository;

        public DashboardBannerService(IRepository<Banner> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<string>> GetAwardingOrganisationBanners(Func<DateTime> getToday)
            => GetBanners(BannerTarget.AwardingOrganisation, getToday);

        public Task<IEnumerable<string>> GetProviderBanners(Func<DateTime> getToday)
            => GetBanners(BannerTarget.Provider, getToday);

        private async Task<IEnumerable<string>> GetBanners(BannerTarget target, Func<DateTime> getToday)
        {
            DateTime today = getToday();
            BannerTarget[] targets = new[] { target, BannerTarget.Both };

            IQueryable<string> query = _repository
                .GetManyAsync(b => targets.Contains(b.Target) && b.IsOptedin && b.Start <= today && today <= b.End)
                .Select(b => b.Content);

            return await query.ToListAsync();
        }
    }
}