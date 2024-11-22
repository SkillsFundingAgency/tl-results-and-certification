using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminProviderServiceTests.UpdateProviderTests
{
    public abstract class UpdateProviderBaseTest : AdminProviderServiceBaseTest
    {
        protected IAdminProviderService AdminProviderService => CreateAdminProviderService();
        protected UpdateProviderRequest Request { get; set; }
        protected UpdateProviderResponse Result { get; set; }

        protected static DateTime Now => new(2024, 1, 1);

        public override async Task When()
        {
            Result = await AdminProviderService.UpdateProviderAsync(Request, () => Now);
        }
    }
}