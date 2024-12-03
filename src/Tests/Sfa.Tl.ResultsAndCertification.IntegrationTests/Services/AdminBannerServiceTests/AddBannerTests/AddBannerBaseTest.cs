using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminBannerServiceTests.AddBannerTests
{
    public abstract class AddBannerBaseTest : AdminBannerServiceBaseTest
    {
        protected AddBannerRequest Request { get; set; }

        protected AddBannerResponse Result { get; set; }

        public override async Task When()
        {
            Result = await AdminBannerService.AddBannerAsync(Request);
        }
    }
}