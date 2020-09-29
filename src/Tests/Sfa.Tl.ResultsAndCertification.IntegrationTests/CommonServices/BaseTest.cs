using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices
{
    public abstract class BaseTest : IBaseTest
    {
        protected ResultsAndCertificationDbContext DbContext;

        public BaseTest()
        {
            Setup();
            Given();
            When();
        }

        public void Setup()
        {
            DbContext = InMemoryDbContext.Create();
        }

        public abstract void Given();
        public abstract Task When();

        public void Dispose()
        {
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
