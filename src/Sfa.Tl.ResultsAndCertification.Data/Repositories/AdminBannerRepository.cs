using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class AdminBannerRepository : IAdminBannerRepository
    {
        private readonly ResultsAndCertificationDbContext _dbContext;

        public AdminBannerRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<SearchBannerDetail>> SearchBannersAsync(AdminSearchBannerRequest request)
        {
            IQueryable<Banner> query = _dbContext.Banner.AsQueryable();

            int totalCount = query.Count();

            if (request.SelectActiveBanners.HasValue)
            {
                bool selectActive = request.SelectActiveBanners.Value;
                query = query.Where(p => p.IsOptedin == selectActive);
            }

            int filteredRecordsCount = await query.CountAsync();
            var pager = new Pager(filteredRecordsCount, request.PageNumber, 10);

            IQueryable<SearchBannerDetail> bannerQueryable = query
                .Select(x => new SearchBannerDetail
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    Target = x.Target,
                    Start = x.End,
                    End = x.End,
                    IsOptedin = x.IsOptedin
                })
                .OrderByDescending(x => x.Id)
                .Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

            List<SearchBannerDetail> banners = await bannerQueryable.ToListAsync();
            return new PagedResponse<SearchBannerDetail> { Records = banners, TotalRecords = totalCount, PagerInfo = pager };
        }
    }
}