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
            var registrations = await _dbContext.TqRegistrationPathway
                   .Where(x => x.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn
                          && (x.Status == RegistrationPathwayStatus.Active || x.Status == RegistrationPathwayStatus.Withdrawn))
                   .Select(x => new RegistrationsExport
                   {
                       Uln = x.TqRegistrationProfile.UniqueLearnerNumber,
                       FirstName = x.TqRegistrationProfile.Firstname,
                       LastName = x.TqRegistrationProfile.Lastname,
                       DateOfBirth = x.TqRegistrationProfile.DateofBirth,
                       Ukprn = x.TqProvider.TlProvider.UkPrn,
                       AcademicYear = x.AcademicYear,
                       Core = x.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                       SpecialismsList = x.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null).Select(s => s.TlSpecialism.LarId).ToList(),
                       Status = x.Status.ToString(),
                       CreatedOn = x.CreatedOn
                   }).ToListAsync();

            if (registrations == null) return null;

            var latestRegistratons = registrations
                    .GroupBy(x => x.Uln)
                    .Select(x => x.OrderByDescending(o => o.CreatedOn).Take(1))
                    .ToList();

            return registrations;
        }

        public async Task<IList<CoreAssessmentsExport>> GetDataExportCoreAssessmentsAsync(long aoUkprn)
        {
            return await _dbContext.TqPathwayAssessment
                .Where(pa => pa.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn
                       && pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                       && pa.TqRegistrationPathway.EndDate == null
                       && pa.IsOptedin && pa.EndDate == null)
                .OrderByDescending(pa => pa.CreatedOn)
                .Select(pa => new CoreAssessmentsExport
                {
                    Uln = pa.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                    CoreCode = pa.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                    CoreAssessmentEntry = pa.AssessmentSeries.Name
                }).ToListAsync();
        }

        public async Task<IList<SpecialismAssessmentsExport>> GetDataExportSpecialismAssessmentsAsync(long aoUkprn)
        {
            return await _dbContext.TqSpecialismAssessment
                .Where(sa => sa.TqRegistrationSpecialism.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn
                       && sa.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                       && sa.TqRegistrationSpecialism.IsOptedin && sa.TqRegistrationSpecialism.EndDate == null
                       && sa.IsOptedin && sa.EndDate == null)
                .OrderByDescending(pa => pa.CreatedOn)
                .Select(sa => new SpecialismAssessmentsExport
                {
                    Uln = sa.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                    SpecialismCode = sa.TqRegistrationSpecialism.TlSpecialism.LarId,
                    SpecialismAssessmentEntry = sa.AssessmentSeries.Name
                }).ToListAsync();
        }

        public async Task<IList<CoreResultsExport>> GetDataExportCoreResultsAsync(long aoUkprn)
        {
            // TODO: 
            return await _dbContext.TqPathwayAssessment
                .Where(pa => pa.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn
                       && pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                       && pa.TqRegistrationPathway.EndDate == null
                       && pa.IsOptedin && pa.EndDate == null)
                .OrderByDescending(pa => pa.CreatedOn)
                .Select(pa => new CoreResultsExport
                {
                    Uln = pa.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                    CoreCode = pa.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                    CoreAssessmentEntry = pa.AssessmentSeries.Name
                }).ToListAsync();
        }

        public async Task<IList<SpecialismResultsExport>> GetDataExportSpecialismResultsAsync(long aoUkprn)
        {
            // TODO: Next story.
            await Task.CompletedTask;
            return new List<SpecialismResultsExport>();
        }
    }
}
