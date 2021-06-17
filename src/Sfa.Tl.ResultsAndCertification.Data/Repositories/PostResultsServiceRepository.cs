using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class PostResultsServiceRepository : IPostResultsServiceRepository
    {
        protected readonly ResultsAndCertificationDbContext _dbContext;

        public PostResultsServiceRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TqRegistrationPathway> FindPrsLearnerRecordAsync(long aoUkprn, long uln)
        {
            await Task.CompletedTask;

            var regPathway = await _dbContext.TqRegistrationPathway
                .Include(x => x.TqRegistrationProfile)
                .Include(x => x.TqProvider)
                    .ThenInclude(x => x.TqAwardingOrganisation)
                        .ThenInclude(x => x.TlAwardingOrganisaton)
                .Include(x => x.TqProvider)
                    .ThenInclude(x => x.TqAwardingOrganisation)
                        .ThenInclude(x => x.TlPathway)
                .Include(x => x.TqProvider)
                    .ThenInclude(x => x.TlProvider)
                .OrderByDescending(o => o.CreatedOn)
                .FirstOrDefaultAsync(p => p.TqRegistrationProfile.UniqueLearnerNumber == uln &&
                       p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn &&
                       (
                            p.Status == RegistrationPathwayStatus.Active || p.Status == RegistrationPathwayStatus.Withdrawn
                       ));

            return regPathway;
        }
    }
}
