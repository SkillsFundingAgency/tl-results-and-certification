using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
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

        public async Task<AdminLearnerRecord> GetAdminLearnerRecordAsync(int pathwayId)
        {
            var learnerRecordQuerable = from tqPathway in _dbContext.TqRegistrationPathway
                                        join tqProfile in _dbContext.TqRegistrationProfile on tqPathway.TqRegistrationProfileId equals tqProfile.Id
                                        join tqProvider in _dbContext.TqProvider on tqPathway.TqProviderId equals tqProvider.Id
                                        join tlProvider in _dbContext.TlProvider on tqProvider.TlProviderId equals tlProvider.Id
                                        join tqAo in _dbContext.TqAwardingOrganisation on tqProvider.TqAwardingOrganisationId equals tqAo.Id
                                        join tlPathway in _dbContext.TlPathway on tqAo.TlPathwayId equals tlPathway.Id
                                        orderby tqPathway.CreatedOn descending
                                        let ipRecord = tqPathway.IndustryPlacements.FirstOrDefault()                                       
                                        where tqPathway.Id == pathwayId
                                        select new AdminLearnerRecord
                                        {
                                            PathwayId = pathwayId,
                                            FirstName = tqProfile.Firstname,
                                            LastName = tqProfile.Lastname,
                                            RegistrationPathwayId = tqPathway.Id,
                                            TlPathwayId = tlPathway.Id,
                                            Uln = tqProfile.UniqueLearnerNumber,
                                            Name = tqProfile.Firstname + " " + tqProfile.Lastname,
                                            DateofBirth = tqProfile.DateofBirth,
                                            ProviderName = tlProvider.Name,
                                            ProviderUkprn = tlProvider.UkPrn,
                                            TlevelName = tlPathway.Name,
                                            TlevelStartYear = tlPathway.StartYear,
                                            AcademicYear = tqPathway.AcademicYear,
                                            AwardingOrganisationName = tqAo.TlAwardingOrganisaton.DisplayName,
                                            MathsStatus = tqProfile.MathsStatus,
                                            EnglishStatus = tqProfile.EnglishStatus,
                                            IsLearnerRegistered = tqPathway.Status == RegistrationPathwayStatus.Active || tqPathway.Status == RegistrationPathwayStatus.Withdrawn,
                                            RegistrationPathwayStatus = tqPathway.Status,
                                            IsPendingWithdrawal = tqPathway.IsPendingWithdrawal,
                                            IndustryPlacementId = ipRecord != null ? ipRecord.Id : 0,
                                            IndustryPlacementStatus = ipRecord != null ? ipRecord.Status : null,
                                            IndustryPlacementDetails = ipRecord != null ? ipRecord.Details : null,
                                           
                                        };
            var learnerRecordDetails =  await learnerRecordQuerable.FirstOrDefaultAsync();
            return learnerRecordDetails;
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
                bool isSearchKeyUln = request.SearchKey.IsLong();

                Expression<Func<TqRegistrationPathway, bool>> expression = isSearchKeyUln
                    ? p => p.TqRegistrationProfile.UniqueLearnerNumber == request.SearchKey.ToLong()
                    : p => EF.Functions.Like(p.TqRegistrationProfile.Lastname.Trim(), request.SearchKey.ToLower());

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

            if (request.ProviderUkprn.HasValue)
            {
                registrationPathwayQueryable = registrationPathwayQueryable.Where(p => request.ProviderUkprn == p.TqProvider.TlProvider.UkPrn);
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

    }
}