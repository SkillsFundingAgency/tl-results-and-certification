using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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

        public async Task<AdminChangeLogRecord> GetChangeLogRecordAsync(int changeLogId)
        {
            return await _dbContext.ChangeLog
                            .Include(p => p.TqRegistrationPathway)
                            .ThenInclude(p => p.TqRegistrationProfile)
                            .Include(p => p.TqRegistrationPathway)
                            .Where(p => p.Id == changeLogId)
                                .Select(p => new AdminChangeLogRecord()
                                {
                                    ChangeLogId = p.Id,
                                    RegistrationPathwayId = p.TqRegistrationPathwayId,
                                    FirstName = p.TqRegistrationPathway.TqRegistrationProfile.Firstname,
                                    LastName = p.TqRegistrationPathway.TqRegistrationProfile.Lastname,
                                    Uln = p.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                                    CreatedBy = p.CreatedBy,
                                    ChangeType = (ChangeType)p.ChangeType,
                                    ChangeDetails = p.Details,
                                    ChangeRequestedBy = p.Name,
                                    ChangeDateOfRequest = p.DateOfRequest,
                                    ReasonForChange = p.ReasonForChange,
                                    ZendeskTicketID = p.ZendeskTicketID,
                                    DateAndTimeOfChange = p.CreatedOn
                                })
                                .FirstOrDefaultAsync();
        }
    }
}