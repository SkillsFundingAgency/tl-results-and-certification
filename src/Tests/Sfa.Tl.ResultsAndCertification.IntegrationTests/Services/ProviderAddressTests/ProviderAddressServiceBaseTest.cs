using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderAddressTests
{
    public abstract class ProviderAddressServiceBaseTest : BaseTest<TlProviderAddress>
    {
        protected ProviderAddressService ProviderAddressService;
        protected IRepository<TlProvider> TlProviderRepository;
        protected ILogger<GenericRepository<TlProvider>> TlProviderRepositoryLogger;
        protected ILogger<GenericRepository<TlProviderAddress>> TlProviderAddressLogger;        
        protected IRepository<TlProviderAddress> TlProviderAddressRepository;
        protected IMapper ProviderAddressMapper;
        protected ILogger<ProviderAddressService> ProviderAddressServiceLogger;

        // Data Seed variables        
        protected IEnumerable<TlProvider> TlProviders;
        

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ProviderAddressMapper).Assembly));
            ProviderAddressMapper = new Mapper(mapperConfig);
        }

        protected virtual void SeedTestData()
        {
            TlProviders = ProviderDataProvider.CreateTlProviders(DbContext);
            DbContext.SaveChangesAsync();
        }
    }
    public enum Provider
    {
        BarsleyCollege = 10000536,
        WalsallCollege = 10007315
    }

}
