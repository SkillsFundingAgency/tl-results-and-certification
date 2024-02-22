using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Factory;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _adminDashboardRepository;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly ISystemProvider _systemProvider;
        private readonly IMapper _mapper;

        public AdminDashboardService(
            IAdminDashboardRepository adminDashboardRepository,
            IRepositoryFactory repositoryFactory,
            ISystemProvider systemProvider,
            IMapper mapper)
        {
            _adminDashboardRepository = adminDashboardRepository;
            _repositoryFactory = repositoryFactory;
            _systemProvider = systemProvider;
            _mapper = mapper;
        }

        public async Task<AdminSearchLearnerFilters> GetAdminSearchLearnerFiltersAsync()
        {
            return new AdminSearchLearnerFilters
            {
                AwardingOrganisations = await _adminDashboardRepository.GetAwardingOrganisationFiltersAsync(),
                AcademicYears = await _adminDashboardRepository.GetAcademicYearFiltersAsync(_systemProvider.UtcToday)
            };
        }

        public Task<PagedResponse<AdminSearchLearnerDetail>> GetAdminSearchLearnerDetailsAsync(AdminSearchLearnerRequest request)
        {
            return _adminDashboardRepository.SearchLearnerDetailsAsync(request);
        }

        public async Task<AdminLearnerRecord> GetAdminLearnerRecordAsync(int registrationPathwayId)
        {
            var tqRegistrationPathway = await _adminDashboardRepository.GetLearnerRecordAsync(registrationPathwayId);
            return _mapper.Map<AdminLearnerRecord>(tqRegistrationPathway);
        }

        public async Task<bool> ProcessChangeStartYearAsync(ReviewChangeStartYearRequest request)
        {
            var tqRegistrationPathwayRepository = _repositoryFactory.GetRepository<TqRegistrationPathway>();

            var pathway = await tqRegistrationPathwayRepository.GetFirstOrDefaultAsync(p => p.Id == request.RegistrationPathwayId);
            if (pathway == null) return false;

            pathway.AcademicYear = request.ChangeStartYearDetails.StartYearTo;
            bool updated = await tqRegistrationPathwayRepository.UpdateWithSpecifedColumnsOnlyAsync(pathway, u => u.AcademicYear, u => u.ModifiedBy, u => u.ModifiedOn) > 0;

            if (updated)
            {
                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                var changeLog = CreateChangeLog(request, request.ChangeStartYearDetails);

                return await changeLongRepository.CreateAsync(changeLog) > 0;
            }

            return false;
        }

        public async Task<bool> ProcessChangeIndustryPlacementAsync(ReviewChangeIndustryPlacementRequest request)
        {
            var industryPlacementRepository = _repositoryFactory.GetRepository<IndustryPlacement>();

            var industryPlacement = await industryPlacementRepository.GetFirstOrDefaultAsync(p => p.TqRegistrationPathwayId == request.RegistrationPathwayId);
            int status;

            if (industryPlacement == null)
            {
                industryPlacement = new IndustryPlacement
                {
                    CreatedBy = request.CreatedBy,
                    TqRegistrationPathwayId = request.RegistrationPathwayId,
                    Status = request.ChangeIPDetails.IndustryPlacementStatusTo,
                    Details = request.ChangeIPDetails.IndustryPlacementStatusTo == Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration ? JsonConvert.SerializeObject(ConstructIndustryPlacementDetails(request.ChangeIPDetails)) : null
                };

                status = await industryPlacementRepository.CreateAsync(industryPlacement);
            }
            else
            {
                industryPlacement.ModifiedBy = request.CreatedBy;
                industryPlacement.ModifiedOn = _systemProvider.UtcNow;
                industryPlacement.Status = request.ChangeIPDetails.IndustryPlacementStatusTo;
                industryPlacement.Details = request.ChangeIPDetails.IndustryPlacementStatusTo == Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration ? JsonConvert.SerializeObject(ConstructIndustryPlacementDetails(request.ChangeIPDetails)) : null;

                status = await industryPlacementRepository.UpdateWithSpecifedColumnsOnlyAsync(industryPlacement, u => u.Status, u => u.Details, u => u.ModifiedBy, u => u.ModifiedOn);
            }

            if (status > 0)
            {
                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                return await changeLongRepository.CreateAsync(CreateChangeLog(request, request.ChangeIPDetails)) > 0;
            }

            return false;
        }

        public async Task<bool> ProcessAdminAddPathwayResultAsync(AddPathwayResultRequest request)
        {
            var pathwayAssessmentRepo = _repositoryFactory.GetRepository<TqPathwayAssessment>();

            TqPathwayAssessment pathwayAssessment = await pathwayAssessmentRepo.GetSingleOrDefaultAsync(p => p.Id == request.PathwayAssessmentId);
            if (pathwayAssessment == null)
            {
                return false;
            }

            var pathwayResultRepo = _repositoryFactory.GetRepository<TqPathwayResult>();
            DateTime utcNow = _systemProvider.UtcNow;

            bool created = await pathwayResultRepo.CreateAsync(new TqPathwayResult
            {
                TqPathwayAssessmentId = request.PathwayAssessmentId,
                TlLookupId = request.SelectedGradeId,
                IsOptedin = true,
                StartDate = utcNow,
                EndDate = pathwayAssessment.EndDate.HasValue ? utcNow : null,
                IsBulkUpload = false,
                CreatedBy = request.CreatedBy
            }) > 0;

            if (created)
            {
                var changeLongRepository = _repositoryFactory.GetRepository<ChangeLog>();
                return await changeLongRepository.CreateAsync(CreateChangeLog(request, new { request.PathwayAssessmentId, request.SelectedGradeId })) > 0;
            }

            return false;
        }

        private ChangeLog CreateChangeLog(ReviewChangeRequest request, object details)
        {
            const string SystemUser = "System";

            var changeLog = new ChangeLog
            {
                ChangeType = (int)request.ChangeType,
                ReasonForChange = request.ChangeReason,
                DateOfRequest = request.RequestDate,
                Details = JsonConvert.SerializeObject(details),
                ZendeskTicketID = request.ZendeskId,
                Name = request.ContactName,
                TqRegistrationPathwayId = request.RegistrationPathwayId,
                CreatedBy = string.IsNullOrEmpty(request.CreatedBy) ? SystemUser : request.CreatedBy
            };

            return changeLog;
        }

        private IndustryPlacementDetails ConstructIndustryPlacementDetails(ChangeIPDetails change)
        {
            return new IndustryPlacementDetails
            {
                IndustryPlacementStatus = change.IndustryPlacementStatusTo.ToString(),
                HoursSpentOnPlacement = change.HoursSpentOnPlacementTo,
                SpecialConsiderationReasons = !change.SpecialConsiderationReasonsTo.IsNullOrEmpty() ? change.SpecialConsiderationReasonsTo : new List<int?>()
            };
        }
    }
}