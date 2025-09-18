using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class TrainingProviderRepository : ITrainingProviderRepository
    {
        protected readonly ResultsAndCertificationDbContext _dbContext;

        public TrainingProviderRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<SearchLearnerDetail>> SearchLearnerDetailsAsync(SearchLearnerRequest request)
        {
            var pathwayQueryable = _dbContext.TqRegistrationPathway
                                             .Where(p => p.TqProvider.TlProvider.UkPrn == request.Ukprn && p.Status == RegistrationPathwayStatus.Active)
                                             .AsQueryable();

            bool filteredByAcademicYear = !request.AcademicYear.IsNullOrEmpty();

            var totalCount = filteredByAcademicYear
                ? pathwayQueryable.Where(p => request.AcademicYear.Contains(p.AcademicYear)).Count()
                : pathwayQueryable.Count();

            if (filteredByAcademicYear)
            {
                pathwayQueryable = pathwayQueryable.Where(p => request.AcademicYear.Contains(p.AcademicYear));
            }

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                string searchKey = request.SearchKey.Trim();
                bool isSearchKeyUln = searchKey.IsLong();

                pathwayQueryable = isSearchKeyUln
                    ? pathwayQueryable.Where(p => p.TqRegistrationProfile.UniqueLearnerNumber == searchKey.ToLong())
                    : pathwayQueryable.Where(p => EF.Functions.Like(p.TqRegistrationProfile.Lastname.Trim(), searchKey));
            }

            if (request.Tlevels != null && request.Tlevels.Any())
                pathwayQueryable = pathwayQueryable.Where(p => request.Tlevels.Contains(p.TqProvider.TqAwardingOrganisation.TlPathway.Id));

            if (request.Statuses != null && request.Statuses.Any()
                || request.IndustryPlacementStatus != null && request.IndustryPlacementStatus.Any())
            {
                var expressions = new List<Expression<Func<TqRegistrationPathway, bool>>>();

                foreach (var statusId in request.Statuses.OrderBy(s => s))
                {
                    switch (statusId)
                    {
                        case (int)LearnerStatusFilter.EnglishIncomplete:
                            expressions.Add(p => p.TqRegistrationProfile.EnglishStatus == null);
                            break;
                        case (int)LearnerStatusFilter.MathsIncomplete:
                            expressions.Add(p => p.TqRegistrationProfile.MathsStatus == null);
                            break;
                    }
                }

                foreach (var statusId in request.IndustryPlacementStatus.OrderBy(s => s))
                {
                    switch (statusId)
                    {
                        case (int)IndustryPlacementSearchFilterStatus.IndustryPlacementCompleted:
                            expressions.Add(p => p.IndustryPlacements.Any(ip => ip.Status == IndustryPlacementStatus.Completed));
                            break;
                        case (int)IndustryPlacementSearchFilterStatus.IndustryPlacementCompletedWithConsideration:
                            expressions.Add(p => p.IndustryPlacements.Any(ip => ip.Status == IndustryPlacementStatus.CompletedWithSpecialConsideration));
                            break;
                        case (int)IndustryPlacementSearchFilterStatus.IndustryPlacementNotCompleted:
                            expressions.Add(p => p.IndustryPlacements.Any(ip => ip.Status == IndustryPlacementStatus.NotCompleted));
                            break;
                        case (int)IndustryPlacementSearchFilterStatus.IndustryPlacementWillNotComplete:
                            expressions.Add(p => p.IndustryPlacements.Any(ip => ip.Status == IndustryPlacementStatus.WillNotComplete));
                            break;
                        case (int)IndustryPlacementSearchFilterStatus.IndustryPlacementNotReported:
                            expressions.Add(p => !p.IndustryPlacements.Any());
                            break;
                    }
                }

                Expression<Func<TqRegistrationPathway, bool>> criteria = null;

                expressions.ForEach(exp =>
                {
                    criteria = LinqExpressionExtensions.OrCombine(criteria, exp);
                });

                if (criteria != null)
                    pathwayQueryable = pathwayQueryable.Where(criteria);
            }

            var filteredRecordsCount = await pathwayQueryable.CountAsync();

            var pager = new Pager(filteredRecordsCount, request.PageNumber, 10);

            var learnerRecords = await pathwayQueryable
                .Select(x => new SearchLearnerDetail
                {
                    ProfileId = x.TqRegistrationProfile.Id,
                    Uln = x.TqRegistrationProfile.UniqueLearnerNumber,
                    Firstname = x.TqRegistrationProfile.Firstname,
                    Lastname = x.TqRegistrationProfile.Lastname,
                    AcademicYear = x.AcademicYear,
                    TlevelName = x.TqProvider.TqAwardingOrganisation.TlPathway.Name,
                    EnglishStatus = x.TqRegistrationProfile.EnglishStatus,
                    MathsStatus = x.TqRegistrationProfile.MathsStatus,
                    IndustryPlacementStatus = x.IndustryPlacements.Any() ? x.IndustryPlacements.FirstOrDefault().Status : null
                })
                .OrderBy(x => x.Lastname)
                .Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize)
                .ToListAsync();

            return new PagedResponse<SearchLearnerDetail> { Records = learnerRecords, TotalRecords = totalCount, PagerInfo = pager };
        }

        public async Task<IList<FilterLookupData>> GetSearchAcademicYearFiltersAsync(DateTime searchDate)
        {
            return await _dbContext.AcademicYear
                    .Where(x => searchDate >= x.EndDate || (searchDate >= x.StartDate && searchDate <= x.EndDate))
                    .OrderBy(x => x.Year)
                    .Take(4)
                    .Select(x => new FilterLookupData { Id = x.Year, Name = $"{x.Year} to {x.Year + 1}", IsSelected = false })
                    .ToListAsync();
        }

        public async Task<IList<FilterLookupData>> GetSearchTlevelFiltersAsync()
        {
            return await _dbContext.TlPathway
                    .OrderBy(x => x.Name)
                    .Select(x => new FilterLookupData { Id = x.Id, Name = x.Name, IsSelected = false })
                    .ToListAsync();

        }

        public async Task<FindLearnerRecord> FindLearnerRecordAsync(long providerUkprn, long uln)
        {
            var learnerRecord = await (from tqPathway in _dbContext.TqRegistrationPathway
                                       join tqProfile in _dbContext.TqRegistrationProfile on tqPathway.TqRegistrationProfileId equals tqProfile.Id
                                       join tqProvider in _dbContext.TqProvider on tqPathway.TqProviderId equals tqProvider.Id
                                       join tlProvider in _dbContext.TlProvider on tqProvider.TlProviderId equals tlProvider.Id
                                       join tqAo in _dbContext.TqAwardingOrganisation on tqProvider.TqAwardingOrganisationId equals tqAo.Id
                                       join tlPathway in _dbContext.TlPathway on tqAo.TlPathwayId equals tlPathway.Id
                                       orderby tqPathway.CreatedOn descending
                                       where tqProfile.UniqueLearnerNumber == uln && tlProvider.UkPrn == providerUkprn
                                       select new FindLearnerRecord
                                       {
                                           ProfileId = tqProfile.Id,
                                           Uln = tqProfile.UniqueLearnerNumber,
                                           Name = tqProfile.Firstname + " " + tqProfile.Lastname,
                                           DateofBirth = tqProfile.DateofBirth,
                                           ProviderName = tlProvider.Name + " (" + tlProvider.UkPrn + ")",
                                           PathwayName = tlPathway.Name + " (" + tlPathway.LarId + ")",
                                           IsLearnerRegistered = tqPathway.Status == RegistrationPathwayStatus.Active || tqPathway.Status == RegistrationPathwayStatus.Withdrawn
                                       })
                                .FirstOrDefaultAsync();
            return learnerRecord;
        }

        public async Task<LearnerRecordDetails> GetLearnerRecordDetailsAsync(long providerUkprn, int profileId, int? pathwayId = null)
        {
            var learnerRecordQuerable = from tqPathway in _dbContext.TqRegistrationPathway
                                        join tqProfile in _dbContext.TqRegistrationProfile on tqPathway.TqRegistrationProfileId equals tqProfile.Id
                                        join tqProvider in _dbContext.TqProvider on tqPathway.TqProviderId equals tqProvider.Id
                                        join tlProvider in _dbContext.TlProvider on tqProvider.TlProviderId equals tlProvider.Id
                                        join tqAo in _dbContext.TqAwardingOrganisation on tqProvider.TqAwardingOrganisationId equals tqAo.Id
                                        join tlPathway in _dbContext.TlPathway on tqAo.TlPathwayId equals tlPathway.Id
                                        orderby tqPathway.CreatedOn descending
                                        let printCertificate = tqPathway.PrintCertificates.Where(p => p.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active
                                                                                                && (p.Type == PrintCertificateType.StatementOfAchievement || p.Type == PrintCertificateType.Certificate))
                                                                                          .OrderByDescending(c => c.Id).FirstOrDefault() // Fetching certificate for only active pathway
                                        let ipRecord = tqPathway.IndustryPlacements.FirstOrDefault()
                                        let overallResult = tqPathway.OverallResults.FirstOrDefault(o => o.IsOptedin && (tqPathway.Status == RegistrationPathwayStatus.Withdrawn) ? o.EndDate != null : o.EndDate == null)
                                        let specialisms = tqPathway.TqRegistrationSpecialisms.Where(t => t.EndDate == null).Select(t => t.TlSpecialism.Name).ToList()
                                        where tqProfile.Id == profileId && tlProvider.UkPrn == providerUkprn
                                        select new LearnerRecordDetails
                                        {
                                            ProfileId = tqProfile.Id,
                                            RegistrationPathwayId = tqPathway.Id,
                                            TlPathwayId = tlPathway.Id,
                                            Uln = tqProfile.UniqueLearnerNumber,
                                            Name = tqProfile.Firstname + " " + tqProfile.Lastname,
                                            DateofBirth = tqProfile.DateofBirth,
                                            ProviderName = tlProvider.Name,
                                            ProviderUkprn = tlProvider.UkPrn,
                                            TlevelTitle = tlPathway.TlevelTitle,
                                            Specialisms = specialisms,
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
                                            OverallResultDetails = overallResult != null ? overallResult.Details : null,
                                            OverallResultPublishDate = overallResult != null ? overallResult.PublishDate : null,
                                            PrintCertificateId = printCertificate != null ? printCertificate.Id : null,
                                            PrintCertificateType = printCertificate != null ? printCertificate.Type : null,
                                            LastDocumentRequestedDate = printCertificate != null ? printCertificate.LastRequestedOn : null,
                                            IsReprint = printCertificate != null ? printCertificate.IsReprint : null,
                                            ProviderAddress = tlProvider.TlProviderAddresses.Where(pa => pa.IsActive)
                                                                                            .OrderByDescending(pa => pa.CreatedOn)
                                                                                            .Select(address => new Address
                                                                                            {
                                                                                                AddressId = address.Id,
                                                                                                DepartmentName = address.DepartmentName,
                                                                                                OrganisationName = address.OrganisationName,
                                                                                                AddressLine1 = address.AddressLine1,
                                                                                                AddressLine2 = address.AddressLine2,
                                                                                                Town = address.Town,
                                                                                                Postcode = address.Postcode
                                                                                            }).FirstOrDefault()

                                        };

            var learnerRecordDetails = pathwayId.HasValue ? await learnerRecordQuerable.FirstOrDefaultAsync(p => p.RegistrationPathwayId == pathwayId) : await learnerRecordQuerable.FirstOrDefaultAsync();

            return learnerRecordDetails;
        }
    }
}
