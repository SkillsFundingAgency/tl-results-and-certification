using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminChangeLog;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Repositories
{
    public class AdminChangeLogRepository : IAdminChangeLogRepository
    {
        private readonly ResultsAndCertificationDbContext _dbContext;

        public AdminChangeLogRepository(ResultsAndCertificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<AdminSearchChangeLog>> SearchChangeLogsAsync(AdminSearchChangeLogRequest request)
        {
            IQueryable<ChangeLog> changeLogQueryable = _dbContext.ChangeLog
                                                            .Include(p => p.TqRegistrationPathway)
                                                                .ThenInclude(p => p.TqRegistrationProfile)
                                                            .Include(p => p.TqRegistrationPathway)
                                                                .ThenInclude(p => p.TqProvider)
                                                                .ThenInclude(p => p.TlProvider);

            int totalCount = changeLogQueryable.Count();

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                Expression<Func<ChangeLog, bool>> queryExpression =
                        p => (request.SearchKey.IsLong() && p.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == request.SearchKey.ToLong())
                        || EF.Functions.Like(p.TqRegistrationPathway.TqRegistrationProfile.Lastname.Trim(), request.SearchKey)
                        || EF.Functions.Like(p.ZendeskTicketID.Trim(), request.SearchKey);

                changeLogQueryable = changeLogQueryable.Where(queryExpression);
            }

            int filteredRecordsCount = await changeLogQueryable.CountAsync();
            var pager = new Pager(filteredRecordsCount, request.PageNumber, 10);

            IQueryable<AdminSearchChangeLog> searchChangeLogQueryable = changeLogQueryable
                .Select(x => new AdminSearchChangeLog
                {
                    ChangeLogId = x.Id,
                    ChangeType = x.ChangeType,
                    DateAndTimeOfChange = x.CreatedOn,
                    ZendeskTicketID = x.ZendeskTicketID,
                    LearnerFirstname = x.TqRegistrationPathway.TqRegistrationProfile.Firstname,
                    LearnerLastname = x.TqRegistrationPathway.TqRegistrationProfile.Lastname,
                    Uln = x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                    ProviderName = x.TqRegistrationPathway.TqProvider.TlProvider.Name,
                    ProviderUkprn = x.TqRegistrationPathway.TqProvider.TlProvider.UkPrn,
                    LastUpdatedBy = x.Name
                })
            .OrderByDescending(x => x.DateAndTimeOfChange)
            .Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

            List<AdminSearchChangeLog> changeLogs = await searchChangeLogQueryable.ToListAsync();
            return new PagedResponse<AdminSearchChangeLog> { Records = changeLogs, TotalRecords = totalCount, PagerInfo = pager };
        }

        public async Task<ChangeLog> GetChangeLogRecordAsync(int changeLogId)
        {
            return await _dbContext.ChangeLog
                .Include(p => p.TqRegistrationPathway)
                    .ThenInclude(p => p.TqPathwayAssessments.Where(pa => pa.IsOptedin))
                    .ThenInclude(p => p.TqPathwayResults.Where(pr => pr.IsOptedin))
                    .ThenInclude(p => p.TlLookup)
                .Include(p => p.TqRegistrationPathway.TqPathwayAssessments.Where(pa => pa.IsOptedin))
                .ThenInclude(p => p.AssessmentSeries)
                .Include(p => p.TqRegistrationPathway.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin))
                    .ThenInclude(p => p.TqSpecialismAssessments.Where(sa => sa.IsOptedin))
                    .ThenInclude(p => p.TqSpecialismResults.Where(sr => sr.IsOptedin))
                    .ThenInclude(p => p.TlLookup)
                .Include(p => p.TqRegistrationPathway.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin))
                    .ThenInclude(p => p.TqSpecialismAssessments.Where(sa => sa.IsOptedin))
                    .ThenInclude(p => p.AssessmentSeries)
                .Include(p => p.TqRegistrationPathway.TqRegistrationSpecialisms.Where(sa => sa.IsOptedin))
                    .ThenInclude(p => p.TlSpecialism)
                .Include(p => p.TqRegistrationPathway.TqRegistrationProfile)
                .Include(p => p.TqRegistrationPathway.TqProvider)
                    .ThenInclude(p => p.TqAwardingOrganisation)
                    .ThenInclude(p => p.TlPathway)
                .FirstOrDefaultAsync(p => p.Id == changeLogId);
        }
    }
}