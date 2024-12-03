using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IAdminBannerService
    {
        Task<PagedResponse<SearchBannerDetail>> SearchBannersAsync(AdminSearchBannerRequest request);

        Task<GetBannerResponse> GetBannerAsync(int bannerId);

        Task<AddBannerResponse> AddBannerAsync(AddBannerRequest request);

        Task<bool> UpdateBannerAsync(UpdateBannerRequest request, Func<DateTime> getNow);
    }
}