using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Factory;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AdminPostResultsService : IAdminPostResultsService
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ISystemProvider _systemProvider;

        public AdminPostResultsService(IRepositoryFactory repositoryFactory, ISystemProvider systemProvider)
        {
            _repositoryFactory = repositoryFactory;
            _systemProvider = systemProvider;
        }

        public async Task<bool> ProcessAdminOpenPathwayRommAsync(OpenPathwayRommRequest request)
        {
            var pathwayResultRepo = _repositoryFactory.GetRepository<TqPathwayResult>();

            TqPathwayResult existingPathwayResult = await pathwayResultRepo.GetFirstOrDefaultAsync(p => p.Id == request.PathwayResultId);
            if (existingPathwayResult == null)
            {
                return false;
            }

            DateTime utcNow = _systemProvider.UtcNow;

            existingPathwayResult.IsOptedin = false;
            existingPathwayResult.EndDate = utcNow;
            existingPathwayResult.ModifiedBy = request.CreatedBy;
            existingPathwayResult.ModifiedOn = utcNow;

            bool updated = await pathwayResultRepo.UpdateWithSpecifedColumnsOnlyAsync(existingPathwayResult,
                p => p.IsOptedin,
                p => p.EndDate,
                p => p.ModifiedBy,
                p => p.ModifiedOn) > 0;

            if (!updated)
            {
                return false;
            }

            var newPathwayResult = new TqPathwayResult
            {
                TqPathwayAssessmentId = existingPathwayResult.TqPathwayAssessmentId,
                TlLookupId = existingPathwayResult.TlLookupId,
                PrsStatus = PrsStatus.UnderReview,
                IsOptedin = true,
                StartDate = utcNow,
                IsBulkUpload = false,
                CreatedBy = request.CreatedBy,
                CreatedOn = utcNow
            };

            bool created = await pathwayResultRepo.CreateAsync(newPathwayResult) > 0;
            if (!created)
            {
                return false;
            }

            var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();

            ChangeLog changeLog = CreateChangeLog(ChangeType.OpenPathwayRomm, request, new { PathwayResultId = newPathwayResult.Id });
            return await changeLongRepository.CreateAsync(changeLog) > 0;
        }

        public async Task<bool> ProcessAdminOpenSpecialismRommAsync(OpenSpecialismRommRequest request)
        {
            var specialismResultRepo = _repositoryFactory.GetRepository<TqSpecialismResult>();

            TqSpecialismResult existingSpecialismResult = await specialismResultRepo.GetFirstOrDefaultAsync(p => p.Id == request.SpecialismResultId);
            if (existingSpecialismResult == null)
            {
                return false;
            }

            DateTime utcNow = _systemProvider.UtcNow;

            existingSpecialismResult.IsOptedin = false;
            existingSpecialismResult.EndDate = utcNow;
            existingSpecialismResult.ModifiedBy = request.CreatedBy;
            existingSpecialismResult.ModifiedOn = utcNow;

            bool updated = await specialismResultRepo.UpdateWithSpecifedColumnsOnlyAsync(existingSpecialismResult,
                p => p.IsOptedin,
                p => p.EndDate,
                p => p.ModifiedBy,
                p => p.ModifiedOn) > 0;

            if (!updated)
            {
                return false;
            }

            var newSpecialismResult = new TqSpecialismResult
            {
                TqSpecialismAssessmentId = existingSpecialismResult.TqSpecialismAssessmentId,
                TlLookupId = existingSpecialismResult.TlLookupId,
                PrsStatus = PrsStatus.UnderReview,
                IsOptedin = true,
                StartDate = utcNow,
                IsBulkUpload = false,
                CreatedBy = request.CreatedBy,
                CreatedOn = utcNow
            };

            bool created = await specialismResultRepo.CreateAsync(newSpecialismResult) > 0;

            if (!created)
            {
                return false;
            }

            var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();

            ChangeLog changeLog = CreateChangeLog(ChangeType.OpenSpecialismRomm, request, new { SpecialismResultId = newSpecialismResult.Id });
            return await changeLongRepository.CreateAsync(changeLog) > 0;
        }

        public async Task<bool> ProcessAdminOpenCoreAppealAsync(OpenCoreAppealRequest request)
        {
            var pathwayResultRepo = _repositoryFactory.GetRepository<TqPathwayResult>();

            TqPathwayResult existingPathwayResult = await pathwayResultRepo.GetFirstOrDefaultAsync(p => p.Id == request.PathwayResultId);
            if (existingPathwayResult == null)
            {
                return false;
            }

            DateTime utcNow = _systemProvider.UtcNow;

            existingPathwayResult.IsOptedin = false;
            existingPathwayResult.EndDate = utcNow;
            existingPathwayResult.ModifiedBy = request.CreatedBy;
            existingPathwayResult.ModifiedOn = utcNow;

            bool updated = await pathwayResultRepo.UpdateWithSpecifedColumnsOnlyAsync(existingPathwayResult,
                p => p.IsOptedin,
                p => p.EndDate,
                p => p.ModifiedBy,
                p => p.ModifiedOn) > 0;

            if (!updated)
            {
                return false;
            }

            var newPathwayResult = new TqPathwayResult
            {
                TqPathwayAssessmentId = existingPathwayResult.TqPathwayAssessmentId,
                TlLookupId = existingPathwayResult.TlLookupId,
                PrsStatus = PrsStatus.BeingAppealed,
                IsOptedin = true,
                StartDate = utcNow,
                IsBulkUpload = false,
                CreatedBy = request.CreatedBy,
                CreatedOn = utcNow
            };

            bool created = await pathwayResultRepo.CreateAsync(newPathwayResult) > 0;
            if (!created)
            {
                return false;
            }

            var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();

            ChangeLog changeLog = CreateChangeLog(ChangeType.OpenPathwayAppeal, request, new { PathwayResultId = newPathwayResult.Id });
            return await changeLongRepository.CreateAsync(changeLog) > 0;
        }

        public async Task<bool> ProcessAdminOpenSpecialismAppealAsync(OpenSpecialismAppealRequest request)
        {
            var specialismResultRepo = _repositoryFactory.GetRepository<TqSpecialismResult>();

            TqSpecialismResult existingSpecialismResult = await specialismResultRepo.GetFirstOrDefaultAsync(p => p.Id == request.SpecialismResultId);
            if (existingSpecialismResult == null)
            {
                return false;
            }

            DateTime utcNow = _systemProvider.UtcNow;

            existingSpecialismResult.IsOptedin = false;
            existingSpecialismResult.EndDate = utcNow;
            existingSpecialismResult.ModifiedBy = request.CreatedBy;
            existingSpecialismResult.ModifiedOn = utcNow;

            bool updated = await specialismResultRepo.UpdateWithSpecifedColumnsOnlyAsync(existingSpecialismResult,
                p => p.IsOptedin,
                p => p.EndDate,
                p => p.ModifiedBy,
                p => p.ModifiedOn) > 0;

            if (!updated)
            {
                return false;
            }

            var newSpecialismResult = new TqSpecialismResult
            {
                TqSpecialismAssessmentId = existingSpecialismResult.TqSpecialismAssessmentId,
                TlLookupId = existingSpecialismResult.TlLookupId,
                PrsStatus = PrsStatus.BeingAppealed,
                IsOptedin = true,
                StartDate = utcNow,
                IsBulkUpload = false,
                CreatedBy = request.CreatedBy,
                CreatedOn = utcNow
            };

            bool created = await specialismResultRepo.CreateAsync(newSpecialismResult) > 0;

            if (!created)
            {
                return false;
            }

            var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();

            ChangeLog changeLog = CreateChangeLog(ChangeType.OpenSpecialismAppeal, request, new { SpecialismResultId = newSpecialismResult.Id });
            return await changeLongRepository.CreateAsync(changeLog) > 0;
        }

        private static ChangeLog CreateChangeLog(ChangeType changeType, AdminPostResultsRequest request, object details)
            => new()
            {
                TqRegistrationPathwayId = request.RegistrationPathwayId,
                ChangeType = changeType,
                Details = JsonConvert.SerializeObject(details),
                Name = request.ContactName,
                DateOfRequest = request.DateOfRequest,
                ReasonForChange = request.ChangeReason,
                ZendeskTicketID = request.ZendeskTicketId,
                CreatedBy = string.IsNullOrEmpty(request.CreatedBy) ? "System" : request.CreatedBy
            };

    }
}