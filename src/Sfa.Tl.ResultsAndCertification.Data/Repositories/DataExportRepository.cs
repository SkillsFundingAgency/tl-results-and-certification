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

            return await _dbContext.TqPathwayAssessment
                .Include(pa => pa.TqPathwayResults.Where(pa => pa.IsOptedin && pa.EndDate == null))
                .Include(pa => pa.TqRegistrationPathway)
                .Include(pa => pa.AssessmentSeries)
                .Include(pa => pa.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway)
                .Include(pa => pa.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton)
                .Include(pa => pa.TqRegistrationPathway.TqRegistrationProfile)
                .Include(pa => pa.TqRegistrationPathway.TqProvider)
                .Where(pa => pa.IsOptedin && pa.EndDate == null
                       && pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                       && pa.TqRegistrationPathway.EndDate == null
                       && pa.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn)
                .OrderBy(pa => pa.CreatedOn)
            .Select(pa => new CoreResultsExport
            {
                Uln = pa.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                AcademicYear = academicYear.First(e => e.Year == pa.TqRegistrationPathway.AcademicYear).Name,
                CoreCode = pa.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                CoreAssessmentEntry = pa.AssessmentSeries.Name,
                CoreGrade = pa.TqPathwayResults.Any() ? pa.TqPathwayResults.First().TlLookup.Value : string.Empty
            })
            .ToListAsync();
        }

        public async Task<IList<SpecialismResultsExport>> GetDataExportSpecialismResultsAsync(long aoUkprn)
        {
            var academicYear = _dbContext.AcademicYear;

            return await _dbContext.TqSpecialismAssessment
               .Include(sa => sa.TqSpecialismResults.Where(sa => sa.IsOptedin && sa.EndDate == null))
               .Include(sa => sa.TqRegistrationSpecialism)
               .Include(sa => sa.AssessmentSeries)
               .Include(sa => sa.TqRegistrationSpecialism.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway)
               .Include(sa => sa.TqRegistrationSpecialism.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton)
               .Include(sa => sa.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile)
               .Include(sa => sa.TqRegistrationSpecialism.TqRegistrationPathway.TqProvider)
               .Where(sa => sa.IsOptedin && sa.EndDate == null
                      && sa.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                      && sa.TqRegistrationSpecialism.TqRegistrationPathway.EndDate == null
                      && sa.TqRegistrationSpecialism.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn)
           .Select(sa => new SpecialismResultsExport
           {
               Uln = sa.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
               AcademicYear = academicYear.First(e => e.Year == sa.TqRegistrationSpecialism.TqRegistrationPathway.AcademicYear).Name,
               SpecialismCode = sa.TqRegistrationSpecialism.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
               SpecialismAssessmentEntry = sa.AssessmentSeries.Name,
               SpecialismGrade = sa.TqSpecialismResults.Any() ? sa.TqSpecialismResults.First().TlLookup.Value : string.Empty
           })
           .ToListAsync();
        }

        public async Task<IList<PendingWithdrawalsExport>> GetDataExportPendingWithdrawalsAsync(long aoUkprn)
        {
            var pendingWithdrawals = await _dbContext.TqRegistrationPathway
                   .Where(x => x.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn
                          && (x.Status == RegistrationPathwayStatus.Active)
                          && x.IsPendingWithdrawal)
                   .Select(x => new PendingWithdrawalsExport
                   {
                       Uln = x.TqRegistrationProfile.UniqueLearnerNumber,
                       FirstName = x.TqRegistrationProfile.Firstname,
                       LastName = x.TqRegistrationProfile.Lastname,
                       DateOfBirth = x.TqRegistrationProfile.DateofBirth,
                       Ukprn = x.TqProvider.TlProvider.UkPrn,
                       AcademicYear = x.AcademicYear,
                       Core = x.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                       SpecialismsList = x.TqRegistrationSpecialisms.Where(s => s.IsOptedin).Select(s => s.TlSpecialism.LarId).ToList(),
                       CreatedOn = x.CreatedOn
                   }).ToListAsync();

            return pendingWithdrawals?.GroupBy(x => x.Uln)
                    .Select(x => x.OrderByDescending(o => o.CreatedOn).First())
                    .ToList();
        }

        public async Task<IList<RommsExport>> GetDataExportRommsAsync(long aoUkprn)
        {
            var query = _dbContext.TqRegistrationPathway
                .Include(pr => pr.TqPathwayAssessments.Where(pa => pa.IsOptedin && pa.EndDate == null))
                    .ThenInclude(pa => pa.TqPathwayResults)
                    .ThenInclude(pr => pr.TlLookup)
                .Include(pr => pr.TqRegistrationProfile)
                .Include(pr => pr.TqPathwayAssessments)
                    .ThenInclude(pa => pa.AssessmentSeries)
                .Include(pr => pr.TqProvider)
                    .ThenInclude(tp => tp.TqAwardingOrganisation)
                    .ThenInclude(ao => ao.TlAwardingOrganisaton)
                .Include(pr => pr.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin && rs.EndDate == null))
                    .ThenInclude(rs => rs.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.EndDate == null))
                    .ThenInclude(sa => sa.TqSpecialismResults)
                    .ThenInclude(sr => sr.TlLookup)
                .Include(pr => pr.TqRegistrationSpecialisms)
                    .ThenInclude(rs => rs.TlSpecialism)
                .Where(pr => pr.EndDate == null
                    && pr.Status == RegistrationPathwayStatus.Active
                    && pr.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn
                    && (pr.TqPathwayAssessments.Any(pa => pa.TqPathwayResults.Any(pr => pr.PrsStatus == PrsStatus.UnderReview || pr.PrsStatus == PrsStatus.Reviewed))
                        || pr.TqRegistrationSpecialisms.Any(rs => rs.IsOptedin && rs.EndDate == null && rs.TqSpecialismAssessments.Any(sa => sa.IsOptedin && sa.EndDate == null && sa.TqSpecialismResults.Any(sr => sr.PrsStatus == PrsStatus.UnderReview || sr.PrsStatus == PrsStatus.Reviewed)))));

            var result = await query
                .Select(romm => new RommsExport
                {
                    Uln = romm.TqRegistrationProfile.UniqueLearnerNumber,
                    FirstName = romm.TqRegistrationProfile.Firstname,
                    LastName = romm.TqRegistrationProfile.Lastname,
                    DateOfBirth = romm.TqRegistrationProfile.DateofBirth,
                    Ukprn = romm.TqProvider.TlProvider.UkPrn,
                    AcademicYear = romm.AcademicYear,
                    AssessmentSeriesCore = romm.TqPathwayAssessments.First().AssessmentSeries.Name,
                    CoreComponentCode = romm.TqPathwayAssessments.First().TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                    CoreRommOpen = romm.TqPathwayAssessments.Any(pa => pa.TqPathwayResults.Any(pr => pr.PrsStatus == PrsStatus.UnderReview || pr.PrsStatus == PrsStatus.Reviewed)),
                    CoreRommOutcome = romm.TqPathwayAssessments.First(pa => pa.IsOptedin && pa.EndDate == null).TqPathwayResults.First(pr => pr.IsOptedin && pr.EndDate == null && pr.PrsStatus == PrsStatus.Reviewed || pr.PrsStatus == PrsStatus.BeingAppealed).TlLookup.Value,
                    AssessmentSeriesSpecialisms = romm.TqRegistrationSpecialisms.First().TqSpecialismAssessments.First().AssessmentSeries.Name,
                    SpecialismComponentCode = romm.TqRegistrationSpecialisms.First().TlSpecialism.LarId,
                    SpecialismRommOpen = romm.TqRegistrationSpecialisms.Any(rs => rs.TqSpecialismAssessments.Any(sa => sa.TqSpecialismResults.Any(sr => sr.IsOptedin && sr.EndDate == null && sr.PrsStatus == PrsStatus.UnderReview || sr.PrsStatus == PrsStatus.Reviewed))),
                    SpecialismRommOutcome = romm.TqRegistrationSpecialisms.First().TqSpecialismAssessments.First().TqSpecialismResults.First(sr => sr.IsOptedin && sr.EndDate == null && sr.PrsStatus == PrsStatus.Reviewed || sr.PrsStatus == PrsStatus.BeingAppealed).TlLookup.Value
                })
                .ToListAsync();

            return result;
        }
    }
}