using Microsoft.EntityFrameworkCore;
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

        public async Task<FindPrsLearnerRecord> FindPrsLearnerRecordAsync(long aoUkprn, long uln)
        {
            var prsLearnerRecord = await (from tqPathway in _dbContext.TqRegistrationPathway
                                          join tqProfile in _dbContext.TqRegistrationProfile on tqPathway.TqRegistrationProfileId equals tqProfile.Id
                                          join tqProvider in _dbContext.TqProvider on tqPathway.TqProviderId equals tqProvider.Id
                                          join tlProvider in _dbContext.TlProvider on tqProvider.TlProviderId equals tlProvider.Id
                                          join tqAo in _dbContext.TqAwardingOrganisation on tqProvider.TqAwardingOrganisationId equals tqAo.Id
                                          join tlAo in _dbContext.TlAwardingOrganisation on tqAo.TlAwardingOrganisatonId equals tlAo.Id
                                          join tlPathway in _dbContext.TlPathway on tqAo.TlPathwayId equals tlPathway.Id
                                          orderby tqPathway.CreatedOn descending
                                          where
                                            tlAo.UkPrn == aoUkprn && tqProfile.UniqueLearnerNumber == uln &&
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
                                              Status = tqPathway.Status
                                          })
                                          .FirstOrDefaultAsync();
            return prsLearnerRecord;
        }

        public async Task<PrsLearnerDetails> GetPrsLearnerDetailsAsync(long aoUkprn, int profileId)
        {

            var prsLearnerdetails = await (from tqPathway in _dbContext.TqRegistrationPathway
                                           join tqProfile in _dbContext.TqRegistrationProfile on tqPathway.TqRegistrationProfileId equals tqProfile.Id
                                           join tqProvider in _dbContext.TqProvider on tqPathway.TqProviderId equals tqProvider.Id
                                           join tlProvider in _dbContext.TlProvider on tqProvider.TlProviderId equals tlProvider.Id
                                           join tqAo in _dbContext.TqAwardingOrganisation on tqProvider.TqAwardingOrganisationId equals tqAo.Id
                                           join tlAo in _dbContext.TlAwardingOrganisation on tqAo.TlAwardingOrganisatonId equals tlAo.Id
                                           join tlPathway in _dbContext.TlPathway on tqAo.TlPathwayId equals tlPathway.Id
                                           orderby tqPathway.CreatedOn descending
                                           where
                                            tlAo.UkPrn == aoUkprn && tqProfile.Id == profileId && tqPathway.Status == RegistrationPathwayStatus.Active
                                           select new PrsLearnerDetails
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
                                               PathwayName = tlPathway.Name,
                                               PathwayCode = tlPathway.LarId,
                                               AssessmentResults = from pAssessment in _dbContext.TqPathwayAssessment.Where(x => x.IsOptedin && x.EndDate == null && x.TqRegistrationPathwayId == tqPathway.Id)
                                                                   join pResult in _dbContext.TqPathwayResult.Where(x => x.IsOptedin && x.EndDate == null) on pAssessment.Id equals pResult.TqPathwayAssessmentId into resultGroup
                                                                   from result in resultGroup.DefaultIfEmpty()
                                                                   select new AssessmentResult
                                                                   {
                                                                       PathwayAssessmentId = pAssessment.Id,
                                                                       PathwayAssessmentSeries = pAssessment.AssessmentSeries.Name,
                                                                       PathwayResultId = result.Id,
                                                                       PathwayGrade = result.TlLookup.Value,
                                                                       PathwayGradeLastUpdatedBy = result.CreatedBy,
                                                                       PathwayGradeLastUpdatedOn = result.CreatedOn
                                                                   }
                                           })
                                          .FirstOrDefaultAsync();
            return prsLearnerdetails;
        }
    }
}