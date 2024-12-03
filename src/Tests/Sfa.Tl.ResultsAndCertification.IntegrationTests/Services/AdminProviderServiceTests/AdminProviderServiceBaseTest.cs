using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminProviderServiceTests
{
    public abstract class AdminProviderServiceBaseTest : BaseTest<TqProvider>
    {
        protected IAdminProviderService CreateAdminProviderService()
        {
            var logger = new Logger<GenericRepository<TlProvider>>(new NullLoggerFactory());
            var repository = new GenericRepository<TlProvider>(logger, DbContext);
            IMapper mapper = CreateMapper();

            return new AdminProviderService(repository, mapper);
        }

        private static Mapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AdminProviderMapper).Assembly));
            return new Mapper(mapperConfig);
        }
    }
}