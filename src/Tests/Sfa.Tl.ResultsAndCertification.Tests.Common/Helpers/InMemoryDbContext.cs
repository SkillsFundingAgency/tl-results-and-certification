using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
           .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
           .EnableSensitiveDataLogging()
           .Options;

            return new ResultsAndCertificationDbContext(options);
        }
    }
}
