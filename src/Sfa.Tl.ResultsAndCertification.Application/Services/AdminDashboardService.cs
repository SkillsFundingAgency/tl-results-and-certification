using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _adminDashboardRepository;
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;
        private readonly IRepository<IndustryPlacement> _industryPlacementRepository;
        private readonly IRepository<TqPathwayAssessment> _tqPathwayAssessmentRepository;
        private readonly IRepository<TqSpecialismAssessment> _tqSpecialismAssessmentRepository;
        private readonly ISystemProvider _systemProvider;
        private readonly ICommonService _commonService;
        private readonly IMapper _mapper;

        public AdminDashboardService(
            IAdminDashboardRepository adminDashboardRepository,
            IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository,
            IRepository<IndustryPlacement> industryPlacementRepository,
            IRepository<TqPathwayAssessment> tqPathwayAssessment,
            IRepository<TqSpecialismAssessment> TqSpecialismAssessment,
            ISystemProvider systemProvider,
            ICommonService commonService,
            IMapper mapper)
        {
            _adminDashboardRepository = adminDashboardRepository;
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
            _industryPlacementRepository = industryPlacementRepository;
            _tqPathwayAssessmentRepository = tqPathwayAssessment;
            _tqSpecialismAssessmentRepository = TqSpecialismAssessment;
            _systemProvider = systemProvider;
            _commonService = commonService;
            _mapper = mapper;
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
            var pathway = await _tqRegistrationPathwayRepository.GetFirstOrDefaultAsync(p => p.Id == request.RegistrationPathwayId);
            if (pathway == null) return false;

            pathway.AcademicYear = request.ChangeStartYearDetails.StartYearTo;
            var status = await _tqRegistrationPathwayRepository.UpdateWithSpecifedColumnsOnlyAsync(pathway, u => u.AcademicYear, u => u.ModifiedBy, u => u.ModifiedOn);

            if (status > 0)
                return await _commonService.AddChangelog(CreateChangeLogRequest(request, JsonConvert.SerializeObject((request.ChangeStartYearDetails))));
            return false;
        }


        public async Task<bool> ProcessAddCoreAssessmentAsync(ReviewAddCoreAssessmentRequest request)
        {
            var pathway = await _tqRegistrationPathwayRepository.GetFirstOrDefaultAsync(p => p.Id == request.RegistrationPathwayId);
            if (pathway == null) return false;
            int status;

            var pathwayAssessment = new TqPathwayAssessment
            {
                CreatedBy = request.CreatedBy,
                TqRegistrationPathwayId = request.RegistrationPathwayId,
                AssessmentSeriesId = request.AddCoreAssessmentDetails.AssessmentSeriesId,
                IsOptedin = true,
                StartDate = DateTime.Now
            };

            status = await _tqPathwayAssessmentRepository.CreateAsync(pathwayAssessment);

            if (status > 0)
            {
                return await _commonService.AddChangelog(CreateChangeLogRequest(request, JsonConvert.SerializeObject(request.AddCoreAssessmentDetails)));
            }

            return false;

        }

        public async Task<bool> ProcessAddSpecialismAssessmentAsync(ReviewAddSpecialismAssessmentRequest request)
        {
            var pathway = await _tqRegistrationPathwayRepository.GetFirstOrDefaultAsync(p => p.Id == request.RegistrationPathwayId);
            if (pathway == null) return false;
            int status;

            var specialismAssessment = new TqSpecialismAssessment
            {
                CreatedBy = request.CreatedBy,
                TqRegistrationSpecialismId = request.SpecialismId,
                AssessmentSeriesId = request.AddSpecialismDetails.AssessmentSeriesId,
                IsOptedin = true,
                StartDate = DateTime.Now
            };

            status = await _tqSpecialismAssessmentRepository.CreateAsync(specialismAssessment);

            if (status > 0)
            {
                return await _commonService.AddChangelog(CreateChangeLogRequest(request, JsonConvert.SerializeObject(request.AddSpecialismDetails)));
            }

            return false;

        }





        public async Task<bool> ProcessChangeIndustryPlacementAsync(ReviewChangeIndustryPlacementRequest request)
        {
            var industryPlacement = await _industryPlacementRepository.GetFirstOrDefaultAsync(p => p.TqRegistrationPathwayId == request.RegistrationPathwayId);
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

                status = await _industryPlacementRepository.CreateAsync(industryPlacement);
            }
            else
            {
                industryPlacement.ModifiedBy = request.CreatedBy;
                industryPlacement.ModifiedOn = _systemProvider.UtcNow;
                industryPlacement.Status = request.ChangeIPDetails.IndustryPlacementStatusTo;
                industryPlacement.Details = request.ChangeIPDetails.IndustryPlacementStatusTo == Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration ? JsonConvert.SerializeObject(ConstructIndustryPlacementDetails(request.ChangeIPDetails)) : null;

                status = await _industryPlacementRepository.UpdateWithSpecifedColumnsOnlyAsync(industryPlacement, u => u.Status, u => u.Details, u => u.ModifiedBy, u => u.ModifiedOn);
            }

            if (status > 0)
            {
                return await _commonService.AddChangelog(CreateChangeLogRequest(request, JsonConvert.SerializeObject(request.ChangeIPDetails)));
            }

            return false;
        }

        private ChangeLog CreateChangeLogRequest(ReviewChangeRequest request, string details)
        {
            const string SystemUser = "System";

            var changeLog = new ChangeLog
            {
                ChangeType = (int)request.ChangeType,
                ReasonForChange = request.ChangeReason,
                DateOfRequest = Convert.ToDateTime(request.RequestDate),
                Details = details,
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

        public async Task<bool> ProcessRemovePathwayAssessmentEntryAsync(ReviewRemoveAssessmentEntryRequest model)
        {
            var pathwayAssessment = await _pathwayAssessmentRepository.GetFirstOrDefaultAsync(pa => pa.Id == model.AssessmentId && pa.IsOptedin
                                                                                              && pa.EndDate == null && (pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active ||
                                                                                              pa.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn)
                                                                                              && !pa.TqPathwayResults.Any(x => x.IsOptedin && x.EndDate == null));
            if (pathwayAssessment == null) return false;

            pathwayAssessment.IsOptedin = false;
            pathwayAssessment.EndDate = DateTime.UtcNow;
            pathwayAssessment.ModifiedOn = DateTime.UtcNow;
            pathwayAssessment.ModifiedBy = model.CreatedBy;

            var isSuccess = await _pathwayAssessmentRepository.UpdateAsync(pathwayAssessment) > 0;

            if (isSuccess)
            {
                return await _commonService.AddChangelog(CreateChangeLogRequest(model, JsonConvert.SerializeObject(model.ChangeAssessmentDetails)));
            }

            return false;
        }

        public async Task<bool> ProcessRemoveSpecialismAssessmentEntryAsync(ReviewRemoveAssessmentEntryRequest model)
        {
            if (model.AssessmentId == 0) return false;

            var specialismAssessment = await _specialismAssessmentRepository.GetFirstOrDefaultAsync(sa => sa.Id == model.AssessmentId && sa.IsOptedin
                                                                                    && sa.EndDate == null
                                                                                    && (sa.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active ||
                                                                                    sa.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn));
            if (specialismAssessment == null) return false;


            specialismAssessment.IsOptedin = false;
            specialismAssessment.EndDate = DateTime.UtcNow;
            specialismAssessment.ModifiedOn = DateTime.UtcNow;
            specialismAssessment.ModifiedBy = model.CreatedBy;

            var isSuccess = await _specialismAssessmentRepository.UpdateAsync(specialismAssessment) > 0;

            if (isSuccess)
            {
                return await _commonService.AddChangelog(CreateChangeLogRequest(model, JsonConvert.SerializeObject(model.ChangeSpecialismAssessmentDetails)));
            }

            return false;
        }
    }
}