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

            if (request.IndustryPlacementStatus == IndustryPlacementStatus.NotSpecified || (industryPlacement != null && (industryPlacement.Status == IndustryPlacementStatus.Completed || industryPlacement.Status == IndustryPlacementStatus.CompletedWithSpecialConsideration)))
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
