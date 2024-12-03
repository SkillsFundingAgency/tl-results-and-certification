using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IAdminBannerRepository
    {
        Task<PagedResponse<SearchBannerDetail>> SearchBannersAsync(AdminSearchBannerRequest request);
    }
}