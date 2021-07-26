using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class PrintingRepository : IPrintingRepository
    {
        protected readonly ResultsAndCertificationDbContext _dbContext;
        private readonly ILogger<IPrintingRepository> _logger;

        public PrintingRepository(ResultsAndCertificationDbContext dbContext, ILogger<IPrintingRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IList<Batch>> GetPendingPrintRequestAsync()
        {
            var batches = await _dbContext.Batch
                .Where(b => b.Status == BatchStatus.Created)
                .Include(x => x.PrintBatchItems)
                    .ThenInclude(x => x.PrintCertificates)
                .Include(x => x.PrintBatchItems)
                    .ThenInclude(x => x.TlProviderAddress)
                        .ThenInclude(x => x.TlProvider)
                .ToListAsync();
            return batches;
        }
    }
}
