using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract = Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using DbModel = Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class IndustryPlacementService : IIndustryPlacementService
    {
        private readonly IRepository<IpLookup> _ipLookupRepository;
        private readonly IRepository<IpModelTlevelCombination> _ipModelTlevelCombinationRepository;
        private readonly IRepository<IpTempFlexTlevelCombination> _ipTempFlexTlevelCombinationRepository;
        private readonly IRepository<DbModel.IpTempFlexNavigation> _ipTempFlexNavigationRepository;
        private readonly IRepository<IndustryPlacement> _industryPlacementRepository;
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;

        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public IndustryPlacementService(
            IRepository<IpLookup> ipLookupRepository,
            IRepository<IpModelTlevelCombination> ipModelTlevelCombinationRepository,
            IRepository<IpTempFlexTlevelCombination> ipTempFlexTlevelCombinationRepository,
            IRepository<DbModel.IpTempFlexNavigation> ipTempFlexNavigationRepository,
            IRepository<IndustryPlacement> industryPlacementRepository,
            IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository,
            IMapper mapper, ILogger<IndustryPlacementService> logger)
        {
            _ipLookupRepository = ipLookupRepository;
            _ipModelTlevelCombinationRepository = ipModelTlevelCombinationRepository;
            _ipTempFlexTlevelCombinationRepository = ipTempFlexTlevelCombinationRepository;
            _ipTempFlexNavigationRepository = ipTempFlexNavigationRepository;
            _industryPlacementRepository = industryPlacementRepository;
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> ProcessIndustryPlacementDetailsAsync(IndustryPlacementRequest request)
        {
            var pathway = await _tqRegistrationPathwayRepository.GetManyAsync(p => p.Id == request.RegistrationPathwayId
                                                                              && p.TqRegistrationProfileId == request.ProfileId
                                                                              && p.TqProvider.TlProvider.UkPrn == request.ProviderUkprn
                                                                              && p.TqProvider.TqAwardingOrganisation.TlPathway.Id == request.PathwayId
                                                                              && (p.Status == RegistrationPathwayStatus.Active
                                                                              || p.Status == RegistrationPathwayStatus.Withdrawn))
                                                                 .OrderByDescending(ip => ip.CreatedOn)
                                                                 .FirstOrDefaultAsync();

            if (pathway == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found to update industry placement for profielId = {request.ProfileId}. Method: ProcessIndustryPlacementDetailsAsync({request})");
                return false;
            }

            var industryPlacement = await _industryPlacementRepository.GetManyAsync(ip => ip.TqRegistrationPathwayId == pathway.Id)
                                                                      .OrderByDescending(ip => ip.CreatedOn)
                                                                      .FirstOrDefaultAsync();

            var isValidData = await IsValidIndustryPlacementData(request, request.PathwayId, pathway.AcademicYear);

            if (!isValidData || (industryPlacement != null && (industryPlacement.Status == IndustryPlacementStatus.Completed || industryPlacement.Status == IndustryPlacementStatus.CompletedWithSpecialConsideration)))
                return false;

            int status = 0;

            if (industryPlacement == null)
            {
                status = await _industryPlacementRepository.CreateAsync(new IndustryPlacement
                {
                    TqRegistrationPathwayId = request.RegistrationPathwayId,
                    Status = request.IndustryPlacementStatus,
                    Details = request.IndustryPlacementDetails != null ? JsonConvert.SerializeObject(request.IndustryPlacementDetails) : null,
                    CreatedBy = request.PerformedBy
                });
            }
            else
            {
                industryPlacement.Status = request.IndustryPlacementStatus;
                industryPlacement.Details = request.IndustryPlacementDetails != null ? JsonConvert.SerializeObject(request.IndustryPlacementDetails) : null;
                industryPlacement.ModifiedBy = request.PerformedBy;
                industryPlacement.ModifiedOn = DateTime.UtcNow;                

                status = await _industryPlacementRepository.UpdateWithSpecifedColumnsOnlyAsync(industryPlacement, u => u.Status, u => u.Details, u => u.ModifiedBy, u => u.ModifiedOn);
            }

            return status > 0;
        }

        private async Task<bool> IsValidIndustryPlacementData(IndustryPlacementRequest request, int? pathwayId, int academicYear)
        {
            if (request == null || request.IndustryPlacementStatus == IndustryPlacementStatus.NotSpecified)
                return false;

            if (request.IndustryPlacementStatus == IndustryPlacementStatus.NotCompleted && request.IndustryPlacementDetails != null)
                return false;

            if (request.IndustryPlacementStatus == IndustryPlacementStatus.NotCompleted && request.IndustryPlacementDetails == null)
                return true;

            if (request.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
            {
                if (request.IndustryPlacementDetails.HoursSpentOnPlacement == null || request.IndustryPlacementDetails.HoursSpentOnPlacement <= 0
                    || request.IndustryPlacementDetails.SpecialConsiderationReasons == null || !request.IndustryPlacementDetails.SpecialConsiderationReasons.Any())
                    return false;

                var specialConsiderations = await GetIpLookupDataAsync(IpLookupType.SpecialConsideration, pathwayId);

                specialConsiderations = specialConsiderations?.Where(x => academicYear >= x.StartDate.Year && (x.EndDate == null || academicYear <= x.EndDate.Value.Year))?.ToList();

                if (specialConsiderations == null || !specialConsiderations.Any())
                    return false;

                var specialConsiderationIds = specialConsiderations.Select(s => s.Id).ToList();

                var isValidSpecialConsiderations = request.IndustryPlacementDetails.SpecialConsiderationReasons.All(x => specialConsiderationIds.Contains(x.Value));

                if (!isValidSpecialConsiderations)
                    return false;
            }

            if (request.IndustryPlacementStatus == IndustryPlacementStatus.Completed)
            {
                // special considerations objects should not be populated
                if (request.IndustryPlacementDetails.HoursSpentOnPlacement != null 
                    || (request.IndustryPlacementDetails.SpecialConsiderationReasons != null && request.IndustryPlacementDetails.SpecialConsiderationReasons.Any()))
                    return false;                
            }

            // Ip Models
            if (request.IndustryPlacementDetails.IndustryPlacementModelsUsed)
            {
                if (!request.IndustryPlacementDetails.MultipleEmployerModelsUsed.HasValue)
                    return false;

                var ipModels = await GetIpLookupDataAsync(IpLookupType.IndustryPlacementModel, pathwayId);

                if (ipModels == null || !ipModels.Any())
                    return false;

                var ipModelIds = ipModels.Select(i => i.Id);

                if (request.IndustryPlacementDetails.MultipleEmployerModelsUsed.Value)
                {
                    if (request.IndustryPlacementDetails.OtherIndustryPlacementModels == null || !request.IndustryPlacementDetails.OtherIndustryPlacementModels.Any())
                        return false;

                    var isValidOtherIpModels = request.IndustryPlacementDetails.OtherIndustryPlacementModels.All(x => ipModelIds.Contains(x.Value));

                    if (!isValidOtherIpModels)
                        return false;
                }
                else
                {
                    if (request.IndustryPlacementDetails.IndustryPlacementModels == null || !request.IndustryPlacementDetails.IndustryPlacementModels.Any())
                        return false;

                    var isValidIpModels = request.IndustryPlacementDetails.IndustryPlacementModels.All(x => ipModelIds.Contains(x.Value));

                    if (!isValidIpModels)
                        return false;
                }
            }
            else
            {
                // If IndustryPlacementModels not used then IP models should not be populated
                if (request.IndustryPlacementDetails.MultipleEmployerModelsUsed.HasValue 
                    || (request.IndustryPlacementDetails.OtherIndustryPlacementModels != null &&request.IndustryPlacementDetails.OtherIndustryPlacementModels.Any())
                    || (request.IndustryPlacementDetails.IndustryPlacementModels != null && request.IndustryPlacementDetails.IndustryPlacementModels.Any()))
                    return false;
            }

            // Temporary flexibility
            if (!request.IndustryPlacementDetails.TemporaryFlexibilitiesUsed.HasValue || !request.IndustryPlacementDetails.TemporaryFlexibilitiesUsed.Value)
            {
                if (request.IndustryPlacementDetails.TemporaryFlexibilities != null && request.IndustryPlacementDetails.TemporaryFlexibilities.Any())
                    return false;
            }

            if (request.IndustryPlacementDetails.TemporaryFlexibilitiesUsed.HasValue)
            {
                var tempFlexibilities = await GetIpLookupDataAsync(IpLookupType.TemporaryFlexibility, pathwayId);

                if (tempFlexibilities == null || !tempFlexibilities.Any() || !request.IndustryPlacementDetails.TemporaryFlexibilities.Any())
                    return false;

                // check if temp flex options are valid
                var tempFlexIds = tempFlexibilities.Select(i => i.Id).ToList();
                var isValidTempFlexibilities = request.IndustryPlacementDetails.TemporaryFlexibilities.All(x => tempFlexIds.Contains(x.Value));

                if (!isValidTempFlexibilities)
                    return false;
            }

            return true;
        }

        public async Task<IList<IpLookupData>> GetIpLookupDataAsync(IpLookupType ipLookupType, int? pathwayId)
        {
            return ipLookupType switch
            {
                IpLookupType.SpecialConsideration => await SpecialConsiderationReasonsAsync(),
                IpLookupType.IndustryPlacementModel => await IndustryPlacementModelsAsync(pathwayId),
                IpLookupType.TemporaryFlexibility => await TemporaryFlexibilitiesAsync(pathwayId),
                _ => null
            };
        }

        private async Task<IList<IpLookupData>> SpecialConsiderationReasonsAsync()
        {
            var lookupData = await _ipLookupRepository.GetManyAsync(x => x.TlLookup.Category == IpLookupType.SpecialConsideration.ToString()).OrderBy(x => x.SortOrder).ToListAsync();
            return _mapper.Map<IList<IpLookupData>>(lookupData);
        }

        private async Task<IList<IpLookupData>> IndustryPlacementModelsAsync(int? pathwayId)
        {
            var lookupData = await _ipModelTlevelCombinationRepository
                                    .GetManyAsync(x => x.IsActive && x.TlPathwayId == pathwayId && x.IpLookup.TlLookup.Category == IpLookupType.IndustryPlacementModel.ToString())
                                    .Select(x => x.IpLookup)
                                    .OrderBy(x => x.SortOrder).ToListAsync();

            return _mapper.Map<IList<IpLookupData>>(lookupData);
        }

        private async Task<IList<IpLookupData>> TemporaryFlexibilitiesAsync(int? pathwayId)
        {
            var lookupData = await _ipTempFlexTlevelCombinationRepository
                                    .GetManyAsync(x => x.IsActive && x.TlPathwayId == pathwayId && x.IpLookup.TlLookup.Category == IpLookupType.TemporaryFlexibility.ToString())
                                    .Select(x => x.IpLookup)
                                    .OrderBy(x => x.SortOrder).ToListAsync();

            return _mapper.Map<IList<IpLookupData>>(lookupData);
        }

        public async Task<Contract.IpTempFlexNavigation> GetTempFlexNavigationAsync(int pathwayId, int academicYear)
        {
            var navigation = await _ipTempFlexNavigationRepository.GetFirstOrDefaultAsync(x => x.TlPathwayId == pathwayId && x.AcademicYear == academicYear);
            return _mapper.Map<Contract.IpTempFlexNavigation>(navigation);
        }
    }
}
