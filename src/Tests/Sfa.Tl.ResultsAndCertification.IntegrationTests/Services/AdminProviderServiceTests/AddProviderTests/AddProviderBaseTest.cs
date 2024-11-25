using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminProviderServiceTests.AddProviderTests
{
    public abstract class AddProviderBaseTest : AdminProviderServiceBaseTest
    {
        protected IAdminProviderService AdminProviderService => CreateAdminProviderService();
        protected AddProviderRequest Request { get; set; }
        protected AddProviderResponse Result { get; set; }

        protected IList<TlProvider> ProvidersDb { get; set; }

        public override async Task When()
        {
            Result = await AdminProviderService.AddProviderAsync(Request);
        }

        protected void SeedTestData()
        {
            TlProviderBuilder providerBuilder = new();
            ProvidersDb = providerBuilder.BuildList();

            DbContext.AddRange(ProvidersDb);
            DbContext.SaveChanges();
        }
    }
}