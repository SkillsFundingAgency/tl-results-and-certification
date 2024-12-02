using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminProviderServiceTests.UpdateProviderTests
{
    public abstract class UpdateProviderBaseTest : AdminProviderServiceBaseTest
    {
        protected IAdminProviderService AdminProviderService => CreateAdminProviderService();
        protected UpdateProviderRequest Request { get; set; }
        protected UpdateProviderResponse Result { get; set; }

        protected static DateTime Now => new(2024, 1, 1);

        protected IList<TlProvider> ProvidersDb { get; set; }

        public override async Task When()
        {
            Result = await AdminProviderService.UpdateProviderAsync(Request, () => Now);
        }

        protected TlProvider SeedTestData()
        {
            TlProviderBuilder providerBuilder = new();
            ProvidersDb = providerBuilder.BuildList();

            DbContext.AddRange(ProvidersDb);
            DbContext.SaveChanges();

            return ProvidersDb.First();
        }
    }
}