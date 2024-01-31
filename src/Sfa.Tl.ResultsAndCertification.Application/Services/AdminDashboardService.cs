using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _adminDashboardRepository;
        private readonly ISystemProvider _systemProvider;
        private readonly IMapper _mapper;
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;
        private readonly ICommonService _commonService;
        private readonly IRepository<IndustryPlacement> _industryPlacementRepository;
        private const string SystemUser = "System";



        public AdminDashboardService(IAdminDashboardRepository adminDashboardRepository,
            ISystemProvider systemProvider,
            IMapper mapper,
            IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository,
            ICommonService commonService,
            IRepository<IndustryPlacement> industryPlacementRepository)
        {
            _adminDashboardRepository = adminDashboardRepository;
            _systemProvider = systemProvider;
            _mapper = mapper;
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
            _commonService = commonService;
            _industryPlacementRepository = industryPlacementRepository;
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

        public async Task<AdminLearnerRecord> GetAdminLearnerRecordAsync(int pathwayId)
        {
            var _academicYearToBe = new List<int>();

            var result = await _adminDashboardRepository.GetAdminLearnerRecordAsync(pathwayId);
            var _adminLearnerRecord = _mapper.Map<AdminLearnerRecord>(result);

            for (int i = result.AcademicYear - 1, j = 1; i >= result.TlevelStartYear && j <= 2; i--, j++)
                _academicYearToBe.Add(i);

            _adminLearnerRecord.AcademicStartYearsToBe = _academicYearToBe;

            return _adminLearnerRecord;
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

        public async Task<bool> ProcessChangeIndustryPlacementAsync(ReviewChangeIndustryPlacementRequest request)
        {
            var industryPlacement = await _industryPlacementRepository.GetFirstOrDefaultAsync(p => p.TqRegistrationPathwayId == request.RegistrationPathwayId);
            int status;

            
            if (industryPlacement == null)
            {
                    industryPlacement = new IndustryPlacement()
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
                industryPlacement.ModifiedOn = DateTime.UtcNow;
                industryPlacement.Status = request.ChangeIPDetails.IndustryPlacementStatusTo;
                industryPlacement.Details = request.ChangeIPDetails.IndustryPlacementStatusTo == Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration ? JsonConvert.SerializeObject(ConstructIndustryPlacementDetails(request.ChangeIPDetails)) : null;
                status = await _industryPlacementRepository.UpdateWithSpecifedColumnsOnlyAsync(industryPlacement, u => u.Status, u => u.Details, u => u.ModifiedBy, u => u.ModifiedOn);
            }
            if (status > 0)
                return await _commonService.AddChangelog(CreateChangeLogRequest(request, JsonConvert.SerializeObject(request.ChangeIPDetails)));
            return false;
        }


        private ChangeLog CreateChangeLogRequest(ReviewChangeRequest request,string details)
        {
            var changeLog = new ChangeLog()
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
                SpecialConsiderationReasons = change.SpecialConsiderationReasonsTo != null && change.SpecialConsiderationReasonsTo.Any() ? change.SpecialConsiderationReasonsTo : new List<int?>()
            };
        }

    }

}
