using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminBannerServiceTests.GetBannerTests
{
    public abstract class GetBannerBaseTest : AdminBannerServiceBaseTest
    {
        protected int BannerId { get; set; }

        protected GetBannerResponse Result { get; set; }

        public override async Task When()
        {
            Result = await AdminBannerService.GetBannerAsync(BannerId);
        }
    }
}