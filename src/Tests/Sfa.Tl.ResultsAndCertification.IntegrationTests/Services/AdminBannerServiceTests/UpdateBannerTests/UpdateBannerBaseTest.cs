using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminBannerServiceTests.UpdateBannerTests
{
    public abstract class UpdateBannerBaseTest : AdminBannerServiceBaseTest
    {
        protected UpdateBannerRequest Request { get; set; }

        protected bool Result { get; set; }

        protected static DateTime Now => new(2024, 1, 1);

        public override async Task When()
        {
            Result = await AdminBannerService.UpdateBannerAsync(Request, () => Now);
        }
    }
}