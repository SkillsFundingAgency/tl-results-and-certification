using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        protected readonly ResultsAndCertificationDbContext _dbContext;

        public CommonRepository(ResultsAndCertificationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<LoggedInUserTypeInfo> GetLoggedInUserTypeInfoAsync(long ukprn)
        {
            var aoQuery = _dbContext.TqAwardingOrganisation.Where(x => x.IsActive && x.TlAwardingOrganisaton.IsActive).Select(x => new LoggedInUserTypeInfo { Name = x.TlAwardingOrganisaton.Name, Ukprn = x.TlAwardingOrganisaton.UkPrn, UserType = LoginUserType.AwardingOrganisation });
            var tpQuery = _dbContext.TqProvider.Where(x => x.IsActive && x.TlProvider.IsActive).Select(x => new LoggedInUserTypeInfo { Name = x.TlProvider.Name, Ukprn = x.TlProvider.UkPrn, UserType = LoginUserType.TrainingProvider });
            return await aoQuery.Concat(tpQuery).Distinct().FirstOrDefaultAsync(x => x.Ukprn == ukprn);
        }
    }
}
