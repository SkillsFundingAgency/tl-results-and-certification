using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Data;
using System;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers
{
    public static class InMemoryDbContext
    {
        public static ResultsAndCertificationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ResultsAndCertificationDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .EnableSensitiveDataLogging()
           .Options;

            return new ResultsAndCertificationDbContext(options);
        }
    }
}
