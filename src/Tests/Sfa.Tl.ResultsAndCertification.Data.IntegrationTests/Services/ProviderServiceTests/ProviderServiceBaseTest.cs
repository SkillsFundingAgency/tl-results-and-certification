using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests
{
    public abstract class ProviderServiceBaseTest : BaseTest<TqProvider>
    {
        protected IMapper ProviderMapper;
        protected ProviderService ProviderService;
        protected TlRoute Route;
        protected TlPathway Pathway;
        protected TlProvider TlProvider;
        protected TlAwardingOrganisation TlAwardingOrganisation;
        protected TqAwardingOrganisation TqAwardingOrganisation;        
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected ILogger<GenericRepository<TlProvider>> TlProviderRepositoryLogger;
        protected IRepository<TlProvider> TlproviderRepository;
        protected IProviderRepository ProviderRepository;
        protected ILogger<ProviderRepository> ProviderRepositoryLogger;

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ProviderMapper).Assembly));
            ProviderMapper = new Mapper(mapperConfig);
        }

        protected virtual void SeedTestData()
        {
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, EnumAwardingOrganisation.Pearson);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, EnumAwardingOrganisation.Pearson);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, EnumAwardingOrganisation.Pearson, Route);
            TqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Route, Pathway, TlAwardingOrganisation);
            DbContext.SaveChangesAsync();
        }
    }
}
