using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Data.Factory
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ResultsAndCertificationDbContext _dbContext;

        public RepositoryFactory(ILoggerFactory loggerFactory, ResultsAndCertificationDbContext dbContext)
        {
            _loggerFactory = loggerFactory;
            _dbContext = dbContext;
        }

        public IRepository<T> GetRepository<T>() where T : BaseEntity, new()
            => new GenericRepository<T>(_loggerFactory.CreateLogger<GenericRepository<T>>(), _dbContext);
    }
}