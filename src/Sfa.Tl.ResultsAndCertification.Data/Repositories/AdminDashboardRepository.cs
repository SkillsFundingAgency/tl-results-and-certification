using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly ResultsAndCertificationDbContext _dbContext;

        public AdminDashboardRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<FilterLookupData>> GetAwardingOrganisationFiltersAsync()
        {
            return await _dbContext.TlAwardingOrganisation
                .OrderBy(x => x.DisplayName)
                .Select(x => new FilterLookupData { Id = x.Id, Name = x.DisplayName, IsSelected = false })
                .ToListAsync();
        }

        public async Task<IList<FilterLookupData>> GetAcademicYearFiltersAsync(DateTime searchDate)
        {
            return await _dbContext.AcademicYear
                .Where(x => searchDate >= x.EndDate || (searchDate >= x.StartDate && searchDate <= x.EndDate))
                .OrderByDescending(x => x.Year)
                .Take(5)
                .Select(x => new FilterLookupData { Id = x.Year, Name = $"{x.Year} to {x.Year + 1}", IsSelected = false })
                .ToListAsync();
        }

        public async Task<TqRegistrationPathway> GetLearnerRecordAsync(int registrationPathwayId)
        {
            IQueryable<TqRegistrationPathway> query = _dbContext.TqRegistrationPathway
                .Include(x => x.TqPathwayAssessments.Where(pa => pa.IsOptedin && pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? pa.EndDate != null : pa.EndDate == null))
                    .ThenInclude(x => x.AssessmentSeries)
                .Include(x => x.TqPathwayAssessments.Where(pa => pa.IsOptedin && pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? pa.EndDate != null : pa.EndDate == null))
                    .ThenInclude(x => x.TqPathwayResults.Where(pr => pr.IsOptedin && pr.TqPathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? pr.EndDate != null : pr.EndDate == null))
                    .ThenInclude(x => x.TlLookup)
                .Include(x => x.TqRegistrationProfile)
                .Include(x => x.TqProvider)
                    .ThenInclude(x => x.TqAwardingOrganisation)
                    .ThenInclude(x => x.TlPathway)
                .Include(x => x.TqProvider)
                    .ThenInclude(x => x.TlProvider)
                    .ThenInclude(x => x.TlProviderAddresses.Where(pa => pa.IsActive))
                .Include(x => x.TqProvider)
                    .ThenInclude(x => x.TqAwardingOrganisation)
                    .ThenInclude(x => x.TlAwardingOrganisaton)
                .Include(x => x.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin && rs.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? rs.EndDate != null : rs.EndDate == null))
                    .ThenInclude(x => x.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? sa.EndDate != null : sa.EndDate == null))
                    .ThenInclude(x => x.TqSpecialismResults.Where(sr => sr.IsOptedin && sr.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? sr.EndDate != null : sr.EndDate == null))
                    .ThenInclude(x => x.TlLookup)
                .Include(x => x.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin && rs.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? rs.EndDate != null : rs.EndDate == null))
                    .ThenInclude(x => x.TlSpecialism)
                    .ThenInclude(x => x.TlPathwaySpecialismCombinations)
                .Include(x => x.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin && rs.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? rs.EndDate != null : rs.EndDate == null))
                    .ThenInclude(x => x.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? sa.EndDate != null : sa.EndDate == null))
                    .ThenInclude(x => x.AssessmentSeries)
            .Include(x => x.IndustryPlacements)
            .Include(x => x.OverallResults.Where(o => o.IsOptedin && (o.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn) ? o.EndDate != null : o.EndDate == null))
            .Include(x => x.PrintCertificates.Where(pc => pc.Type == PrintCertificateType.StatementOfAchievement || pc.Type == PrintCertificateType.Certificate))
                .ThenInclude(x => x.PrintBatchItem)
                .ThenInclude(x => x.Batch)
            .OrderByDescending(o => o.CreatedOn);

            TqRegistrationPathway regPathway = await query.FirstOrDefaultAsync(p => p.Id == registrationPathwayId &&
            (
                p.Status == RegistrationPathwayStatus.Active ||
                p.Status == RegistrationPathwayStatus.Withdrawn
            ));

            if (regPathway == null)
                return null;

            // Sort core and specialism assessments.
            regPathway.TqPathwayAssessments = regPathway.TqPathwayAssessments?.OrderByDescending(x => x.AssessmentSeriesId).ThenByDescending(x => x.CreatedOn).ToList();
            regPathway.TqRegistrationSpecialisms?.ToList().ForEach(s =>
            {
                s.TqSpecialismAssessments = s.TqSpecialismAssessments?.OrderByDescending(x => x.AssessmentSeriesId).ThenByDescending(x => x.CreatedOn).ToList();
            });

            return regPathway;
        }

        public async Task<PagedResponse<AdminSearchLearnerDetail>> SearchLearnerDetailsAsync(AdminSearchLearnerRequest request)
        {
            IQueryable<TqRegistrationPathway> registrationPathwayQueryable = _dbContext.TqRegistrationPathway
                                                                                        .Include(p => p.TqProvider)
                                                                                            .ThenInclude(p => p.TlProvider)
                                                                                        .Include(p => p.TqProvider)
                                                                                            .ThenInclude(p => p.TqAwardingOrganisation)
                                                                                            .ThenInclude(p => p.TlAwardingOrganisaton)
                                                                                       .Where(p => !_dbContext.TqRegistrationPathway.Any(p2 => p2.TqRegistrationProfileId == p.TqRegistrationProfileId && p2.Id > p.Id))
                                                                                       .AsQueryable();

            int totalCount = registrationPathwayQueryable.Count();

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                string searchKey = request.SearchKey.Trim();
                bool isSearchKeyUln = searchKey.IsLong();

                Expression<Func<TqRegistrationPathway, bool>> expression = isSearchKeyUln
                    ? p => p.TqRegistrationProfile.UniqueLearnerNumber == searchKey.ToLong()
                    : p => EF.Functions.Like(p.TqRegistrationProfile.Lastname.Trim(), searchKey);

                registrationPathwayQueryable = registrationPathwayQueryable.Where(expression);
            }

            if (!request.SelectedAwardingOrganisations.IsNullOrEmpty())
            {
                registrationPathwayQueryable = registrationPathwayQueryable.Where(p => request.SelectedAwardingOrganisations.Contains(p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId));
            }

            if (!request.SelectedAcademicYears.IsNullOrEmpty())
            {
                registrationPathwayQueryable = registrationPathwayQueryable.Where(p => request.SelectedAcademicYears.Contains(p.AcademicYear));
            }

            if (request.ProviderId.HasValue)
            {
                registrationPathwayQueryable = registrationPathwayQueryable.Where(p => request.ProviderId == p.TqProvider.TlProviderId);
            }

            int filteredRecordsCount = await registrationPathwayQueryable.CountAsync();
            var pager = new Pager(filteredRecordsCount, request.PageNumber, 10);

            IQueryable<AdminSearchLearnerDetail> learnerRecordsQueryable = registrationPathwayQueryable
                .Select(x => new AdminSearchLearnerDetail
                {
                    RegistrationPathwayId = x.Id,
                    Uln = x.TqRegistrationProfile.UniqueLearnerNumber,
                    Firstname = x.TqRegistrationProfile.Firstname,
                    Lastname = x.TqRegistrationProfile.Lastname,
                    Provider = x.TqProvider.TlProvider.Name,
                    ProviderUkprn = x.TqProvider.TlProvider.UkPrn,
                    AwardingOrganisation = x.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.DisplayName,
                    AcademicYear = x.AcademicYear
                })
                .OrderBy(x => x.Lastname)
                .Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

            List<AdminSearchLearnerDetail> learnerRecords = await learnerRecordsQueryable.ToListAsync();
            return new PagedResponse<AdminSearchLearnerDetail> { Records = learnerRecords, TotalRecords = totalCount, PagerInfo = pager };
        }

        public async Task<IList<int>> GetAllowedChangeAcademicYearsAsync(Func<DateTime> getToday, int learnerAcademicYear, int pathwayStartYear)
        {
            DateTime today = getToday();

            IQueryable<int> academicYearsQueryable = _dbContext.AcademicYear
                                                                .Where(p => today >= p.StartDate
                                                                    && p.Year >= pathwayStartYear
                                                                    && p.Year >= learnerAcademicYear - 2
                                                                    && p.Year <= learnerAcademicYear + 1
                                                                    && p.Year != learnerAcademicYear)
                                                                .OrderByDescending(p => p.Year)
                                                                .Select(p => p.Year);

            return await academicYearsQueryable.ToListAsync();
        }
    }
}