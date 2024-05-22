using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class SearchRegistrationRepository : ISearchRegistrationRepository
    {
        private readonly ResultsAndCertificationDbContext _dbContext;

        public SearchRegistrationRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<FilterLookupData>> GetAcademicYearFiltersAsync(Func<DateTime> getSearchDate)
        {
            DateTime searchDate = getSearchDate();

            return await _dbContext.AcademicYear
                .Where(x => searchDate >= x.EndDate || (searchDate >= x.StartDate && searchDate <= x.EndDate))
                .OrderByDescending(x => x.Year)
                .Take(5)
                .Select(x => new FilterLookupData { Id = x.Year, Name = $"{x.Year} to {x.Year + 1}", IsSelected = false })
                .ToListAsync();
        }

        public async Task<PagedResponse<SearchRegistrationDetail>> SearchRegistrationDetailsAsync(SearchRegistrationRequest request)
        {
            IQueryable<TqRegistrationPathway> registrationPathwayQueryable = _dbContext.TqRegistrationPathway
                                                                                        .Include(p => p.TqProvider)
                                                                                            .ThenInclude(p => p.TlProvider)
                                                                                        .Include(p => p.TqProvider)
                                                                                            .ThenInclude(p => p.TqAwardingOrganisation)
                                                                                            .ThenInclude(p => p.TlAwardingOrganisaton)
                                                                                        .Include(p => p.TqProvider)
                                                                                            .ThenInclude(p => p.TqAwardingOrganisation)
                                                                                            .ThenInclude(p => p.TlPathway)
                                                                                        .Include(p => p.TqPathwayAssessments.Where(pa => pa.IsOptedin))
                                                                                            .ThenInclude(p => p.TqPathwayResults.Where(pr => pr.IsOptedin))
                                                                                        .Include(p => p.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin))
                                                                                            .ThenInclude(p => p.TqSpecialismAssessments.Where(sa => sa.IsOptedin))
                                                                                            .ThenInclude(p => p.TqSpecialismResults.Where(sr => sr.IsOptedin))
                                                                                        .Where(p => p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == request.AoUkprn
                                                                                                   && !_dbContext.TqRegistrationPathway.Any(p2 => p2.TqRegistrationProfileId == p.TqRegistrationProfileId && p2.Id > p.Id))
                                                                                       .AsQueryable();

            int totalCount = registrationPathwayQueryable.Count();

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                bool isSearchKeyUln = request.SearchKey.IsLong();

                Expression<Func<TqRegistrationPathway, bool>> expression = isSearchKeyUln
                    ? p => p.TqRegistrationProfile.UniqueLearnerNumber == request.SearchKey.ToLong()
                    : p => EF.Functions.Like(p.TqRegistrationProfile.Lastname.Trim(), request.SearchKey);

                registrationPathwayQueryable = registrationPathwayQueryable.Where(expression);
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

            IQueryable<SearchRegistrationDetail> searchRegistrationDetailQueryable = registrationPathwayQueryable
                .Select(x => new SearchRegistrationDetail
                {
                    RegistrationProfileId = x.TqRegistrationProfileId,
                    Uln = x.TqRegistrationProfile.UniqueLearnerNumber,
                    Firstname = x.TqRegistrationProfile.Firstname,
                    Lastname = x.TqRegistrationProfile.Lastname,
                    ProviderName = x.TqProvider.TlProvider.Name,
                    ProviderUkprn = x.TqProvider.TlProvider.UkPrn,
                    PathwayName = x.TqProvider.TqAwardingOrganisation.TlPathway.Name,
                    PathwayLarId = x.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                    AcademicYear = x.AcademicYear,
                    IsWithdrawn = x.Status == RegistrationPathwayStatus.Withdrawn,
                    HasResults = x.TqPathwayAssessments.Any(pa => pa.TqPathwayResults.Any())
                        || x.TqRegistrationSpecialisms.Any(rs => rs.TqSpecialismAssessments.Any(sa => sa.TqSpecialismResults.Any()))
                })
                .OrderBy(x => x.Lastname)
                .Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

            List<SearchRegistrationDetail> registrationDetails = await searchRegistrationDetailQueryable.ToListAsync();
            return new PagedResponse<SearchRegistrationDetail> { Records = registrationDetails, TotalRecords = totalCount, PagerInfo = pager };
        }
    }
}
