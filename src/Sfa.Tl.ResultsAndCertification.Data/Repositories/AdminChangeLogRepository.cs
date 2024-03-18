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
            IQueryable<ChangeLog> query =
                _dbContext.ChangeLog
                        .Include(p => p.TqRegistrationPathway)
                            .ThenInclude(p => p.TqPathwayAssessments.Where(pa => pa.IsOptedin))
                            .ThenInclude(p => p.TqPathwayResults.Where(pr => pr.IsOptedin))
                            .ThenInclude(p => p.TlLookup)
                        .Include(p => p.TqRegistrationPathway.TqPathwayAssessments.Where(pa => pa.IsOptedin))
                        .ThenInclude(p => p.AssessmentSeries)
                        .Include(p => p.TqRegistrationPathway.TqRegistrationSpecialisms.Where(rs => rs.IsOptedin))
                            .ThenInclude(p => p.TqSpecialismAssessments.Where(sa => sa.IsOptedin))
                            .ThenInclude(p => p.TqSpecialismResults)
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
                        .Where(p => p.Id == changeLogId);


            ChangeLog changeLog = await query.FirstOrDefaultAsync();

            TqRegistrationProfile registrationProfile = changeLog.TqRegistrationPathway.TqRegistrationProfile;
            TlPathway pathway = changeLog.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway;
            AssessmentSeries assessmentSeries = changeLog.TqRegistrationPathway.TqPathwayAssessments.FirstOrDefault()?.AssessmentSeries;
            TlSpecialism specialism = changeLog.TqRegistrationPathway.TqRegistrationSpecialisms.FirstOrDefault()?.TlSpecialism;
            AssessmentSeries specialismSeries = changeLog.TqRegistrationPathway.TqRegistrationSpecialisms.FirstOrDefault()?.TqSpecialismAssessments?.FirstOrDefault()?.AssessmentSeries;


            var changlogRecord = new AdminChangeLogRecord()
            {
                ChangeLogId = changeLog.Id,
                RegistrationPathwayId = changeLog.TqRegistrationPathwayId,
                FirstName = registrationProfile.Firstname,
                LastName = registrationProfile.Lastname,
                Uln = registrationProfile.UniqueLearnerNumber,
                PathwayName = pathway.Name,
                CoreCode = pathway.LarId,
                CoreExamPeriod = assessmentSeries == null ? string.Empty : assessmentSeries.Name,
                SpecialismName = specialism.Name,
                SpecialismCode = specialism.LarId,
                SpecialismExamPeriod = specialismSeries.Name,
                CreatedBy = changeLog.CreatedBy,
                ChangeType = (ChangeType)changeLog.ChangeType,
                ChangeDetails = changeLog.Details,
                ChangeRequestedBy = changeLog.Name,
                ChangeDateOfRequest = changeLog.DateOfRequest,
                ReasonForChange = changeLog.ReasonForChange,
                ZendeskTicketID = changeLog.ZendeskTicketID,
                DateAndTimeOfChange = changeLog.CreatedOn
            };

            return changlogRecord;
        }
    }
}