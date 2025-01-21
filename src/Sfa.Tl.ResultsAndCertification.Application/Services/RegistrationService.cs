﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Models;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Comparer;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IProviderRepository _tqProviderRepository;
        private readonly IRegistrationRepository _tqRegistrationRepository;
        private readonly IRepository<TqRegistrationPathway> _tqRegistrationPathwayRepository;
        private readonly IRepository<TqRegistrationSpecialism> _tqRegistrationSpecialismRepository;
        private readonly IRepository<TqPathwayAssessment> _tqPathwayAssessmentRepository;
        private readonly ICommonService _commonService;
        private readonly ISystemProvider _systemProvider;
        private readonly IMapper _mapper;
        private readonly ILogger<IRegistrationRepository> _logger;

        public RegistrationService(IProviderRepository providerRespository,
            IRegistrationRepository tqRegistrationRepository,
            IRepository<TqRegistrationPathway> tqRegistrationPathwayRepository,
            IRepository<TqPathwayAssessment> tqPathwayAssessmentRepository,
            IRepository<TqRegistrationSpecialism> tqRegistrationSpecialismRepository,
            ICommonService commonService,
            ISystemProvider systemProvider,
            IMapper mapper,
            ILogger<IRegistrationRepository> logger
            )
        {
            _tqProviderRepository = providerRespository;
            _tqRegistrationRepository = tqRegistrationRepository;
            _tqRegistrationPathwayRepository = tqRegistrationPathwayRepository;
            _tqPathwayAssessmentRepository = tqPathwayAssessmentRepository;
            _tqRegistrationSpecialismRepository = tqRegistrationSpecialismRepository;
            _commonService = commonService;
            _systemProvider = systemProvider;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IList<RegistrationRecordResponse>> ValidateRegistrationTlevelsAsync(long aoUkprn, IEnumerable<RegistrationCsvRecordResponse> validRegistrationsData)
        {
            var response = new List<RegistrationRecordResponse>();
            var aoProviderTlevels = await GetAllTLevelsByAoUkprnAsync(aoUkprn);
            var academicYears = await _commonService.GetAcademicYearsAsync();

            foreach (var registrationData in validRegistrationsData)
            {
                var academicYear = academicYears.FirstOrDefault(x => x.Name.Equals(registrationData.AcademicYearName, StringComparison.InvariantCultureIgnoreCase));
                if (academicYear == null)
                {
                    response.Add(AddStage3ValidationError(registrationData.RowNum, registrationData.Uln, ValidationMessages.AcademicYearIsNotValid));
                    continue;
                }
                else
                    registrationData.AcademicYear = academicYear.Year;

                var isProviderRegisteredWithAwardingOrganisation = aoProviderTlevels.Any(t => t.ProviderUkprn == registrationData.ProviderUkprn);
                if (!isProviderRegisteredWithAwardingOrganisation)
                {
                    response.Add(AddStage3ValidationError(registrationData.RowNum, registrationData.Uln, ValidationMessages.ProviderNotRegisteredWithAo));
                    continue;
                }

                var technicalQualification = aoProviderTlevels.FirstOrDefault(tq => tq.ProviderUkprn == registrationData.ProviderUkprn && tq.PathwayLarId == registrationData.CoreCode);
                if (technicalQualification == null)
                {
                    response.Add(AddStage3ValidationError(registrationData.RowNum, registrationData.Uln, ValidationMessages.CoreNotRegisteredWithProvider));
                    continue;
                }

                if (registrationData.SpecialismCodes.Count() > 0)
                {
                    var specialismCodes = technicalQualification.TlSpecialismLarIds.Select(x => x.Value);
                    var invalidSpecialismCodes = registrationData.SpecialismCodes.Except(specialismCodes, StringComparer.InvariantCultureIgnoreCase);

                    if (invalidSpecialismCodes.Any())
                    {
                        response.Add(AddStage3ValidationError(registrationData.RowNum, registrationData.Uln, ValidationMessages.SpecialismNotValidWithCore));
                        continue;
                    }

                    var isSpecialismPartOfCouplets = IsSpecialismPartOfCouplet(technicalQualification.TlSpecialismCombinations, registrationData.SpecialismCodes);

                    if (isSpecialismPartOfCouplets)
                    {
                        if (!IsValidCouplet(technicalQualification.TlSpecialismCombinations, registrationData.SpecialismCodes))
                        {
                            response.Add(AddStage3ValidationError(registrationData.RowNum, registrationData.Uln, registrationData.SpecialismCodes.Count() == 1 ? ValidationMessages.SpecialismCannotBeSelectedAsSingleOption : ValidationMessages.SpecialismIsNotValid));
                            continue;
                        }
                    }
                }

                response.Add(new RegistrationRecordResponse
                {
                    Uln = registrationData.Uln,
                    FirstName = registrationData.FirstName,
                    LastName = registrationData.LastName,
                    DateOfBirth = registrationData.DateOfBirth,
                    AcademicYear = registrationData.AcademicYear,
                    TqProviderId = technicalQualification.TqProviderId,
                    TqAwardingOrganisationId = technicalQualification.TqAwardingOrganisationId,
                    TlPathwayId = technicalQualification.TlPathwayId,
                    TlSpecialismLarIds = technicalQualification.TlSpecialismLarIds.Where(s => registrationData.SpecialismCodes.Any(sc => sc.Equals(s.Value, StringComparison.InvariantCultureIgnoreCase))),
                    TlAwardingOrganisatonId = technicalQualification.TlAwardingOrganisatonId,
                    TlProviderId = technicalQualification.TlProviderId
                });
            };

            return response;
        }

        private async Task<RegistrationRecordResponse> ValidateManualRegistrationTlevelsAsync(RegistrationRequest registrationData)
        {
            var aoProviderTlevels = await GetAllTLevelsByAoUkprnAsync(registrationData.AoUkprn);

            var isProviderRegisteredWithAwardingOrganisation = aoProviderTlevels.Any(t => t.ProviderUkprn == registrationData.ProviderUkprn);
            if (!isProviderRegisteredWithAwardingOrganisation)
            {
                return AddStage3ValidationError(0, registrationData.Uln, ValidationMessages.ProviderNotRegisteredWithAo);
            }

            var technicalQualification = aoProviderTlevels.FirstOrDefault(tq => tq.ProviderUkprn == registrationData.ProviderUkprn && tq.PathwayLarId == registrationData.CoreCode);
            if (technicalQualification == null)
            {
                return AddStage3ValidationError(0, registrationData.Uln, ValidationMessages.CoreNotRegisteredWithProvider);
            }

            if (registrationData.SpecialismCodes.Count() > 0)
            {
                var specialismCodes = technicalQualification.TlSpecialismLarIds.Select(x => x.Value);
                var invalidSpecialismCodes = registrationData.SpecialismCodes.Except(specialismCodes, StringComparer.InvariantCultureIgnoreCase);

                if (invalidSpecialismCodes.Any())
                {
                    return AddStage3ValidationError(0, registrationData.Uln, ValidationMessages.SpecialismNotValidWithCore);
                }

                var isSpecialismPartOfCouplets = IsSpecialismPartOfCouplet(technicalQualification.TlSpecialismCombinations, registrationData.SpecialismCodes);

                if (isSpecialismPartOfCouplets)
                {
                    if (!IsValidCouplet(technicalQualification.TlSpecialismCombinations, registrationData.SpecialismCodes))
                    {
                        return AddStage3ValidationError(0, registrationData.Uln, registrationData.SpecialismCodes.Count() == 1 ? ValidationMessages.SpecialismCannotBeSelectedAsSingleOption : ValidationMessages.SpecialismIsNotValid);
                    }
                }
            }

            return new RegistrationRecordResponse
            {
                Uln = registrationData.Uln,
                FirstName = registrationData.FirstName,
                LastName = registrationData.LastName,
                DateOfBirth = registrationData.DateOfBirth,
                AcademicYear = registrationData.AcademicYear,
                TqProviderId = technicalQualification.TqProviderId,
                TqAwardingOrganisationId = technicalQualification.TqAwardingOrganisationId,
                TlPathwayId = technicalQualification.TlPathwayId,
                TlSpecialismLarIds = technicalQualification.TlSpecialismLarIds.Where(s => registrationData.SpecialismCodes.Any(sc => sc.Equals(s.Value, StringComparison.InvariantCultureIgnoreCase))),
                TlAwardingOrganisatonId = technicalQualification.TlAwardingOrganisatonId,
                TlProviderId = technicalQualification.TlProviderId
            };
        }

        public IList<TqRegistrationProfile> TransformRegistrationModel(IList<RegistrationRecordResponse> registrationsData, string performedBy)
        {
            var registrationProfiles = new List<TqRegistrationProfile>();
            int registrationSpecialismStartIndex = Constants.RegistrationSpecialismsStartIndex;

            foreach (var (registration, index) in registrationsData.Select((value, i) => (value, i)))
            {
                registrationProfiles.Add(new TqRegistrationProfile
                {
                    Id = index - Constants.RegistrationProfileStartIndex,
                    UniqueLearnerNumber = registration.Uln,
                    Firstname = registration.FirstName,
                    Lastname = registration.LastName,
                    DateofBirth = registration.DateOfBirth,
                    CreatedBy = performedBy,
                    CreatedOn = DateTime.UtcNow,

                    TqRegistrationPathways = new List<TqRegistrationPathway>
                    {
                        new TqRegistrationPathway
                        {
                            Id = index - Constants.RegistrationPathwayStartIndex,
                            TqProviderId = registration.TqProviderId,
                            AcademicYear = registration.AcademicYear,
                            StartDate = DateTime.UtcNow,
                            Status = RegistrationPathwayStatus.Active,
                            IsBulkUpload = true,
                            TqRegistrationSpecialisms = MapSpecialisms(registration.TlSpecialismLarIds, performedBy, registrationSpecialismStartIndex),
                            TqProvider = new TqProvider
                            {
                                TqAwardingOrganisationId = registration.TqAwardingOrganisationId,
                                TlProviderId = registration.TlProviderId,
                                TqAwardingOrganisation = new TqAwardingOrganisation
                                {
                                    TlAwardingOrganisatonId = registration.TlAwardingOrganisatonId,
                                    TlPathwayId = registration.TlPathwayId,
                                }
                            },
                            CreatedBy = performedBy,
                            CreatedOn = DateTime.UtcNow
                        }
                    }
                });
                registrationSpecialismStartIndex -= registration.TlSpecialismLarIds.Count();
            }
            return registrationProfiles;
        }

        public async Task<RegistrationProcessResponse> CompareAndProcessRegistrationsAsync(IList<TqRegistrationProfile> registrationsToProcess)
        {
            var response = new RegistrationProcessResponse();

            var ulnComparer = new TqRegistrationUlnEqualityComparer();
            var amendedRegistrations = new List<TqRegistrationProfile>();
            var amendedRegistrationsToIgnore = new List<TqRegistrationProfile>();
            var amendedPathwayRecords = new List<TqRegistrationPathway>();
            var amendedSpecialismRecords = new List<TqRegistrationSpecialism>();

            var existingRegistrationsFromDb = await _tqRegistrationRepository.GetRegistrationProfilesAsync(registrationsToProcess);

            var newRegistrations = registrationsToProcess.Except(existingRegistrationsFromDb, ulnComparer).ToList();
            var matchedRegistrations = registrationsToProcess.Intersect(existingRegistrationsFromDb, ulnComparer).ToList();
            var unchangedRegistrations = matchedRegistrations.Intersect(existingRegistrationsFromDb, new TqRegistrationRecordEqualityComparer()).ToList();
            var hasAnyMatchedRegistrationsToProcess = matchedRegistrations.Count != unchangedRegistrations.Count;

            if (newRegistrations.Any())
            {
                var currentAcademicYears = await _commonService.GetCurrentAcademicYearsAsync();

                foreach (var newReg in newRegistrations)
                {
                    var isValidAcademicYear = newReg.TqRegistrationPathways.All(p => currentAcademicYears.Any(a => a.Year == p.AcademicYear));

                    if (!isValidAcademicYear)
                        response.ValidationErrors.Add(GetRegistrationValidationError(newReg.UniqueLearnerNumber, ValidationMessages.AcademicYearMustBeCurrentOne));
                }
            }

            if (hasAnyMatchedRegistrationsToProcess)
            {
                var tqRegistrationProfileComparer = new TqRegistrationProfileEqualityComparer();

                amendedRegistrations = matchedRegistrations.Except(unchangedRegistrations, ulnComparer).ToList();

                var entityIndex = new BulkRegistrationEntityIndex();

                amendedRegistrations.ForEach(amendedRegistration =>
                {
                    var existingRegistration = existingRegistrationsFromDb.FirstOrDefault(profile => profile.UniqueLearnerNumber == amendedRegistration.UniqueLearnerNumber);

                    if (existingRegistration != null)
                    {
                        #region Validation_Rules
                        // Validation Rule: Registration should not be in Withdrawn Status.
                        var latestRegPathway = existingRegistration.TqRegistrationPathways.OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                        if (latestRegPathway != null && latestRegPathway.Status == RegistrationPathwayStatus.Withdrawn)
                        {
                            var hasAoChanged = amendedRegistration.TqRegistrationPathways.All(rp => rp.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId != latestRegPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId);
                            response.ValidationErrors.Add(GetRegistrationValidationError(existingRegistration.UniqueLearnerNumber, hasAoChanged ? ValidationMessages.LearnerPreviouslyRegisteredWithAnotherAo : ValidationMessages.RegistrationCannotBeInWithdrawnStatus));
                            return;
                        }

                        // Validation Rule: Academic Year can not be changed.
                        var isAcademicYearChanged = !amendedRegistration.TqRegistrationPathways.All(p => p.AcademicYear == latestRegPathway.AcademicYear);
                        if (isAcademicYearChanged)
                        {
                            response.ValidationErrors.Add(GetRegistrationValidationError(amendedRegistration.UniqueLearnerNumber, ValidationMessages.AcademicYearCannotBeChanged));
                            return;
                        }
                        #endregion 

                        var hasBothPathwayAndSpecialismsRecordsChanged = false;
                        var hasOnlySpecialismsRecordChanged = false;
                        var hasTqRegistrationProfileRecordChanged = !tqRegistrationProfileComparer.Equals(amendedRegistration, existingRegistration);

                        amendedRegistration.Id = existingRegistration.Id; // assign existing registrionprofile table Id to amendedRegistration Id
                        amendedRegistration.TqRegistrationPathways.ToList().ForEach(p => p.TqRegistrationProfileId = existingRegistration.Id); // updating profile fk

                        // below step returns only Active Pathway and associated acitve specialisms
                        var activePathwayRegistrationsInDb = GetActivePathwayAndSpecialism(existingRegistration);

                        var pathwaysToAdd = amendedRegistration.TqRegistrationPathways.Where(mp => !activePathwayRegistrationsInDb.Any(ap => ap.TqProviderId == mp.TqProviderId)).ToList();
                        var pathwaysToUpdate = (pathwaysToAdd.Any() ? activePathwayRegistrationsInDb : activePathwayRegistrationsInDb.Where(s => amendedRegistration.TqRegistrationPathways.Any(r => r.TqProviderId == s.TqProviderId))).ToList();

                        if (pathwaysToUpdate.Any())
                        {
                            response = ValidateStage4Rules(amendedRegistration, pathwaysToUpdate, response);

                            if (response.IsValid)
                            {
                                var entitiesChangeStatus = PrepareAndAmendRegistrationData(amendedRegistration, pathwaysToAdd, pathwaysToUpdate, entityIndex);
                                hasBothPathwayAndSpecialismsRecordsChanged = entitiesChangeStatus.Item1;
                                hasOnlySpecialismsRecordChanged = entitiesChangeStatus.Item2;
                                entityIndex = entitiesChangeStatus.Item3;
                            }
                        }

                        if (response.IsValid)
                        {
                            // Consolidate Amended Registration

                            if (hasTqRegistrationProfileRecordChanged)
                            {
                                // populate existing data that cannot be changed from upload file
                                amendedRegistration.Gender = existingRegistration.Gender;
                                amendedRegistration.IsLearnerVerified = existingRegistration.IsLearnerVerified;
                                amendedRegistration.IsEnglishAndMathsAchieved = existingRegistration.IsEnglishAndMathsAchieved;
                                amendedRegistration.IsSendLearner = existingRegistration.IsSendLearner;
                                amendedRegistration.IsRcFeed = existingRegistration.IsRcFeed;
                                amendedRegistration.MathsStatus = existingRegistration.MathsStatus;
                                amendedRegistration.EnglishStatus = existingRegistration.EnglishStatus;
                                amendedRegistration.CreatedBy = existingRegistration.CreatedBy;
                                amendedRegistration.CreatedOn = existingRegistration.CreatedOn;
                                amendedRegistration.ModifiedBy = amendedRegistration.CreatedBy;
                                amendedRegistration.ModifiedOn = DateTime.UtcNow;
                            }

                            if (hasTqRegistrationProfileRecordChanged && hasBothPathwayAndSpecialismsRecordsChanged)
                            {
                                amendedRegistration.TqRegistrationPathways = pathwaysToAdd.Concat(pathwaysToUpdate).ToList();
                            }
                            else if (hasTqRegistrationProfileRecordChanged && !hasBothPathwayAndSpecialismsRecordsChanged && hasOnlySpecialismsRecordChanged)
                            {
                                // profile changed and specialisms modified/added
                                pathwaysToUpdate.ForEach(p => { amendedSpecialismRecords.AddRange(p.TqRegistrationSpecialisms); });
                                amendedRegistration.TqRegistrationPathways.Clear();
                            }
                            else if (hasTqRegistrationProfileRecordChanged && !hasBothPathwayAndSpecialismsRecordsChanged && !hasOnlySpecialismsRecordChanged)
                            {
                                amendedRegistration.TqRegistrationPathways.Clear();
                            }
                            else if (hasBothPathwayAndSpecialismsRecordsChanged && !hasOnlySpecialismsRecordChanged)
                            {
                                amendedPathwayRecords.AddRange(pathwaysToAdd.Concat(pathwaysToUpdate));
                                amendedRegistrationsToIgnore.Add(amendedRegistration);
                            }
                            else if (!hasBothPathwayAndSpecialismsRecordsChanged && hasOnlySpecialismsRecordChanged)
                            {
                                pathwaysToUpdate.ForEach(p => { amendedSpecialismRecords.AddRange(p.TqRegistrationSpecialisms); });
                                amendedRegistrationsToIgnore.Add(amendedRegistration);
                            }
                            else if (!hasTqRegistrationProfileRecordChanged && !hasBothPathwayAndSpecialismsRecordsChanged && !hasOnlySpecialismsRecordChanged)
                            {
                                amendedRegistrationsToIgnore.Add(amendedRegistration);
                            }
                        }
                    }
                });
            }

            if (response.IsValid)
            {
                var registrationProfilesToSendToDB = newRegistrations.Concat(amendedRegistrations.Except(amendedRegistrationsToIgnore, ulnComparer)).ToList();
                response.IsSuccess = await _tqRegistrationRepository.BulkInsertOrUpdateTqRegistrations(registrationProfilesToSendToDB, amendedPathwayRecords, amendedSpecialismRecords);
                response.BulkUploadStats = new BulkUploadStats
                {
                    TotalRecordsCount = registrationsToProcess.Count,
                    NewRecordsCount = newRegistrations.Count,
                    AmendedRecordsCount = amendedRegistrations.Count,
                    UnchangedRecordsCount = unchangedRegistrations.Count
                };
            }
            return response;
        }

        public async Task<bool> AddRegistrationAsync(RegistrationRequest model)
        {
            var validateStage3Response = await ValidateManualRegistrationTlevelsAsync(model);

            if (validateStage3Response.IsValid)
            {
                var transformedRegistration = TransformManualRegistrationModel(model, validateStage3Response);

                var existingRegistrationPathway = await _tqRegistrationPathwayRepository.GetManyAsync(p => p.TqRegistrationProfile.UniqueLearnerNumber == model.Uln, p => p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton).OrderByDescending(p => p.CreatedOn).FirstOrDefaultAsync();

                var isRegisteredWithAnotherAOAndWithdrawn = existingRegistrationPathway != null && existingRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn && existingRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn != model.AoUkprn;

                var hasRegistrationAlreadyExists = existingRegistrationPathway != null;

                if (isRegisteredWithAnotherAOAndWithdrawn)
                {
                    transformedRegistration.Id = existingRegistrationPathway.TqRegistrationProfileId;
                    transformedRegistration.CreatedBy = existingRegistrationPathway.CreatedBy;
                    transformedRegistration.CreatedOn = existingRegistrationPathway.CreatedOn;
                    transformedRegistration.ModifiedOn = DateTime.UtcNow;
                    transformedRegistration.ModifiedBy = model.PerformedBy;
                    transformedRegistration.ModifiedOn = DateTime.UtcNow;
                    return await _tqRegistrationRepository.UpdateAsync(transformedRegistration) > 0;
                }
                else if (hasRegistrationAlreadyExists)
                {
                    _logger.LogWarning(LogEvent.RecordExists, $"Registration already exists for UniqueLearnerNumber = {model.Uln}. Method: AddRegistrationAsync()");
                    return false;
                }
                else
                {
                    return await _tqRegistrationRepository.CreateAsync(transformedRegistration) > 0;
                }
            }
            else
            {
                var errorMessage = string.Join(",", validateStage3Response.ValidationErrors.Select(e => e.ErrorMessage));
                _logger.LogWarning(LogEvent.ManualRegistrationProcessFailed, $"Manual Registration failed to process due to validation errors = {errorMessage}. Method: AddRegistrationAsync()");
                return false;
            }
        }

        public async Task<FindUlnResponse> FindUlnAsync(long aoUkprn, long uln)
        {
            var result = await _tqRegistrationRepository.GetManyAsync(x => x.UniqueLearnerNumber == uln)
                .Select(x => new FindUlnResponse
                {
                    Uln = x.UniqueLearnerNumber,
                    RegistrationProfileId = x.Id,
                    Status = x.TqRegistrationPathways.Any(p => p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn)
                                                     ? x.TqRegistrationPathways
                                                        .Where(p => p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == aoUkprn)
                                                        .OrderByDescending(o => o.CreatedOn)
                                                        .FirstOrDefault(pw => pw.Status == RegistrationPathwayStatus.Active ||
                                                                              pw.Status == RegistrationPathwayStatus.Withdrawn)
                                                        .Status
                                                     : RegistrationPathwayStatus.NotSpecified,
                    IsRegisteredWithOtherAo = x.TqRegistrationPathways.Any(pw => pw.Status == RegistrationPathwayStatus.Active
                                                                           && pw.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn != aoUkprn),

                    RegisteredWithAoName = x.TqRegistrationPathways.FirstOrDefault(e => e.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn != aoUkprn)
                                                            .TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.DisplayName
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<RegistrationDetails> GetRegistrationDetailsAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null)
        {
            var tqRegistration = await _tqRegistrationRepository.GetRegistrationAsync(aoUkprn, profileId);

            if (tqRegistration == null || (status != null && tqRegistration.Status != status)) return null;

            var hasActiveAssessmentEntriesForSpecialisms = false;
            var hasActiveAssessmentResults = false;


            if (tqRegistration.TqPathwayAssessments.Any())
            {
                var pathwayIds = tqRegistration.TqPathwayAssessments.Select(p => p.Id);
                hasActiveAssessmentResults = await _tqPathwayAssessmentRepository
                                                .CountAsync(p => pathwayIds
                                                .Contains(p.Id) && p.TqPathwayResults
                                                .Any(pa => pa.IsOptedin && pa.EndDate == null && pa.TlLookupId != (int)PathwayComponentGradeLookup.XNoResult)) > 0;
            }

            if (tqRegistration.TqRegistrationSpecialisms.Any())
            {
                var specialismIds = tqRegistration.TqRegistrationSpecialisms.Select(s => s.Id);
                hasActiveAssessmentEntriesForSpecialisms = await _tqRegistrationSpecialismRepository
                                                                .CountAsync(s => specialismIds
                                                                .Contains(s.Id) && s.TqSpecialismAssessments
                                                                .Any(sa => sa.IsOptedin && sa.EndDate == null)) > 0;
            }

            var isRegisteredWithOtherAo = false;
            if (tqRegistration.Status == RegistrationPathwayStatus.Withdrawn)
                isRegisteredWithOtherAo = await IsActiveWithOtherAoAsync(aoUkprn, tqRegistration.TqRegistrationProfile.UniqueLearnerNumber, tqRegistration.CreatedOn);

            return _mapper.Map<RegistrationDetails>(tqRegistration, opt =>
            {
                opt.Items["IsActiveWithOtherAo"] = isRegisteredWithOtherAo;
                opt.Items["HasActiveAssessmentEntriesForSpecialisms"] = hasActiveAssessmentEntriesForSpecialisms;
                opt.Items["HasActiveAssessmentResults"] = hasActiveAssessmentResults;
            });
        }

        public async Task<bool> DeleteRegistrationAsync(long aoUkprn, int profileId)
        {
            var registrationProfile = await _tqRegistrationRepository.GetRegistrationDataWithHistoryAsync(aoUkprn, profileId);

            if (registrationProfile == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to delete registration as registration does not exists for ProfileId = {profileId}. Method: DeleteRegistrationByProfileId({aoUkprn}, {profileId})");
                return false;
            }

            var hasCoreResult = registrationProfile.TqRegistrationPathways
                                        .Any(p => p.TqPathwayAssessments.Any(a => a.TqPathwayResults.Any(r => r.IsOptedin && r.EndDate == null)));

            var hasSpecialismResult = registrationProfile.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms)
                                        .Any(s => s.TqSpecialismAssessments.Any(sa => sa.TqSpecialismResults.Any(sr => sr.IsOptedin && sr.EndDate == null)));

            if (hasCoreResult || hasSpecialismResult)
            {
                _logger.LogWarning(LogEvent.RegistrationNotDeleted, $"Unable to delete registration as registration has a result for either core or specialisms for ProfileId = {profileId}. Method: DeleteRegistrationByProfileId({aoUkprn}, {profileId})");
                return false;
            }

            var isIndustryPlacementExist = registrationProfile.TqRegistrationPathways.Any(p => p.IndustryPlacements.Any());
            if (isIndustryPlacementExist)
            {
                _logger.LogWarning(LogEvent.RegistrationNotDeleted, $"Unable to delete registration as registration has industry placement exist for ProfileId = {profileId}. Method: DeleteRegistrationByProfileId({aoUkprn}, {profileId})");
                return false;
            }

            return await _tqRegistrationRepository.DeleteAsync(registrationProfile) > 0;
        }

        public async Task<bool> UpdateRegistrationAsync(ManageRegistration model)
        {
            if (model == null || (!model.HasProfileChanged && !model.HasProviderChanged && !model.HasSpecialismsChanged))
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Model is null or no changes detected to update Registration for UniqueLearnerNumber = {model.Uln}. Method: UpdateRegistrationAsync()");
                return false;
            }

            var validateStage3Response = await ValidateManualRegistrationTlevelsAsync(model);

            if (validateStage3Response.IsValid)
            {
                if (model.HasProfileChanged)
                {
                    return await HandleProfileChanges(model);
                }
                else if (model.HasProviderChanged)
                {
                    return await HandleProviderChanges(model, validateStage3Response);
                }
                else if (model.HasSpecialismsChanged)
                {
                    return await HandleSpecialismChanges(model, validateStage3Response);
                }
                return false;
            }
            else
            {
                var errorMessage = string.Join(",", validateStage3Response.ValidationErrors.Select(e => e.ErrorMessage));
                _logger.LogWarning(LogEvent.ManualRegistrationProcessFailed, $"Manual Change Registration failed to process due to validation errors = {errorMessage}. Method: UpdateRegistrationAsync()");
                return false;
            }
        }

        public async Task<bool> WithdrawRegistrationAsync(WithdrawRegistrationRequest model)
        {
            var registration = await _tqRegistrationRepository.GetRegistrationLiteAsync(model.AoUkprn, model.ProfileId, false, includeOverallResults: true);

            if (registration == null || registration.Status != RegistrationPathwayStatus.Active)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found for ProfileId = {model.ProfileId}. Method: WithdrawRegistrationAsync({model.AoUkprn}, {model.ProfileId})");
                return false;
            }

            EndRegistrationWithStatus(registration, RegistrationPathwayStatus.Withdrawn, model.PerformedBy);

            return await _tqRegistrationPathwayRepository.UpdateAsync(registration) > 0;
        }

        public async Task<bool> RejoinRegistrationAsync(RejoinRegistrationRequest model)
        {
            var tqRegistrationPathway = await _tqRegistrationRepository.GetRegistrationLiteAsync(model.AoUkprn, model.ProfileId, includeProfile: true, includeIndustryPlacements: true, includeOverallResults: true, includePrintCertificates: true);

            if (tqRegistrationPathway == null || tqRegistrationPathway.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found for ProfileId = {model.ProfileId}. Method: RejoinRegistrationAsync({model.AoUkprn}, {model.ProfileId})");
                return false;
            }

            var isRegisteredWithOtherAo = await IsActiveWithOtherAoAsync(model.AoUkprn, tqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber, tqRegistrationPathway.CreatedOn);
            if (isRegisteredWithOtherAo)
            {
                _logger.LogWarning(LogEvent.ManualReregistrationProcessFailed, $"Manual Reregistration failed to process due to validation error = Uln: {tqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber} is already active with other Ao. Method: RejoinRegistrationAsync({model.AoUkprn}, {model.ProfileId})");
                return false;
            }

            var tqPathway = new TqRegistrationPathway
            {
                TqRegistrationProfileId = tqRegistrationPathway.TqRegistrationProfileId,
                TqProviderId = tqRegistrationPathway.TqProviderId,
                AcademicYear = tqRegistrationPathway.AcademicYear,
                StartDate = DateTime.UtcNow,
                Status = RegistrationPathwayStatus.Active,
                IsBulkUpload = false,
                TqRegistrationSpecialisms = MapInactiveSpecialismAssessmentsAndResults(tqRegistrationPathway, true, false, model.PerformedBy),
                TqPathwayAssessments = MapInactivePathwayAssessmentsAndResults(tqRegistrationPathway, true, false, model.PerformedBy),
                IndustryPlacements = MapIndustryPlacements(tqRegistrationPathway, model.PerformedBy),
                OverallResults = MapOverallResults(tqRegistrationPathway, model.PerformedBy),
                PrintCertificates = MapPrintCertificates(tqRegistrationPathway.PrintCertificates, model.PerformedBy),
                CreatedBy = model.PerformedBy,
                CreatedOn = DateTime.UtcNow
            };

            return await _tqRegistrationPathwayRepository.CreateAsync(tqPathway) > 0;
        }

        public async Task<bool> ReregistrationAsync(ReregistrationRequest model)
        {
            var tqRegistrationPathway = await _tqRegistrationPathwayRepository.GetFirstOrDefaultAsync(p => p.TqRegistrationProfile.Id == model.ProfileId
                                                                                        && p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == model.AoUkprn, p => p, p => p.CreatedOn, false,
                                                                                        p => p.TqProvider, p => p.TqProvider.TqAwardingOrganisation, p => p.TqProvider.TqAwardingOrganisation.TlPathway, p => p.TqRegistrationProfile);

            if (tqRegistrationPathway == null || tqRegistrationPathway.Status != RegistrationPathwayStatus.Withdrawn)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found for ProfileId = {model.ProfileId}. Method: ReregisterRegistrationAsync({model.AoUkprn}, {model.ProfileId})");
                return false;
            }

            var isRegisteredWithOtherAo = await IsActiveWithOtherAoAsync(model.AoUkprn, tqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber, tqRegistrationPathway.CreatedOn);
            if (isRegisteredWithOtherAo)
            {
                _logger.LogWarning(LogEvent.ManualReregistrationProcessFailed, $"Manual Reregistration failed to process due to validation error = Uln: {tqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber} is already active with other Ao. Method: ReregisterRegistrationAsync({model.AoUkprn}, {model.ProfileId})");
                return false;
            }

            var withdrawnCorecode = tqRegistrationPathway.TqProvider?.TqAwardingOrganisation?.TlPathway?.LarId;
            var isCoreSameAsWithdrawnCore = string.IsNullOrWhiteSpace(withdrawnCorecode) || withdrawnCorecode.Equals(model.CoreCode, StringComparison.InvariantCultureIgnoreCase);

            if (isCoreSameAsWithdrawnCore)
            {
                _logger.LogWarning(LogEvent.ManualReregistrationProcessFailed, $"Manual Reregistration failed to process due to validation error = Cannot reregister learner on same course that has withdrawn previously. Method: ReregistrationAsync()");
                return false;
            }

            var validateStage3Response = await ValidateManualRegistrationTlevelsAsync(model);

            if (validateStage3Response.IsValid)
            {
                var tqPathway = new TqRegistrationPathway
                {
                    TqRegistrationProfileId = tqRegistrationPathway.TqRegistrationProfileId,
                    TqProviderId = validateStage3Response.TqProviderId,
                    AcademicYear = validateStage3Response.AcademicYear,
                    StartDate = DateTime.UtcNow,
                    Status = RegistrationPathwayStatus.Active,
                    IsBulkUpload = false,
                    TqRegistrationSpecialisms = MapSpecialisms(validateStage3Response.TlSpecialismLarIds, model.PerformedBy, 0, false),
                    CreatedBy = model.PerformedBy,
                    CreatedOn = DateTime.UtcNow
                };

                return await _tqRegistrationPathwayRepository.CreateAsync(tqPathway) > 0;
            }
            else
            {
                var errorMessage = string.Join(",", validateStage3Response.ValidationErrors.Select(e => e.ErrorMessage));
                _logger.LogWarning(LogEvent.ManualReregistrationProcessFailed, $"Manual Reregistration failed to process due to validation errors = {errorMessage}. Method: ReregistrationAsync()");
                return false;
            }
        }

        public async Task<bool> SetRegistrationAsPendingWithdrawalAsync(SetRegistrationAsPendingWithdrawalRequest model)
        {
            var registration = await _tqRegistrationRepository.GetRegistrationLiteByProviderUkprnAsync(model.ProviderUkprn, model.ProfileId, includeProfile: false);

            if (registration == null || registration.Status != RegistrationPathwayStatus.Active || (registration.Status == RegistrationPathwayStatus.Active && registration.IsPendingWithdrawal))
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found for ProfileId = {model.ProfileId}. Method: {nameof(SetRegistrationAsPendingWithdrawalAsync)}({model.ProviderUkprn}, {model.ProfileId})");
                return false;
            }

            registration.IsPendingWithdrawal = true;
            return await _tqRegistrationPathwayRepository.UpdateWithSpecifedColumnsOnlyAsync(registration, r => r.IsPendingWithdrawal, r => r.ModifiedBy, r => r.ModifiedOn) > 0;
        }

        public async Task<bool> ReinstateRegistrationFromPendingWithdrawalAsync(ReinstateRegistrationFromPendingWithdrawalRequest model)
        {
            var registration = await _tqRegistrationRepository.GetRegistrationLiteByProviderUkprnAsync(model.ProviderUkprn, model.ProfileId, includeProfile: false);

            if (registration == null || registration.Status != RegistrationPathwayStatus.Active || (registration.Status == RegistrationPathwayStatus.Active && !registration.IsPendingWithdrawal))
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found for ProfileId = {model.ProfileId}. Method: {nameof(ReinstateRegistrationFromPendingWithdrawalAsync)}({model.ProviderUkprn}, {model.ProfileId})");
                return false;
            }

            registration.IsPendingWithdrawal = false;
            return await _tqRegistrationPathwayRepository.UpdateWithSpecifedColumnsOnlyAsync(registration, r => r.IsPendingWithdrawal, r => r.ModifiedBy, r => r.ModifiedOn) > 0;
        }

        public async Task<bool> ProcessChangeAcademicYearAsync(ChangeAcademicYearRequest model)
        {
            var tqRegistrationProfile = await _tqRegistrationRepository.GetRegistrationLiteAsync(model.AoUkprn, model.ProfileId);

            var pathway = await _tqRegistrationPathwayRepository.GetFirstOrDefaultAsync(p => p.Id == tqRegistrationProfile.Id && p.Status == RegistrationPathwayStatus.Active);
            if (pathway == null) return false;

            pathway.EndDate = _systemProvider.UtcNow;
            pathway.Status = RegistrationPathwayStatus.Withdrawn;
            pathway.ModifiedBy = model.PerformedBy;
            pathway.ModifiedOn = _systemProvider.UtcNow;

            var isRegistrationWithdrawn = await _tqRegistrationPathwayRepository.UpdateWithSpecifedColumnsOnlyAsync(pathway, u => u.Status, u => u.ModifiedBy, u => u.ModifiedOn) > 0;

            if (!isRegistrationWithdrawn) return false;

            return await _tqRegistrationPathwayRepository.CreateAsync(new TqRegistrationPathway
            {
                TqRegistrationProfileId = model.ProfileId,
                TqProviderId = pathway.TqProviderId,
                AcademicYear = int.Parse(model.AcademicYearChangeTo),
                StartDate = _systemProvider.UtcNow,
                Status = RegistrationPathwayStatus.Active,
                CreatedBy = model.PerformedBy,
                CreatedOn = _systemProvider.UtcNow
            }) > 0;
        }

        #region Private Methods

        private async Task<bool> HandleProfileChanges(ManageRegistration model)
        {
            var profile = await _tqRegistrationPathwayRepository.GetFirstOrDefaultAsync(p => p.TqRegistrationProfile.Id == model.ProfileId
                                                                                        && p.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == model.AoUkprn
                                                                                        && p.Status == RegistrationPathwayStatus.Active, p => p.TqRegistrationProfile, p => p.CreatedOn, false);
            if (profile == null)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found to update Registration for UniqueLearnerNumber = {model.Uln}. Method: UpdateRegistrationAsync()");
                return false;
            }

            _mapper.Map(model, profile);
            return await _tqRegistrationRepository.UpdateWithSpecifedColumnsOnlyAsync(profile, u => u.Firstname, u => u.Lastname, u => u.DateofBirth) > 0;
        }

        private async Task<bool> HandleProviderChanges(ManageRegistration model, RegistrationRecordResponse registrationRecord)
        {
            var pathway = await _tqRegistrationRepository.GetRegistrationLiteAsync(model.AoUkprn, model.ProfileId, includeProfile: false, includeIndustryPlacements: true, includeOverallResults: true);

            if (pathway == null || pathway.Status != RegistrationPathwayStatus.Active)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found to update Registration for UniqueLearnerNumber = {model.Uln}. Method: HandleProviderChanges()");
                return false;
            }

            var pathways = new List<TqRegistrationPathway>();

            EndRegistrationWithStatus(pathway, RegistrationPathwayStatus.Transferred, model.PerformedBy);

            pathways.Add(pathway);

            // add new records
            pathways.Add(
                new TqRegistrationPathway
                {
                    TqRegistrationProfileId = model.ProfileId,
                    TqProviderId = registrationRecord.TqProviderId,
                    AcademicYear = registrationRecord.AcademicYear,
                    StartDate = DateTime.UtcNow,
                    Status = RegistrationPathwayStatus.Active,
                    IsBulkUpload = false,
                    TqRegistrationSpecialisms = MapInactiveSpecialismAssessmentsAndResults(pathway, true, false, model.PerformedBy),
                    TqPathwayAssessments = MapInactivePathwayAssessmentsAndResults(pathway, true, false, model.PerformedBy),
                    IndustryPlacements = MapIndustryPlacements(pathway, model.PerformedBy),
                    OverallResults = MapOverallResults(pathway, model.PerformedBy),
                    CreatedBy = model.PerformedBy,
                    CreatedOn = DateTime.UtcNow
                });

            return await _tqRegistrationPathwayRepository.UpdateManyAsync(pathways) > 0;
        }

        private async Task<bool> HandleSpecialismChanges(ManageRegistration model, RegistrationRecordResponse registrationRecord)
        {
            var existingPathway = await _tqRegistrationRepository.GetRegistrationLiteAsync(model.AoUkprn, model.ProfileId, false, false);

            if (existingPathway == null || existingPathway.Status != RegistrationPathwayStatus.Active)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"No record found to update Registration for UniqueLearnerNumber = {model.Uln}. Method: HandleSpecialismChanges()");
                return false;
            }

            var hasActiveSpecialismAssessmentEntries = existingPathway.TqRegistrationSpecialisms
                                                       .Where(s => s.IsOptedin && s.EndDate == null)
                                                       .Any(s => s.TqSpecialismAssessments
                                                       .Any(sa => sa.IsOptedin && sa.EndDate == null));

            if (hasActiveSpecialismAssessmentEntries)
            {
                _logger.LogWarning(LogEvent.NoDataFound, $"Unable to change specialisms as there are active assessment entries registered for specialism. UniqueLearnerNumber = {model.Uln}. Method: HandleSpecialismChanges()");
                return false;
            }

            var specialismsToUpdateList = new List<TqRegistrationSpecialism>();

            var existingSpecialismsInDb = _tqRegistrationSpecialismRepository.GetManyAsync(s => s.TqRegistrationPathwayId == existingPathway.Id && s.IsOptedin && s.EndDate == null).ToList();
            var filteredTlSpecialismLarIdsToAdd = registrationRecord.TlSpecialismLarIds.Where(s => !existingSpecialismsInDb.Any(r => r.TlSpecialismId == s.Key)).ToList();
            var specialismsToUpdate = existingSpecialismsInDb.Where(s => !registrationRecord.TlSpecialismLarIds.Any(x => x.Key == s.TlSpecialismId)).ToList();

            specialismsToUpdate.ForEach(s =>
            {
                s.IsOptedin = false;
                s.EndDate = DateTime.UtcNow;
                s.ModifiedBy = model.PerformedBy;
                s.ModifiedOn = DateTime.UtcNow;
            });

            if (filteredTlSpecialismLarIdsToAdd.Count > 0 || specialismsToUpdate.Count > 0)
            {
                registrationRecord.TlSpecialismLarIds = filteredTlSpecialismLarIdsToAdd;
                var mappedSpecialisms = MapSpecialisms(registrationRecord.TlSpecialismLarIds, model.PerformedBy, 0, false);
                mappedSpecialisms.ToList().ForEach(s => s.TqRegistrationPathwayId = existingPathway.Id);
                specialismsToUpdateList = mappedSpecialisms.Concat(specialismsToUpdate).ToList();
            }

            return specialismsToUpdateList.Any() && await _tqRegistrationSpecialismRepository.UpdateManyAsync(specialismsToUpdateList) > 0;
        }

        private static void EndRegistrationWithStatus(TqRegistrationPathway pathway, RegistrationPathwayStatus status, string performedBy)
        {
            if (pathway != null)
            {
                // Pathway
                pathway.Status = status;
                pathway.IsPendingWithdrawal = false;
                pathway.EndDate = DateTime.UtcNow;
                pathway.ModifiedBy = performedBy;
                pathway.ModifiedOn = DateTime.UtcNow;

                pathway.TqPathwayAssessments.Where(s => s.IsOptedin && s.EndDate == null).ToList().ForEach(pa =>
                {
                    pa.EndDate = DateTime.UtcNow;
                    pa.ModifiedBy = performedBy;
                    pa.ModifiedOn = DateTime.UtcNow;

                    pa.TqPathwayResults.Where(r => r.IsOptedin && r.EndDate == null).ToList().ForEach(pr =>
                    {
                        pr.EndDate = DateTime.UtcNow;
                        pr.ModifiedBy = performedBy;
                        pr.ModifiedOn = DateTime.UtcNow;
                    });
                });

                // Specialisms
                pathway.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null).ToList().ForEach(s =>
                {
                    s.EndDate = DateTime.UtcNow;
                    s.ModifiedBy = performedBy;
                    s.ModifiedOn = DateTime.UtcNow;

                    s.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.EndDate == null).ToList().ForEach(sa =>
                    {
                        sa.EndDate = DateTime.UtcNow;
                        sa.ModifiedBy = performedBy;
                        sa.ModifiedOn = DateTime.UtcNow;

                        sa.TqSpecialismResults.Where(sr => sr.IsOptedin && sr.EndDate == null).ToList().ForEach(sr =>
                        {
                            sr.EndDate = DateTime.UtcNow;
                            sr.ModifiedBy = performedBy;
                            sr.ModifiedOn = DateTime.UtcNow;
                        });
                    });
                });

                // Overall Results
                var overallResult = pathway.OverallResults.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);
                if (overallResult != null)
                {
                    overallResult.EndDate = DateTime.UtcNow;
                    overallResult.ModifiedBy = performedBy;
                    overallResult.ModifiedOn = DateTime.UtcNow;
                }
            }
        }

        private TqRegistrationProfile TransformManualRegistrationModel(RegistrationRequest model, RegistrationRecordResponse registrationRecord)
        {
            var toAddRegistration = new TqRegistrationProfile
            {
                UniqueLearnerNumber = registrationRecord.Uln,
                Firstname = registrationRecord.FirstName,
                Lastname = registrationRecord.LastName,
                DateofBirth = registrationRecord.DateOfBirth,
                CreatedBy = model.PerformedBy,
                CreatedOn = DateTime.UtcNow,
                TqRegistrationPathways = new List<TqRegistrationPathway>
                {
                    new TqRegistrationPathway
                    {
                        TqProviderId = registrationRecord.TqProviderId,
                        AcademicYear = registrationRecord.AcademicYear,
                        StartDate = DateTime.UtcNow,
                        Status = RegistrationPathwayStatus.Active,
                        IsBulkUpload = false,
                        TqRegistrationSpecialisms = MapSpecialisms(registrationRecord.TlSpecialismLarIds, model.PerformedBy, 0, false),
                        CreatedBy = model.PerformedBy,
                        CreatedOn = DateTime.UtcNow
                    }
                }
            };
            return toAddRegistration;
        }

        private RegistrationRecordResponse AddStage3ValidationError(int rowNum, long uln, string errorMessage)
        {
            return new RegistrationRecordResponse()
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

        private async Task<IList<TechnicalQualificationDetails>> GetAllTLevelsByAoUkprnAsync(long ukprn)
        {
            var result = await _tqProviderRepository.GetManyAsync(p => p.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == ukprn
                                                                    && p.TqAwardingOrganisation.TlAwardingOrganisaton.IsActive
                                                                    && p.TqAwardingOrganisation.TlPathway.IsActive,
               p => p.TlProvider, p => p.TqAwardingOrganisation, p => p.TqAwardingOrganisation.TlAwardingOrganisaton,
               p => p.TqAwardingOrganisation.TlPathway, p => p.TqAwardingOrganisation.TlPathway.TlPathwaySpecialismCombinations.Where(p => p.IsActive),
               p => p.TqAwardingOrganisation.TlPathway.TlSpecialisms.Where(p => p.IsActive)).ToListAsync();

            return _mapper.Map<IList<TechnicalQualificationDetails>>(result);
        }

        private IList<TqRegistrationSpecialism> MapSpecialisms(IEnumerable<KeyValuePair<int, string>> specialismsList, string performedBy, int specialismStartIndex, bool isBulkUpload = true)
        {
            return specialismsList.Select((x, index) => new TqRegistrationSpecialism
            {
                Id = isBulkUpload ? index - specialismStartIndex : 0,
                TlSpecialismId = x.Key,
                StartDate = DateTime.UtcNow,
                IsOptedin = true,
                IsBulkUpload = isBulkUpload,
                CreatedBy = performedBy,
                CreatedOn = DateTime.UtcNow,
            }).ToList();
        }

        private static List<TqPathwayAssessment> MapInactivePathwayAssessmentsAndResults(TqRegistrationPathway tqRegistrationPathway, bool isOptedIn, bool isBulkUpload, string performedBy)
        {
            return tqRegistrationPathway.TqPathwayAssessments.Where(a => a.IsOptedin && a.EndDate != null).Select(x => new TqPathwayAssessment
            {
                AssessmentSeriesId = x.AssessmentSeriesId,
                StartDate = DateTime.UtcNow,
                IsOptedin = isOptedIn,
                IsBulkUpload = isBulkUpload,
                CreatedBy = performedBy,
                TqPathwayResults = x.TqPathwayResults.Where(pr => pr.IsOptedin && pr.EndDate != null).Select(r => new TqPathwayResult
                {
                    TlLookupId = r.TlLookupId,
                    StartDate = DateTime.UtcNow,
                    PrsStatus = r.PrsStatus,
                    IsOptedin = isOptedIn,
                    IsBulkUpload = isBulkUpload,
                    CreatedBy = performedBy,
                }).ToList()
            }).ToList();
        }

        private static List<TqRegistrationSpecialism> MapInactiveSpecialismAssessmentsAndResults(TqRegistrationPathway tqRegistrationPathway, bool isOptedIn, bool isBulkUpload, string performedBy)
        {
            DateTime utcNow = DateTime.UtcNow;

            return tqRegistrationPathway.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate != null).Select(x => new TqRegistrationSpecialism
            {
                TlSpecialismId = x.TlSpecialismId,
                StartDate = utcNow,
                IsOptedin = isOptedIn,
                IsBulkUpload = isBulkUpload,
                CreatedBy = performedBy,
                CreatedOn = utcNow,
                TqSpecialismAssessments = x.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.EndDate != null).Select(sa => new TqSpecialismAssessment
                {
                    AssessmentSeriesId = sa.AssessmentSeriesId,
                    StartDate = utcNow,
                    IsOptedin = isOptedIn,
                    IsBulkUpload = isBulkUpload,
                    CreatedBy = performedBy,
                    TqSpecialismResults = sa.TqSpecialismResults.Where(sr => sr.IsOptedin && sr.EndDate != null).Select(sr => new TqSpecialismResult
                    {
                        TlLookupId = sr.TlLookupId,
                        StartDate = utcNow,
                        PrsStatus = sr.PrsStatus,
                        IsOptedin = isOptedIn,
                        IsBulkUpload = isBulkUpload,
                        CreatedBy = performedBy,
                    }).ToList()
                }).ToList()
            }).ToList();
        }

        private IList<IndustryPlacement> MapIndustryPlacements(TqRegistrationPathway tqRegistrationPathway, string performedBy)
        {
            return tqRegistrationPathway.IndustryPlacements.Select(x => new IndustryPlacement
            {
                Status = x.Status,
                Details = x.Details,
                CreatedBy = performedBy
            }).ToList();
        }

        private IList<OverallResult> MapOverallResults(TqRegistrationPathway tqRegistrationPathway, string performedBy)
        {
            return tqRegistrationPathway.OverallResults.Select(x => new OverallResult
            {
                Details = x.Details,
                ResultAwarded = x.ResultAwarded,
                SpecialismResultAwarded = x.SpecialismResultAwarded,
                PublishDate = x.PublishDate,
                PrintAvailableFrom = x.PrintAvailableFrom,
                CalculationStatus = x.CalculationStatus,
                CertificateType = x.CertificateType,
                CertificateStatus = x.CertificateStatus,
                StartDate = x.StartDate,
                IsOptedin = true,
                CreatedBy = performedBy
            }).ToList();
        }

        private static IList<PrintCertificate> MapPrintCertificates(ICollection<PrintCertificate> printCertificates, string performedBy)
            => printCertificates.Select(p => new PrintCertificate
            {
                PrintBatchItemId = p.PrintBatchItemId,
                CertificateNumber = p.CertificateNumber,
                Uln = p.Uln,
                LearnerName = p.LearnerName,
                Type = p.Type,
                LearningDetails = p.LearningDetails,
                DisplaySnapshot = p.DisplaySnapshot,
                IsReprint = p.IsReprint,
                CreatedOn = p.CreatedOn,
                CreatedBy = p.CreatedBy,
                LastRequestedOn = null
            }).ToList();

        private IEnumerable<TqRegistrationPathway> GetActivePathwayAndSpecialism(TqRegistrationProfile existingRegistration)
        {
            return existingRegistration.TqRegistrationPathways.Where(p => p.Status == RegistrationPathwayStatus.Active)
                                    .Select(x =>
                                    {
                                        x.TqRegistrationSpecialisms = x.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null).ToList();
                                        x.TqPathwayAssessments = x.TqPathwayAssessments.Where(a => a.IsOptedin && a.EndDate == null).ToList();
                                        return x;
                                    });
        }

        private RegistrationProcessResponse ValidateStage4Rules(TqRegistrationProfile amendedRegistration, List<TqRegistrationPathway> pathwaysToUpdate, RegistrationProcessResponse response)
        {
            var hasPathwayChanged = !pathwaysToUpdate.Any(x => amendedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TqAwardingOrganisation.TlPathwayId == x.TqProvider.TqAwardingOrganisation.TlPathwayId));

            // check if there is an active registration for another AO, if so show error message and reject the file
            var hasAoChanged = !pathwaysToUpdate.Any(x => amendedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId == x.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId));

            if (hasAoChanged || hasPathwayChanged)
            {
                response.ValidationErrors.Add(GetRegistrationValidationError(amendedRegistration.UniqueLearnerNumber, hasAoChanged ? ValidationMessages.ActiveUlnWithDifferentAo : ValidationMessages.CoreForUlnCannotBeChangedYet));
            }

            // check if specialism has any assessments registered

            var existingSpecialismsInDb = pathwaysToUpdate.SelectMany(p => p.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null));
            var incomingSpecialisms = amendedRegistration.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList();

            foreach (var existingSpecialism in existingSpecialismsInDb)
            {
                var isRequestToRemoveSpecialism = !incomingSpecialisms.Any(s => s.TlSpecialismId == existingSpecialism.TlSpecialismId);
                var hasActiveAssessmentEntriesForExistingSpecialism = existingSpecialism.TqSpecialismAssessments.Any(a => a.IsOptedin && a.EndDate == null);

                if (isRequestToRemoveSpecialism && hasActiveAssessmentEntriesForExistingSpecialism)
                {
                    response.ValidationErrors.Add(GetRegistrationValidationError(amendedRegistration.UniqueLearnerNumber, ValidationMessages.SpecialismCannotBeRemovedWhenActiveAssessmentEntryExist));
                    break;
                }
            }
            return response;
        }

        private Tuple<bool, bool, BulkRegistrationEntityIndex> PrepareAndAmendRegistrationData(
            TqRegistrationProfile amendedRegistration,
            List<TqRegistrationPathway> pathwaysToAdd,
            List<TqRegistrationPathway> pathwaysToUpdate,
            BulkRegistrationEntityIndex entityIndex)
        {
            var hasBothPathwayAndSpecialismsRecordsChanged = false;
            var hasOnlySpecialismsRecordChanged = false;
            DateTime now = DateTime.UtcNow;

            var hasProviderChanged = !pathwaysToUpdate.Any(x => amendedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TlProviderId == x.TqProvider.TlProviderId));

            if (hasProviderChanged)
            {
                // Transferred - Change existing TqRegistrationPathway record status and related TqRegistrationSpecialism records enddate
                pathwaysToUpdate.ForEach(pathwayToUpdate =>
                {
                    var associatedPathwayToAdd = pathwaysToAdd.FirstOrDefault(x => x.TqRegistrationProfileId == pathwayToUpdate.TqRegistrationProfileId && x.AcademicYear == pathwayToUpdate.AcademicYear);

                    if (associatedPathwayToAdd == null)
                        throw new ApplicationException("AssociatedPathwayToAdd cannot be null");

                    pathwayToUpdate.Status = RegistrationPathwayStatus.Transferred;
                    pathwayToUpdate.EndDate = now;
                    pathwayToUpdate.ModifiedBy = amendedRegistration.CreatedBy;
                    pathwayToUpdate.ModifiedOn = now;

                    // Transfer - Specialisms
                    var specialismsToUpdate = pathwayToUpdate.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null).ToList();
                    foreach (var (specialismToUpdate, idx) in specialismsToUpdate.Select((value, i) => (value, i)))
                    {
                        var associatedSpecialismsToAdd = associatedPathwayToAdd.TqRegistrationSpecialisms.FirstOrDefault(s => s.TlSpecialismId == specialismToUpdate.TlSpecialismId);
                        if (associatedSpecialismsToAdd == null)
                            throw new ApplicationException("AssociatedSpecialisms cannot be null");

                        // Update existing Specialism record
                        specialismToUpdate.IsOptedin = false;
                        specialismToUpdate.EndDate = now;
                        specialismToUpdate.ModifiedBy = amendedRegistration.CreatedBy;
                        specialismToUpdate.ModifiedOn = now;

                        var specialismAssessmentsToUpdate = specialismToUpdate.TqSpecialismAssessments.Where(s => s.IsOptedin && s.EndDate == null).ToList();
                        foreach (var (splAssessment, index) in specialismAssessmentsToUpdate.Select((value, i) => (value, i)))
                        {
                            // Update existing Specialism Assessment record 
                            splAssessment.IsOptedin = false;
                            splAssessment.EndDate = now;
                            splAssessment.ModifiedBy = amendedRegistration.CreatedBy;
                            splAssessment.ModifiedOn = now;

                            // Add new Specialism Assessment record 
                            var newActiveSpecialismsAssessment = new TqSpecialismAssessment
                            {
                                Id = index - entityIndex.SpecialismAssessmentStartIndex,
                                AssessmentSeriesId = splAssessment.AssessmentSeriesId,
                                IsOptedin = true,
                                StartDate = now,
                                IsBulkUpload = true,
                                CreatedOn = now,
                                CreatedBy = amendedRegistration.CreatedBy
                            };

                            // Transfer - Specialism Results
                            var specialismResultsToUpdate = splAssessment.TqSpecialismResults.Where(s => s.IsOptedin && s.EndDate == null).ToList();
                            foreach (var (specialismResult, spIndex) in specialismResultsToUpdate.Select((value, i) => (value, i)))
                            {
                                specialismResult.IsOptedin = false;
                                specialismResult.EndDate = now;
                                specialismResult.ModifiedBy = amendedRegistration.CreatedBy;
                                specialismResult.ModifiedOn = now;

                                var newActiveSpecialismResult = new TqSpecialismResult
                                {
                                    Id = spIndex - entityIndex.SpecialismResultStartIndex,
                                    TlLookupId = specialismResult.TlLookupId,
                                    PrsStatus = specialismResult.PrsStatus,
                                    IsOptedin = true,
                                    StartDate = now,
                                    IsBulkUpload = true,
                                    CreatedOn = now,
                                    CreatedBy = amendedRegistration.CreatedBy
                                };
                                newActiveSpecialismsAssessment.TqSpecialismResults.Add(newActiveSpecialismResult);
                            };
                            entityIndex.SpecialismResultStartIndex -= specialismResultsToUpdate.Count();

                            associatedSpecialismsToAdd.TqSpecialismAssessments.Add(newActiveSpecialismsAssessment);
                        }
                        entityIndex.SpecialismAssessmentStartIndex -= specialismAssessmentsToUpdate.Count();
                    }

                    // Transfer - Industry Placements
                    foreach (var (industryPlacement, idx) in pathwayToUpdate.IndustryPlacements.Select((value, i) => (value, i)))
                    {
                        associatedPathwayToAdd.IndustryPlacements.Add(new IndustryPlacement
                        {
                            Id = idx - entityIndex.IpStartIndex,
                            Status = industryPlacement.Status,
                            Details = industryPlacement.Details,
                            CreatedOn = now,
                            CreatedBy = amendedRegistration.CreatedBy
                        });
                    }
                    entityIndex.IpStartIndex -= pathwayToUpdate.IndustryPlacements.Count();

                    // Transfer - Pathway Assessments
                    var pathwayAssessmentsToUpdate = pathwayToUpdate.TqPathwayAssessments.Where(s => s.IsOptedin && s.EndDate == null).ToList();
                    foreach (var (pathwayAssessment, idx) in pathwayAssessmentsToUpdate.Select((value, i) => (value, i)))
                    {
                        pathwayAssessment.IsOptedin = false;
                        pathwayAssessment.EndDate = now;
                        pathwayAssessment.ModifiedBy = amendedRegistration.CreatedBy;
                        pathwayAssessment.ModifiedOn = now;

                        var newActiveAssessment = new TqPathwayAssessment
                        {
                            Id = idx - entityIndex.PathwayAssessmentStartIndex,
                            AssessmentSeriesId = pathwayAssessment.AssessmentSeriesId,
                            IsOptedin = true,
                            StartDate = now,
                            IsBulkUpload = true,
                            CreatedOn = now,
                            CreatedBy = amendedRegistration.CreatedBy
                        };

                        // Transfer - Results
                        var pathwayResultsToUpdate = pathwayAssessment.TqPathwayResults.Where(s => s.IsOptedin && s.EndDate == null).ToList();
                        foreach (var (pathwayResult, index) in pathwayResultsToUpdate.Select((value, i) => (value, i)))
                        {
                            pathwayResult.IsOptedin = false;
                            pathwayResult.EndDate = now;
                            pathwayResult.ModifiedBy = amendedRegistration.CreatedBy;
                            pathwayResult.ModifiedOn = now;

                            var newActiveResult = new TqPathwayResult
                            {
                                Id = index - entityIndex.PathwayResultStartIndex,
                                TlLookupId = pathwayResult.TlLookupId,
                                PrsStatus = pathwayResult.PrsStatus,
                                IsOptedin = true,
                                StartDate = now,
                                IsBulkUpload = true,
                                CreatedOn = now,
                                CreatedBy = amendedRegistration.CreatedBy
                            };
                            newActiveAssessment.TqPathwayResults.Add(newActiveResult);
                        };
                        entityIndex.PathwayResultStartIndex -= pathwayResultsToUpdate.Count();

                        associatedPathwayToAdd.TqPathwayAssessments.Add(newActiveAssessment);
                    };
                    entityIndex.PathwayAssessmentStartIndex -= pathwayAssessmentsToUpdate.Count();

                    // Transfer - OverallResult
                    var overallResultsToUpdate = pathwayToUpdate.OverallResults.Where(x => x.IsOptedin && x.EndDate == null);
                    foreach (var (overallResult, idx) in overallResultsToUpdate.Select((value, i) => (value, i)))
                    {
                        overallResult.IsOptedin = false;
                        overallResult.EndDate = now;
                        overallResult.ModifiedBy = amendedRegistration.CreatedBy;
                        overallResult.ModifiedOn = now;

                        var newOverallResult = new OverallResult
                        {
                            Id = idx - entityIndex.OverallResultStartIndex,
                            CalculationStatus = overallResult.CalculationStatus,
                            Details = overallResult.Details,
                            ResultAwarded = overallResult.ResultAwarded,
                            PublishDate = overallResult.PublishDate,
                            PrintAvailableFrom = overallResult.PrintAvailableFrom,
                            StartDate = now,
                            IsOptedin = true,
                            CertificateType = overallResult.CertificateType,
                            CertificateStatus = overallResult.CertificateStatus,
                            CreatedOn = now,
                            CreatedBy = amendedRegistration.CreatedBy
                        };
                        associatedPathwayToAdd.OverallResults.Add(newOverallResult);

                    }
                    entityIndex.OverallResultStartIndex -= overallResultsToUpdate.Count();
                });
                hasBothPathwayAndSpecialismsRecordsChanged = true;
            }
            else
            {
                foreach (var importPathwayRecord in amendedRegistration.TqRegistrationPathways)
                {
                    var existingPathwayRecordInDb = pathwaysToUpdate.FirstOrDefault(p => p.TqProviderId == importPathwayRecord.TqProviderId);

                    if (existingPathwayRecordInDb == null) continue;

                    if (existingPathwayRecordInDb.TqRegistrationSpecialisms.Any())
                    {
                        var existingSpecialismsInDb = existingPathwayRecordInDb.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null);
                        var specialismsToAdd = importPathwayRecord.TqRegistrationSpecialisms.Where(s => !existingSpecialismsInDb.Any(r => r.TlSpecialismId == s.TlSpecialismId)).ToList();
                        var specialismsToUpdate = existingSpecialismsInDb.Where(s => !importPathwayRecord.TqRegistrationSpecialisms.Any(r => r.TlSpecialismId == s.TlSpecialismId)).ToList();

                        specialismsToUpdate.ForEach(s =>
                        {
                            s.IsOptedin = false;
                            s.EndDate = now;
                            s.ModifiedBy = amendedRegistration.CreatedBy;
                            s.ModifiedOn = now;
                        });

                        specialismsToAdd.ForEach(s =>
                        {
                            s.TqRegistrationPathwayId = existingPathwayRecordInDb.Id;
                        });

                        if (specialismsToAdd.Count > 0 || specialismsToUpdate.Count > 0)
                        {
                            hasOnlySpecialismsRecordChanged = true;
                            existingPathwayRecordInDb.TqRegistrationSpecialisms.Clear();
                            existingPathwayRecordInDb.TqRegistrationSpecialisms = specialismsToAdd.Concat(specialismsToUpdate).ToList();
                        }
                    }
                    else if (importPathwayRecord.TqRegistrationSpecialisms.Any())
                    {
                        importPathwayRecord.TqRegistrationSpecialisms.ToList().ForEach(specialism =>
                        {
                            specialism.TqRegistrationPathwayId = existingPathwayRecordInDb.Id;
                            existingPathwayRecordInDb.TqRegistrationSpecialisms.Add(specialism);
                        });
                        hasOnlySpecialismsRecordChanged = true;
                    }
                }
            }
            return new Tuple<bool, bool, BulkRegistrationEntityIndex>(hasBothPathwayAndSpecialismsRecordsChanged, hasOnlySpecialismsRecordChanged, entityIndex);
        }

        private BulkProcessValidationError GetRegistrationValidationError(long uln, string errorMessage)
        {
            return new BulkProcessValidationError
            {
                Uln = uln.ToString(),
                ErrorMessage = errorMessage
            };
        }

        private bool IsSpecialismPartOfCouplet(IEnumerable<KeyValuePair<int, string>> coupletPairs, IEnumerable<string> specialismCodesToRegister)
        {
            if (!coupletPairs.Any() || !specialismCodesToRegister.Any()) return false;

            var coupletSpecialismCodes = new List<string>();

            coupletPairs.Select(x => x.Value).ToList().ForEach(c => { coupletSpecialismCodes.AddRange(c.Split(Constants.PipeSeperator)); });

            var result = specialismCodesToRegister.Any(s => coupletSpecialismCodes.Any(c => c.Equals(s, StringComparison.InvariantCultureIgnoreCase)));
            return result;
        }

        private bool IsValidCouplet(IEnumerable<KeyValuePair<int, string>> coupletPairs, IEnumerable<string> specialismCodesToRegister)
        {
            if (!coupletPairs.Any() || !specialismCodesToRegister.Any()) return false;

            var coupletSpecialismCodes = coupletPairs.Select(x => x.Value).ToList();

            var hasValidSpecialismCodes = coupletSpecialismCodes.Any(cs => specialismCodesToRegister.Except(cs.Split(Constants.PipeSeperator), StringComparer.InvariantCultureIgnoreCase).Count() == 0);
            var hasValidCoupletSpecialismCodes = coupletSpecialismCodes.Any(cs => cs.Split(Constants.PipeSeperator).Except(specialismCodesToRegister, StringComparer.InvariantCultureIgnoreCase).Count() == 0);
            return hasValidSpecialismCodes && hasValidCoupletSpecialismCodes;
        }

        private async Task<bool> IsActiveWithOtherAoAsync(long aoUkprn, long uln, DateTime currentAoRegistrationDate)
        {
            var isWithOtherAo = await _tqRegistrationPathwayRepository
                    .GetFirstOrDefaultAsync(OtherAo => OtherAo.TqRegistrationProfile.UniqueLearnerNumber == uln &&
                    OtherAo.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn != aoUkprn &&
                    OtherAo.Status == RegistrationPathwayStatus.Active &&
                    OtherAo.CreatedOn >= currentAoRegistrationDate) != null;

            return isWithOtherAo;
        }

        #endregion
    }
}
