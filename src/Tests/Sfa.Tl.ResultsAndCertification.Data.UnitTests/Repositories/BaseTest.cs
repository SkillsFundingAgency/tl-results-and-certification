using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Data.UnitTests.Repositories
{
    public abstract class BaseTest<T> : IBaseTest<T>  where T : BaseEntity, new()
    {
        protected GenericRepository<T> Repository;
        protected ILogger<GenericRepository<T>> Logger;
        protected ResultsAndCertificationDbContext DbContext;

        public BaseTest()
        {
            Setup();
            Given();
            When();
        }

        public void Setup()
        {
            Logger = Substitute.For<ILogger<GenericRepository<T>>>();
            DbContext = InMemoryDbContext.Create();
            Repository = new GenericRepository<T>(Logger, DbContext);
        }

        public abstract void Given();
        public abstract void When();

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
