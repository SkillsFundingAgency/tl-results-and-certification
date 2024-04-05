using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Service;
using Sfa.Tl.ResultsAndCertification.Data.Factory;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminPostResultsServiceTests
{
    public abstract class AdminPostResultsServiceBaseTest : BaseTest<TqRegistrationPathway>
    {
        protected IAdminPostResultsService CreateAdminPostResultsService()
        {
            IRepositoryFactory repositoryFactory = new RepositoryFactory(new NullLoggerFactory(), DbContext);
            ISystemProvider systemProvider = new SystemProvider();

            return new AdminPostResultsService(repositoryFactory, systemProvider);
        }
    }
}
