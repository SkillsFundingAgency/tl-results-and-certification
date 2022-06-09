using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
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
        private readonly ILogger<TrainingProviderRepository> _logger;

        public TrainingProviderRepository(ResultsAndCertificationDbContext dbContext, ILogger<TrainingProviderRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<PagedResponse<SearchLearnerDetail>> SearchLearnerDetailsAsync(SearchLearnerRequest request)
        {
            var pathwayQueryable = _dbContext.TqRegistrationPathway
                                             .Where(p => p.TqProvider.TlProvider.UkPrn == request.Ukprn && p.Status == RegistrationPathwayStatus.Active)
                                             .AsQueryable();

            if (request.AcademicYear != null && request.AcademicYear.Any())
                pathwayQueryable = pathwayQueryable.Where(p => request.AcademicYear.Contains(p.AcademicYear));

            if (request.Tlevels != null && request.Tlevels.Any())
                pathwayQueryable = pathwayQueryable.Where(p => request.Tlevels.Contains(p.TqProvider.TqAwardingOrganisation.TlPathway.Id));

            if (request.Statuses != null && request.Statuses.Any())
            {
                var expressions = new List<Expression<Func<TqRegistrationPathway, bool>>>();

                foreach (var statusId in request.Statuses)
                {
                    switch (statusId)
                    {
                        case (int)LearnerStatusFilter.EnglishIncompleted:
                            expressions.Add(p => p.TqRegistrationProfile.EnglishStatus == null);
                            break;
                        case (int)LearnerStatusFilter.MathsIncompleted:
                            expressions.Add(p => p.TqRegistrationProfile.MathsStatus == null);
                            break;
                        case (int)LearnerStatusFilter.IndustryPlacementIncompleted:
                            expressions.Add(p => !p.IndustryPlacements.Any());
                            break;
                        case (int)LearnerStatusFilter.AllIncomplemented:
                            expressions.Clear();
                            expressions.Add(p => p.TqRegistrationProfile.EnglishStatus == null || p.TqRegistrationProfile.MathsStatus == null || !p.IndustryPlacements.Any());
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
                .ToListAsync();

            return new PagedResponse<SearchLearnerDetail> { Records = learnerRecords, TotalRecords = learnerRecords.Count };
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
                                        let ipRecord = tqPathway.IndustryPlacements.FirstOrDefault()
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
                                            AcademicYear = tqPathway.AcademicYear,
                                            AwardingOrganisationName = tqAo.TlAwardingOrganisaton.DisplayName,
                                            MathsStatus = tqProfile.MathsStatus,
                                            EnglishStatus = tqProfile.EnglishStatus,
                                            IsLearnerRegistered = tqPathway.Status == RegistrationPathwayStatus.Active || tqPathway.Status == RegistrationPathwayStatus.Withdrawn,
                                            IndustryPlacementId = ipRecord != null ? ipRecord.Id : 0,
                                            IndustryPlacementStatus = ipRecord != null ? ipRecord.Status : null
                                        };

            var learnerRecordDetails = pathwayId.HasValue ? await learnerRecordQuerable.FirstOrDefaultAsync(p => p.RegistrationPathwayId == pathwayId) : await learnerRecordQuerable.FirstOrDefaultAsync();

            return learnerRecordDetails;
        }
    }
}
