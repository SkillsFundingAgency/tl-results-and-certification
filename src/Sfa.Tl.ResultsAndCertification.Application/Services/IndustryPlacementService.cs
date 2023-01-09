using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Comparer;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;
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

        public async Task<IList<IndustryPlacementRecordResponse>> ValidateIndustryPlacementsAsync(long providerUkprn, IEnumerable<IndustryPlacementCsvRecordResponse> csvIndustryPlacements)
        {
            var response = new List<IndustryPlacementRecordResponse>();

            var uniqueLearnerNumbers = csvIndustryPlacements.Select(x => x.Uln);
            var learnerPathways = await _tqRegistrationPathwayRepository.GetManyAsync(p => uniqueLearnerNumbers.Contains(p.TqRegistrationProfile.UniqueLearnerNumber) &&
                                                                                      p.TqProvider.TlProvider.UkPrn == providerUkprn &&
                                                                                      (p.Status == RegistrationPathwayStatus.Active || p.Status == RegistrationPathwayStatus.Withdrawn),
                                                                                      p => p.TqRegistrationProfile, p => p.IndustryPlacements, p => p.TqProvider.TqAwardingOrganisation.TlPathway)
                                                                        .ToListAsync();

            var latestPathways = learnerPathways
                    .GroupBy(x => x.TqRegistrationProfileId)
                    .Select(x => x.OrderByDescending(o => o.CreatedOn).First())
                    .ToList();

            foreach (var industryPlacement in csvIndustryPlacements)
            {
                // 1. ULN not recognised with Provider
                var registeredPathway = latestPathways.FirstOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == industryPlacement.Uln);
                if (registeredPathway == null)
                {
                    response.Add(AddStage3ValidationError(industryPlacement.RowNum, industryPlacement.Uln, ValidationMessages.UlnNotRegisteredWithProvider));
                    continue;
                }

                //// 2. Core code not registered against the learner
                //var registeredPathway1 = latestPathways.FirstOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == industryPlacement.Uln);
                //if (registeredPathway == null)
                //{
                //    response.Add(AddStage3ValidationError(industryPlacement.RowNum, industryPlacement.Uln, ValidationMessages.UlnNotRegisteredWithProvider));
                //    continue;
                //}

                var validationErrors = new List<BulkProcessValidationError>();

                // 2. Core Code is incorrect (not registered aganinst the learner)
                var isValidRegisteredCoreCode = registeredPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId.Equals(industryPlacement.CoreCode, StringComparison.InvariantCultureIgnoreCase);
                if (!isValidRegisteredCoreCode)
                    validationErrors.Add(BuildValidationError(industryPlacement, ValidationMessages.InvalidCoreCodeProvider));

                // 3. Industry Placement status not valid
                var ipStatus = EnumExtensions.GetEnumByDisplayName<IndustryPlacementStatus>(industryPlacement.IndustryPlacementStatus);
                if (ipStatus == IndustryPlacementStatus.NotSpecified)
                    validationErrors.Add(BuildValidationError(industryPlacement, ValidationMessages.InvalidIndustryPlacementStatus));

                var specialConsiderationReasonIds = new List<int?>();
                // 4. Industry Placement Special considerations not valid
                if (industryPlacement.SpecialConsiderations.Any())
                {
                    var specialConsiderations = await SpecialConsiderationReasonsAsync();

                    var specialConsiderationCodes = specialConsiderations.Select(x => x.Name);
                    var invalidSpecialConsiderationCodes = industryPlacement.SpecialConsiderations.Except(specialConsiderationCodes, StringComparer.InvariantCultureIgnoreCase);

                    if (invalidSpecialConsiderationCodes.Any())
                    {
                        validationErrors.Add(BuildValidationError(industryPlacement, ValidationMessages.InvalidSpecialConsiderationCodes));
                    }
                    else
                    {
                        specialConsiderationReasonIds = specialConsiderations.Where(sc => industryPlacement.SpecialConsiderations.Any(s => s.Equals(sc.Name, StringComparison.InvariantCultureIgnoreCase))).Select(x => (int?)x.Id).ToList();
                    }
                }

                response.Add(new IndustryPlacementRecordResponse
                {
                    TqRegistrationPathwayId = registeredPathway.Id,
                    IpStatus = (int)ipStatus,
                    IpHours = !string.IsNullOrWhiteSpace(industryPlacement.IndustryPlacementHours) ? industryPlacement.IndustryPlacementHours.ToInt() : null,
                    SpecialConsiderationReasons = specialConsiderationReasonIds
                });
            }
            return response;
        }

        public IList<IndustryPlacement> TransformIndustryPlacementsModel(IList<IndustryPlacementRecordResponse> industryPlacementsData, string performedBy)
        {
            var industryPlacements = new List<IndustryPlacement>();

            foreach (var (industryPlacement, index) in industryPlacementsData.Select((value, i) => (value, i)))
            {
                if (industryPlacement.TqRegistrationPathwayId.HasValue && industryPlacement.TqRegistrationPathwayId.Value > 0)
                {
                    industryPlacements.Add(new IndustryPlacement
                    {
                        TqRegistrationPathwayId = industryPlacement.TqRegistrationPathwayId.Value,
                        Status = EnumExtensions.GetEnum<IndustryPlacementStatus>(industryPlacement.IpStatus),
                        Details = JsonConvert.SerializeObject(ConstructIndustryPlacementDetails(industryPlacement)),
                        CreatedBy = performedBy,
                        CreatedOn = DateTime.UtcNow
                    });
                }
            }

            return industryPlacements;
        }

        public async Task<IndustryPlacementProcessResponse> CompareAndProcessIndustryPlacementsAsync(IList<IndustryPlacement> industryPlacementsToProcess)
        {
            var response = new IndustryPlacementProcessResponse();

            // Prepare Industry Placements
            var newOrAmendedIndustryPlacementRecords = await PrepareNewAndAmendedIndustryPlacements(industryPlacementsToProcess, response);

            if (response.IsValid)
                response.IsSuccess = await _industryPlacementRepository.UpdateManyAsync(newOrAmendedIndustryPlacementRecords) > 0;

            return response;
        }

        private async Task<List<IndustryPlacement>> PrepareNewAndAmendedIndustryPlacements(IList<IndustryPlacement> industryPlacementsToProcess, IndustryPlacementProcessResponse response)
        {
            var industryPlacementComparer = new IndustryPlacementEqualityComparer();
            var amendedIndustryPlacements = new List<IndustryPlacement>();
            var newAndAmendedIndustryPlacementRecords = new List<IndustryPlacement>();

            var tqRegistrationPathwayIds = new HashSet<int>();
            industryPlacementsToProcess.ToList().ForEach(r => tqRegistrationPathwayIds.Add(r.TqRegistrationPathwayId));

            var existingIndustryPlacementsFromDb = await _industryPlacementRepository.GetManyAsync(ip => tqRegistrationPathwayIds.Contains(ip.TqRegistrationPathwayId)).ToListAsync();
            var newIndustryPlacements = industryPlacementsToProcess.Except(existingIndustryPlacementsFromDb, industryPlacementComparer).ToList();
            var matchedIndustryPlacements = industryPlacementsToProcess.Intersect(existingIndustryPlacementsFromDb, industryPlacementComparer).ToList();
            var unchangedIndustryPlacements = matchedIndustryPlacements.Intersect(existingIndustryPlacementsFromDb, new IndustryPlacementRecordEqualityComparer()).ToList();
            var hasAnyMatchedIndustryPlacementsToProcess = matchedIndustryPlacements.Count != unchangedIndustryPlacements.Count;

            if (hasAnyMatchedIndustryPlacementsToProcess)
            {
                amendedIndustryPlacements = matchedIndustryPlacements.Except(unchangedIndustryPlacements, industryPlacementComparer).ToList();
                amendedIndustryPlacements.ForEach(amendedIndustryPlacement =>
                {
                    var existingIndustryPlacement = existingIndustryPlacementsFromDb.FirstOrDefault(existingIndustryPlacement => existingIndustryPlacement.TqRegistrationPathwayId == amendedIndustryPlacement.TqRegistrationPathwayId);
                    if (existingIndustryPlacement != null)
                    {
                        var hasIndustryPlacementChanged = amendedIndustryPlacement.Status != existingIndustryPlacement.Status;

                        if (hasIndustryPlacementChanged)
                        {
                            existingIndustryPlacement.Status = amendedIndustryPlacement.Status;
                            existingIndustryPlacement.Details = amendedIndustryPlacement.Details;
                            existingIndustryPlacement.ModifiedBy = amendedIndustryPlacement.CreatedBy;
                            existingIndustryPlacement.ModifiedOn = DateTime.UtcNow;

                            newAndAmendedIndustryPlacementRecords.Add(existingIndustryPlacement);
                        }
                    }
                });
            }

            if (response.IsValid && newIndustryPlacements.Any())
                newAndAmendedIndustryPlacementRecords.AddRange(newIndustryPlacements);

            return newAndAmendedIndustryPlacementRecords;
        }

        private static IndustryPlacementDetails ConstructIndustryPlacementDetails(IndustryPlacementRecordResponse industryPlacement)
        {
            return new IndustryPlacementDetails
            {
                IndustryPlacementStatus = EnumExtensions.GetDisplayName<IndustryPlacementStatus>(industryPlacement.IpStatus),
                HoursSpentOnPlacement = industryPlacement.IpHours,
                SpecialConsiderationReasons = industryPlacement.SpecialConsiderationReasons != null && industryPlacement.SpecialConsiderationReasons.Any() ? industryPlacement.SpecialConsiderationReasons : null
            };
        }

        private IndustryPlacementRecordResponse AddStage3ValidationError(int rowNum, long uln, string errorMessage)
        {
            return new IndustryPlacementRecordResponse()
            {
                ValidationErrors = new List<BulkProcessValidationError>()
                {
                    new BulkProcessValidationError
                    {
                        RowNum = rowNum.ToString(),
                        Uln = uln.ToString(),
                        ErrorMessage = errorMessage
                    }
                }
            };
        }

        private BulkProcessValidationError BuildValidationError(IndustryPlacementCsvRecordResponse industryPlacement, string message)
        {
            return new BulkProcessValidationError { RowNum = industryPlacement.RowNum.ToString(), Uln = industryPlacement.Uln.ToString(), ErrorMessage = message };
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
                    || (request.IndustryPlacementDetails.OtherIndustryPlacementModels != null && request.IndustryPlacementDetails.OtherIndustryPlacementModels.Any())
                    || (request.IndustryPlacementDetails.IndustryPlacementModels != null && request.IndustryPlacementDetails.IndustryPlacementModels.Any()))
                    return false;
            }

            // Temporary Flexibilities Validation

            // If TemporaryFlexibilitiesUsed is null And BlendedTemporaryFlexibilityUsed is null then TemporaryFlexibilities list should not have any values. If so return false
            if (request.IndustryPlacementDetails.TemporaryFlexibilitiesUsed == null && request.IndustryPlacementDetails.BlendedTemporaryFlexibilityUsed == null)
            {
                if (request.IndustryPlacementDetails.TemporaryFlexibilities != null && request.IndustryPlacementDetails.TemporaryFlexibilities.Any())
                    return false;
            }

            // If TemporaryFlexibilitiesUsed is used then TemporaryFlexibilities list should have values. If not return false
            if (request.IndustryPlacementDetails.TemporaryFlexibilitiesUsed != null && request.IndustryPlacementDetails.TemporaryFlexibilitiesUsed.Value)
            {
                if (request.IndustryPlacementDetails.TemporaryFlexibilities == null || !request.IndustryPlacementDetails.TemporaryFlexibilities.Any())
                    return false;
            }

            // If TemporaryFlexibilitiesUsed is not used then TemporaryFlexibilities list should not have any values. If so return false
            if (request.IndustryPlacementDetails.TemporaryFlexibilitiesUsed != null && request.IndustryPlacementDetails.TemporaryFlexibilitiesUsed.Value == false)
            {
                if (request.IndustryPlacementDetails.TemporaryFlexibilities != null && request.IndustryPlacementDetails.TemporaryFlexibilities.Any())
                    return false;
            }

            // Temporary flexibility not exists and BlendedTemporaryFlexibilityUsed is false then TemporaryFlexibilities list should not have any values. If so return false
            if (request.IndustryPlacementDetails.TemporaryFlexibilitiesUsed == null &&
                request.IndustryPlacementDetails.BlendedTemporaryFlexibilityUsed.HasValue && request.IndustryPlacementDetails.BlendedTemporaryFlexibilityUsed.Value == false)
            {
                if (request.IndustryPlacementDetails.TemporaryFlexibilities != null && request.IndustryPlacementDetails.TemporaryFlexibilities.Any())
                    return false;
            }

            // Temporary flexibility not exists and BlendedTemporaryFlexibilityUsed is true then we need to add BlendedPlacement Temp Flex Id to TemporaryFlexibilities list. Then if validation fails return false
            if (request.IndustryPlacementDetails.TemporaryFlexibilitiesUsed == null &&
                request.IndustryPlacementDetails.BlendedTemporaryFlexibilityUsed.HasValue && request.IndustryPlacementDetails.BlendedTemporaryFlexibilityUsed.Value)
            {
                var tempFlexibilities = await GetIpLookupDataAsync(IpLookupType.TemporaryFlexibility, pathwayId);

                if (tempFlexibilities != null && tempFlexibilities.Any())
                {
                    var blendedPlacementTempFlex = tempFlexibilities.FirstOrDefault(t => t.Name.Equals(Constants.BlendedPlacements, StringComparison.InvariantCultureIgnoreCase));

                    if (blendedPlacementTempFlex != null)
                    {
                        if (request.IndustryPlacementDetails.TemporaryFlexibilities != null)
                        {
                            // If blended placement Id do not exist's then add it
                            if (!request.IndustryPlacementDetails.TemporaryFlexibilities.Any(tfId => tfId == blendedPlacementTempFlex.Id))
                            {
                                request.IndustryPlacementDetails.TemporaryFlexibilities.Add(blendedPlacementTempFlex.Id);
                            }
                        }
                        else
                        {
                            // If Temporary Flexibilites is null then add new List with blended placement id
                            request.IndustryPlacementDetails.TemporaryFlexibilities = new List<int?> { blendedPlacementTempFlex.Id };
                        }
                    }
                }

                // Perform check to see if TemporaryFlexibilities has values. If not return false
                if (request.IndustryPlacementDetails.TemporaryFlexibilities == null || !request.IndustryPlacementDetails.TemporaryFlexibilities.Any())
                    return false;
            }

            //if (request.IndustryPlacementDetails.TemporaryFlexibilitiesUsed.HasValue)
            if (request.IndustryPlacementDetails.TemporaryFlexibilities != null && request.IndustryPlacementDetails.TemporaryFlexibilities.Any())
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
