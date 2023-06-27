using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
                       SpecialismsList = x.TqRegistrationSpecialisms.Where(s => s.IsOptedin).Select(s => s.TlSpecialism.LarId).ToList(),
                       Status = x.Status.ToString(),
                       CreatedOn = x.CreatedOn
                   }).ToListAsync();

            if (registrations == null) return null;

            var latestRegistrations = registrations
                    .GroupBy(x => x.Uln)
                    .Select(x => x.OrderByDescending(o => o.CreatedOn).First())
                    .ToList();

            return latestRegistrations;
        }

        public async Task<IList<CoreAssessmentsExport>> GetDataExportCoreAssessmentsAsync(long aoUkprn)
        {
            var academicYears = _dbContext.AcademicYear.AsQueryable();

            return await _dbContext.TqPathwayAssessment
                   .Include(i => i.TqRegistrationPathway)
               .Where(pa => pa.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn
                      && pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                      && pa.TqRegistrationPathway.EndDate == null
                      && pa.IsOptedin && pa.EndDate == null)
               .OrderByDescending(pa => pa.CreatedOn)
               .Select(pa => new CoreAssessmentsExport
               {
                   Uln = pa.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                   StartYear = academicYears.First(f => f.Year == pa.TqRegistrationPathway.AcademicYear).Name,
                   CoreCode = pa.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                   CoreAssessmentEntry = pa.AssessmentSeries.Name
               }).ToListAsync();         
        }

        public async Task<IList<SpecialismAssessmentsExport>> GetDataExportSpecialismAssessmentsAsync(long aoUkprn)
        {
            var academicYears = _dbContext.AcademicYear.AsQueryable();

            return await _dbContext.TqSpecialismAssessment
                    .Include(i => i.TqRegistrationSpecialism)
                        .ThenInclude(i => i.TqRegistrationPathway)
                .Where(sa => sa.TqRegistrationSpecialism.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn
                       && sa.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                       && sa.TqRegistrationSpecialism.IsOptedin && sa.TqRegistrationSpecialism.EndDate == null
                       && sa.IsOptedin && sa.EndDate == null)
                .OrderByDescending(pa => pa.CreatedOn)
                .Select(sa => new SpecialismAssessmentsExport
                {
                    Uln = sa.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,                    
                    StartYear = academicYears.First(f => f.Year == sa.TqRegistrationSpecialism.TqRegistrationPathway.AcademicYear).Name,
                    SpecialismCode = sa.TqRegistrationSpecialism.TlSpecialism.LarId,
                    SpecialismAssessmentEntry = sa.AssessmentSeries.Name
                }).ToListAsync();
        }

        public async Task<IList<CoreResultsExport>> GetDataExportCoreResultsAsync(long aoUkprn)
        {
            var academicYear = _dbContext.AcademicYear;

            return await _dbContext.TqPathwayResult
                .Where(pr => pr.TqPathwayAssessment.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn
                       && pr.TqPathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                       && pr.TqPathwayAssessment.TqRegistrationPathway.EndDate == null
                       && pr.TqPathwayAssessment.IsOptedin && pr.TqPathwayAssessment.EndDate == null
                       && pr.IsOptedin && pr.EndDate == null)
                .OrderByDescending(pa => pa.CreatedOn)
                .Select(pr => new CoreResultsExport
                {
                    Uln = pr.TqPathwayAssessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                    AcademicYear = academicYear.First(e => e.Year == pr.TqPathwayAssessment.TqRegistrationPathway.AcademicYear).Name,
                    CoreCode = pr.TqPathwayAssessment.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                    CoreAssessmentEntry = pr.TqPathwayAssessment.AssessmentSeries.Name,
                    CoreGrade = pr.TlLookup.Value
                }).ToListAsync();
        }

        public async Task<IList<SpecialismResultsExport>> GetDataExportSpecialismResultsAsync(long aoUkprn)
        {
            var academicYear = _dbContext.AcademicYear;

            return await _dbContext.TqSpecialismResult
                .Where(sr => sr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn
                       && sr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                       && sr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.EndDate == null
                       && sr.TqSpecialismAssessment.IsOptedin && sr.TqSpecialismAssessment.EndDate == null
                       && sr.IsOptedin && sr.EndDate == null)
                .OrderByDescending(pa => pa.CreatedOn)
                .Select(sr => new SpecialismResultsExport
                {
                    Uln = sr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                    AcademicYear = academicYear.First(e => e.Year == sr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.AcademicYear).Name,
                    SpecialismCode = sr.TqSpecialismAssessment.TqRegistrationSpecialism.TlSpecialism.LarId,
                    SpecialismAssessmentEntry = sr.TqSpecialismAssessment.AssessmentSeries.Name,
                    SpecialismGrade = sr.TlLookup.Value
                }).ToListAsync();
        }
    }
}
