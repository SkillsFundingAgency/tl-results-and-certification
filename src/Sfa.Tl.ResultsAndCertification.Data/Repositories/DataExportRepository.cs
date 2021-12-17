using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class DataExportRepository : IDataExportRepository
    {
        protected readonly ResultsAndCertificationDbContext _dbContext;

        public DataExportRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<RegistrationsExport>> GetDataExportRegistrationsAsync(long aoUkprn)
        {
            // TODO: one from each pathway id?
            // or latest pathway for each profilid.
            var registrations = await _dbContext.TqRegistrationPathway
                   .Include(x => x.TqRegistrationProfile)
                   .Include(x => x.TqProvider)
                       .ThenInclude(x => x.TqAwardingOrganisation)
                           .ThenInclude(x => x.TlPathway)
                   .Include(x => x.TqRegistrationSpecialisms)
                       .ThenInclude(x => x.TlSpecialism)
                   .Where(x => x.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn && (x.Status == RegistrationPathwayStatus.Active || x.Status == RegistrationPathwayStatus.Withdrawn))
                   .Select(x => new RegistrationsExport 
                   { 
                       Uln = x.TqRegistrationProfile.UniqueLearnerNumber.ToString(),
                       FirstName = x.TqRegistrationProfile.Firstname,
                       LastName = x.TqRegistrationProfile.Lastname,
                       DateOfBirth = x.TqRegistrationProfile.DateofBirth.ToShortDateString(), // TODO convert
                       Ukprn = x.TqProvider.TlProvider.UkPrn.ToString(),
                       AcademicYear = x.AcademicYear.ToString(), // TODO: get value
                       Core = x.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                       Specialisms = string.Join(",", x.TqRegistrationSpecialisms.Select(s => s.TlSpecialism.LarId)), // TOOD: null check and constant. 
                       Status = x.Status.ToString()

                   }).ToListAsync();

          return registrations;
        }
    }
}
