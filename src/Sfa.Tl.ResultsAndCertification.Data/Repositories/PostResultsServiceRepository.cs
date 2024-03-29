﻿using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class PostResultsServiceRepository : IPostResultsServiceRepository
    {
        protected readonly ResultsAndCertificationDbContext _dbContext;

        public PostResultsServiceRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long? uln, int? profileId = null)
        {
            var prsQuery = from tqPathway in _dbContext.TqRegistrationPathway
                           join tqProfile in _dbContext.TqRegistrationProfile on tqPathway.TqRegistrationProfileId equals tqProfile.Id
                           join tqProvider in _dbContext.TqProvider on tqPathway.TqProviderId equals tqProvider.Id
                           join tlProvider in _dbContext.TlProvider on tqProvider.TlProviderId equals tlProvider.Id
                           join tqAo in _dbContext.TqAwardingOrganisation on tqProvider.TqAwardingOrganisationId equals tqAo.Id
                           join tlAo in _dbContext.TlAwardingOrganisation on tqAo.TlAwardingOrganisatonId equals tlAo.Id
                           join tlPathway in _dbContext.TlPathway on tqAo.TlPathwayId equals tlPathway.Id
                           orderby tqPathway.CreatedOn descending
                           where
                               tlAo.UkPrn == aoUkprn &&
                               (tqPathway.Status == RegistrationPathwayStatus.Active || tqPathway.Status == RegistrationPathwayStatus.Withdrawn)
                           select new FindPrsLearnerRecord
                           {
                               ProfileId = tqProfile.Id,
                               Uln = tqProfile.UniqueLearnerNumber,
                               Firstname = tqProfile.Firstname,
                               Lastname = tqProfile.Lastname,
                               DateofBirth = tqProfile.DateofBirth,
                               ProviderName = tlProvider.Name,
                               ProviderUkprn = tlProvider.UkPrn,
                               TlevelTitle = tlPathway.TlevelTitle,
                               Status = tqPathway.Status,
                               PathwayAssessments = tqPathway.TqPathwayAssessments.Where(a => a.TqRegistrationPathwayId == tqPathway.Id && a.IsOptedin && a.EndDate == null)
                                                    .OrderByDescending(o => o.AssessmentSeriesId)
                                                    .Select(x => new PrsAssessment
                                                    {
                                                        AssessmentId = x.Id,
                                                        SeriesName = x.AssessmentSeries.Name,
                                                        HasResult = x.TqPathwayResults.Any(r => r.IsOptedin && r.EndDate == null)
                                                    }),
                               SpecialismAssessments = tqPathway.TqRegistrationSpecialisms.SelectMany(s => s.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.EndDate == null))
                                                       .OrderByDescending(o => o.AssessmentSeriesId)
                                                       .Select(x => new PrsAssessment
                                                       {
                                                           AssessmentId = x.Id,
                                                           SeriesName = x.AssessmentSeries.Name,
                                                           HasResult = x.TqSpecialismResults.Any(r => r.IsOptedin && r.EndDate == null)
                                                       })
                           };

            bool searchByUlnPredicate() => uln != null;
            prsQuery = searchByUlnPredicate() ? prsQuery.Where(x => x.Uln == uln) : prsQuery.Where(x => x.ProfileId == profileId);

            return await prsQuery.FirstOrDefaultAsync();
        }
    }
}