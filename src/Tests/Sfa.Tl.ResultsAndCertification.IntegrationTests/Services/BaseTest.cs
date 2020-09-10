using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Respawn;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services
{
    public abstract class BaseTest<T> : IBaseTest<T> where T : BaseEntity, new()
    {
        protected GenericRepository<T> Repository;
        protected ILogger<GenericRepository<T>> Logger;
        public ResultsAndCertificationDbContext DbContext;
        public Checkpoint DbCheckpoint;
        private bool _isRelationalDb;

        public BaseTest(bool isRelationalDb = false)
        {
            _isRelationalDb = isRelationalDb;
            Setup();
            Given();
            When();
        }

        public void Setup()
        {
            Logger = Substitute.For<ILogger<GenericRepository<T>>>();
            DbContext = _isRelationalDb ? TestDatabaseConfiguration.CreateRelationalDbContext() : InMemoryDbContext.Create();
            Repository = new GenericRepository<T>(Logger, DbContext);
            DbCheckpoint = _isRelationalDb ? new Checkpoint { WithReseed = true } : null;
        }

        public abstract void Given();
        public abstract void When();

        public void Dispose()
        {
            if (_isRelationalDb)
            {
                DbCheckpoint?.Reset(TestDatabaseConfiguration.GetConnectionString()).GetAwaiter().GetResult();
            }
            DbContext?.Dispose();
        }

        public void DetachAll()
        {
            EntityEntry[] entityEntries = DbContext.ChangeTracker.Entries().ToArray();

            foreach (EntityEntry entityEntry in entityEntries)
            {
                entityEntry.State = EntityState.Detached;
            }
        }

        public void DetachEntity<TEntity>() where TEntity : class
        {
            EntityEntry[] entityEntries = DbContext.ChangeTracker.Entries<TEntity>().ToArray();

            foreach (EntityEntry entityEntry in entityEntries)
            {
                entityEntry.State = EntityState.Detached;
            }
        }        
    }
}
