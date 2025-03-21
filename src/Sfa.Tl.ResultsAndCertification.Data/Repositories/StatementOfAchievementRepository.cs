﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class StatementOfAchievementRepository : IStatementOfAchievementRepository
    {
        protected readonly ResultsAndCertificationDbContext _dbContext;
        private readonly ILogger<StatementOfAchievementRepository> _logger;

        public StatementOfAchievementRepository(ResultsAndCertificationDbContext dbContext, ILogger<StatementOfAchievementRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<FindSoaLearnerRecord> FindSoaLearnerRecordAsync(long providerUkprn, long uln)
        {
            var soaLearnerRecord = await (from tqPathway in _dbContext.TqRegistrationPathway
                                          join tqProfile in _dbContext.TqRegistrationProfile on tqPathway.TqRegistrationProfileId equals tqProfile.Id
                                          join tqProvider in _dbContext.TqProvider on tqPathway.TqProviderId equals tqProvider.Id
                                          join tlProvider in _dbContext.TlProvider on tqProvider.TlProviderId equals tlProvider.Id
                                          join tqAo in _dbContext.TqAwardingOrganisation on tqProvider.TqAwardingOrganisationId equals tqAo.Id
                                          join tlPathway in _dbContext.TlPathway on tqAo.TlPathwayId equals tlPathway.Id
                                          orderby tqPathway.CreatedOn descending
                                          where tqProfile.UniqueLearnerNumber == uln && tlProvider.UkPrn == providerUkprn
                                          select new FindSoaLearnerRecord
                                          {
                                              ProfileId = tqProfile.Id,
                                              Uln = tqProfile.UniqueLearnerNumber,
                                              LearnerName = tqProfile.Firstname + " " + tqProfile.Lastname,
                                              DateofBirth = tqProfile.DateofBirth,
                                              ProviderName = tlProvider.Name + " (" + tlProvider.UkPrn + ")",
                                              TlevelTitle = tlPathway.TlevelTitle,
                                              Status = tqPathway.Status,
                                              IsIndustryPlacementAdded = tqPathway.IndustryPlacements.Any(),
                                              IndustryPlacementStatus = tqPathway.IndustryPlacements.Any() ? tqPathway.IndustryPlacements.FirstOrDefault().Status : IndustryPlacementStatus.NotSpecified,
                                              HasPathwayResult = tqPathway.TqPathwayAssessments.Any(pa => pa.IsOptedin && pa.TqPathwayResults.Any(pr => pr.IsOptedin))
                                          })
                                .FirstOrDefaultAsync();
            return soaLearnerRecord;
        }

        public async Task<SoaLearnerRecordDetails> GetSoaLearnerRecordDetailsAsync(long providerUkprn, int profileId)
        {
            var soaLearnerRecord = await (from tqPathway in _dbContext.TqRegistrationPathway
                                          join tqProfile in _dbContext.TqRegistrationProfile on tqPathway.TqRegistrationProfileId equals tqProfile.Id
                                          join tqProvider in _dbContext.TqProvider on tqPathway.TqProviderId equals tqProvider.Id
                                          join tlProvider in _dbContext.TlProvider on tqProvider.TlProviderId equals tlProvider.Id
                                          join tqAo in _dbContext.TqAwardingOrganisation on tqProvider.TqAwardingOrganisationId equals tqAo.Id
                                          join tlPathway in _dbContext.TlPathway on tqAo.TlPathwayId equals tlPathway.Id
                                          orderby tqPathway.CreatedOn descending
                                          let ipRecord = tqPathway.IndustryPlacements.FirstOrDefault()
                                          let pathwayResults = tqPathway.TqPathwayAssessments.Where(pa => pa.IsOptedin).SelectMany(pa => pa.TqPathwayResults.Where(pr => pr.IsOptedin))
                                          let specialism = tqPathway.TqRegistrationSpecialisms.FirstOrDefault(s => s.IsOptedin)
                                          let printRequest = _dbContext.PrintCertificate.OrderByDescending(o => o.Id).FirstOrDefault(c => c.Uln == tqProfile.UniqueLearnerNumber && c.TqRegistrationPathwayId == tqPathway.Id)
                                          where tqProfile.Id == profileId && tlProvider.UkPrn == providerUkprn
                                          select new SoaLearnerRecordDetails
                                          {
                                              ProfileId = tqProfile.Id,
                                              Uln = tqProfile.UniqueLearnerNumber,
                                              Firstname = tqProfile.Firstname,
                                              Lastname = tqProfile.Lastname,
                                              DateofBirth = tqProfile.DateofBirth,
                                              ProviderName = tlProvider.Name,
                                              ProviderUkprn = tlProvider.UkPrn,
                                              TlevelTitle = tlPathway.TlevelTitle,
                                              RegistrationPathwayId = tqPathway.Id,
                                              PathwayName = tlPathway.Name,
                                              PathwayCode = tlPathway.LarId,
                                              PathwayGrade = pathwayResults.OrderByDescending(r => r.CreatedOn).FirstOrDefault().TlLookup.Value,
                                              SpecialismName = specialism != null ? specialism.TlSpecialism.Name : null,
                                              SpecialismCode = specialism != null ? specialism.TlSpecialism.LarId : null,

                                              MathsStatus = tqProfile.MathsStatus,
                                              EnglishStatus = tqProfile.EnglishStatus,
                                              IndustryPlacementStatus = ipRecord != null ? ipRecord.Status : IndustryPlacementStatus.NotSpecified,

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
                                                                                            }).FirstOrDefault(),
                                              Status = tqPathway.Status,
                                              LastRequestedOn = printRequest.LastRequestedOn
                                          })
                                .FirstOrDefaultAsync();
            return soaLearnerRecord;
        }

        public async Task<PrintRequestSnapshot> GetPrintRequestSnapshotAsync(long providerUkprn, int profileId, int pathwayId)
        {
            var printRequest = await (from printCert in _dbContext.PrintCertificate
                                      join tqPathway in _dbContext.TqRegistrationPathway on printCert.TqRegistrationPathwayId equals tqPathway.Id
                                      join tqProfile in _dbContext.TqRegistrationProfile on tqPathway.TqRegistrationProfileId equals tqProfile.Id
                                      join tqProvider in _dbContext.TqProvider on tqPathway.TqProviderId equals tqProvider.Id
                                      join tlProvider in _dbContext.TlProvider on tqProvider.TlProviderId equals tlProvider.Id
                                      where
                                          tlProvider.UkPrn == providerUkprn &&
                                          tqProfile.Id == profileId &&
                                          printCert.Uln == tqProfile.UniqueLearnerNumber &&
                                          printCert.TqRegistrationPathwayId == pathwayId
                                      orderby printCert.CreatedOn descending
                                      select new PrintRequestSnapshot
                                      {
                                          RegistrationPathwayStatus = tqPathway.Status,
                                          RequestDetails = printCert.DisplaySnapshot,
                                          RequestedOn = printCert.CreatedOn,
                                          RequestedBy = printCert.CreatedBy
                                      })
                                .FirstOrDefaultAsync();
            return printRequest;
        }
    }
}