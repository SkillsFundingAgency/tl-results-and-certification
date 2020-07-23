using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Comparer;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
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
        private readonly IMapper _mapper;

        public RegistrationService(IProviderRepository providerRespository, IRegistrationRepository tqRegistrationRepository, IMapper mapper)
        {
            _tqProviderRepository = providerRespository;
            _tqRegistrationRepository = tqRegistrationRepository;
            _mapper = mapper;
        }

        public async Task<IList<RegistrationRecordResponse>> ValidateRegistrationTlevelsAsync(long aoUkprn, IEnumerable<RegistrationCsvRecordResponse> validRegistrationsData)
        {
            var response = new List<RegistrationRecordResponse>();
            var aoProviderTlevels = await GetAllTLevelsByAoUkprnAsync(aoUkprn);

            foreach (var registrationData in validRegistrationsData)
            {
                var isProviderRegisteredWithAwardingOrganisation = aoProviderTlevels.Any(t => t.ProviderUkprn == registrationData.ProviderUkprn);
                if (!isProviderRegisteredWithAwardingOrganisation)
                {
                    response.Add(AddStage3ValidationError(registrationData.RowNum, registrationData.Uln,  ValidationMessages.ProviderNotRegisteredWithAo));
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
                }

                response.Add(new RegistrationRecordResponse
                {
                    Uln = registrationData.Uln,
                    FirstName = registrationData.FirstName,
                    LastName = registrationData.LastName,
                    DateOfBirth = registrationData.DateOfBirth,
                    RegistrationDate = registrationData.RegistrationDate,
                    TqProviderId = technicalQualification.TqProviderId,
                    TqAwardingOrganisationId = technicalQualification.TqAwardingOrganisationId,
                    TlPathwayId = technicalQualification.TlPathwayId,
                    TlSpecialismLarIds = technicalQualification.TlSpecialismLarIds.Where(s => registrationData.SpecialismCodes.Contains(s.Value)),
                    TlAwardingOrganisatonId = technicalQualification.TlAwardingOrganisatonId,
                    TlProviderId = technicalQualification.TlProviderId
                });
            };

            return response;
        }

        public async Task<RegistrationRecordResponse> ValidateManualRegistrationTlevelsAsync(RegistrationRequest registrationData)
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
            }

            return new RegistrationRecordResponse
            {
                Uln = registrationData.Uln,
                FirstName = registrationData.FirstName,
                LastName = registrationData.LastName,
                DateOfBirth = registrationData.DateOfBirth,
                RegistrationDate = registrationData.RegistrationDate,
                TqProviderId = technicalQualification.TqProviderId,
                TqAwardingOrganisationId = technicalQualification.TqAwardingOrganisationId,
                TlPathwayId = technicalQualification.TlPathwayId,
                TlSpecialismLarIds = technicalQualification.TlSpecialismLarIds.Where(s => registrationData.SpecialismCodes.Contains(s.Value)),
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
                            AcademicYear = registration.RegistrationDate.Year, // TODO: Need to calcualate based on the requirements
                            RegistrationDate = registration.RegistrationDate,
                            StartDate = DateTime.UtcNow,
                            Status = RegistrationPathwayStatus.Active,
                            IsBulkUpload = true,
                            TqRegistrationSpecialisms = MapSpecialisms(registration, performedBy, registrationSpecialismStartIndex),
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

            if (hasAnyMatchedRegistrationsToProcess)
            {
                var tqRegistrationProfileComparer = new TqRegistrationProfileEqualityComparer();

                amendedRegistrations = matchedRegistrations.Except(unchangedRegistrations, ulnComparer).ToList();

                amendedRegistrations.ForEach(amendedRegistration =>
                {
                    var existingRegistration = existingRegistrationsFromDb.FirstOrDefault(existingRegistration => existingRegistration.UniqueLearnerNumber == amendedRegistration.UniqueLearnerNumber);

                    if (existingRegistration != null)
                    {
                        var hasBothPathwayAndSpecialismsRecordsChanged = false;
                        var hasOnlySpecialismsRecordChanged = false;
                        var hasTqRegistrationProfileRecordChanged = !tqRegistrationProfileComparer.Equals(amendedRegistration, existingRegistration);

                        amendedRegistration.Id = existingRegistration.Id; // assign existing registrionprofile table Id to amendedRegistration Id
                        amendedRegistration.TqRegistrationPathways.ToList().ForEach(p => p.TqRegistrationProfileId = existingRegistration.Id); // updating profile fk

                        // below step returns only Active Pathway and associated acitve specialisms
                        var activePathwayRegistrationsInDb = GetActivePathwayAndSpecialism(existingRegistration);

                        var pathwaysToAdd = amendedRegistration.TqRegistrationPathways.Where(mp => !activePathwayRegistrationsInDb.Any(ap => ap.TqProviderId == mp.TqProviderId));
                        var pathwaysToUpdate = (pathwaysToAdd.Any() ? activePathwayRegistrationsInDb : activePathwayRegistrationsInDb.Where(s => amendedRegistration.TqRegistrationPathways.Any(r => r.TqProviderId == s.TqProviderId))).ToList();

                        if (pathwaysToUpdate.Any())
                        {
                            response = ValidateStage4Rules(amendedRegistration, pathwaysToUpdate, response);

                            if (response.IsValid)
                            {
                                var entitiesChangeStatus = PrepareAmendedPathwayAndSpecialisms(amendedRegistration, pathwaysToUpdate);
                                hasBothPathwayAndSpecialismsRecordsChanged = entitiesChangeStatus.Item1;
                                hasOnlySpecialismsRecordChanged = entitiesChangeStatus.Item2;
                            }
                        }

                        if (response.IsValid)
                        {
                            // Consolidate Amended Registration

                            if (hasTqRegistrationProfileRecordChanged)
                            {
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
                var toAddRegistration = TransformManualRegistrationModel(model, validateStage3Response);
                var hasRegistrationAlreadyExists = await _tqRegistrationRepository.GetFirstOrDefaultAsync(p => p.UniqueLearnerNumber == model.Uln) != null;

                if (hasRegistrationAlreadyExists)
                {
                    //TODO Log and return false
                    return false;
                }
                else
                {
                    return await _tqRegistrationRepository.CreateAsync(toAddRegistration) > 0;
                }
            }
            else
            {
                // TODO: log errors
                return false;
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
                CreatedBy = model.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                TqRegistrationPathways = new List<TqRegistrationPathway>
                {
                    new TqRegistrationPathway
                    {
                        TqProviderId = registrationRecord.TqProviderId,
                        AcademicYear = registrationRecord.RegistrationDate.Year, // TODO: Need to calcualate based on the requirements
                        RegistrationDate = registrationRecord.RegistrationDate,
                        StartDate = DateTime.UtcNow,
                        Status = RegistrationPathwayStatus.Active,
                        IsBulkUpload = false,
                        TqRegistrationSpecialisms = MapSpecialisms(registrationRecord, model.CreatedBy, 0, false),
                        CreatedBy = model.CreatedBy,
                        CreatedOn = DateTime.UtcNow
                    }
                }
            };
            return toAddRegistration;
        }

        private RegistrationRecordResponse AddStage3ValidationError(RegistrationCsvRecordResponse registrationCsvRecordResponse, string errorMessage)
        {
            return new RegistrationRecordResponse()
            {
                ValidationErrors = new List<RegistrationValidationError>()
                {
                    new RegistrationValidationError
                    {
                        RowNum = registrationCsvRecordResponse.RowNum.ToString(),
                        Uln = registrationCsvRecordResponse.Uln.ToString(),
                        ErrorMessage = errorMessage
                    }
                }
            };
        }

        private RegistrationRecordResponse AddStage3ValidationError(int rowNum, long uln, string errorMessage)
        {
            return new RegistrationRecordResponse()
            {
                ValidationErrors = new List<RegistrationValidationError>()
                {
                    new RegistrationValidationError
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
            var result = await _tqProviderRepository.GetManyAsync(p => p.TqAwardingOrganisation.TlAwardingOrganisaton.UkPrn == ukprn,
                p => p.TlProvider, p => p.TqAwardingOrganisation, p => p.TqAwardingOrganisation.TlAwardingOrganisaton,
                p => p.TqAwardingOrganisation.TlPathway, p => p.TqAwardingOrganisation.TlPathway.TlSpecialisms).Select(t => new TechnicalQualificationDetails
                {
                    ProviderUkprn = t.TlProvider.UkPrn,
                    TlPathwayId = t.TqAwardingOrganisation.TlPathway.Id,
                    PathwayLarId = t.TqAwardingOrganisation.TlPathway.LarId,
                    TqProviderId = t.Id,
                    TlProviderId = t.TlProviderId,
                    TqAwardingOrganisationId = t.TqAwardingOrganisationId,
                    TlAwardingOrganisatonId = t.TqAwardingOrganisation.TlAwardingOrganisatonId,
                    TlSpecialismLarIds = t.TqAwardingOrganisation.TlPathway.TlSpecialisms.Select(s => new KeyValuePair<int, string>(s.Id, s.LarId))
                }).ToListAsync();

            return result;
        }

        private IList<TqRegistrationSpecialism> MapSpecialisms(RegistrationRecordResponse registration, string performedBy, int specialismStartIndex, bool isBulkUpload = true)
        {
            return registration.TlSpecialismLarIds.Select((x, index) => new TqRegistrationSpecialism
            {
                Id = isBulkUpload ? index - specialismStartIndex : 0,
                TlSpecialismId = x.Key,
                StartDate = DateTime.UtcNow,
                Status = RegistrationSpecialismStatus.Active,
                IsBulkUpload = isBulkUpload,
                CreatedBy = performedBy,
                CreatedOn = DateTime.UtcNow,
            }).ToList();
        }

        private IEnumerable<TqRegistrationPathway> GetActivePathwayAndSpecialism(TqRegistrationProfile existingRegistration)
        {
            return existingRegistration.TqRegistrationPathways.Where(p => p.Status == RegistrationPathwayStatus.Active)
                                    .Select(x =>
                                    {
                                        x.TqRegistrationSpecialisms = x.TqRegistrationSpecialisms.Where(s => s.Status == RegistrationSpecialismStatus.Active).ToList();
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
            return response;
        }

        private Tuple<bool, bool> PrepareAmendedPathwayAndSpecialisms(TqRegistrationProfile amendedRegistration, List<TqRegistrationPathway> pathwaysToUpdate)
        {
            var hasBothPathwayAndSpecialismsRecordsChanged = false;
            var hasOnlySpecialismsRecordChanged = false;

            var hasProviderChanged = !pathwaysToUpdate.Any(x => amendedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TlProviderId == x.TqProvider.TlProviderId));
            var hasRegistrationDateChanged = !pathwaysToUpdate.Any(x => amendedRegistration.TqRegistrationPathways.Any(r => r.RegistrationDate == x.RegistrationDate));

            // change existing TqRegistrationPathway record status and related TqRegistrationSpecialism records status to "Changed"
            if (hasProviderChanged || hasRegistrationDateChanged)
            {
                pathwaysToUpdate.ForEach(pathwayToUpdate =>
                {
                    pathwayToUpdate.Status = hasProviderChanged ? RegistrationPathwayStatus.Transferred : RegistrationPathwayStatus.InActive;
                    pathwayToUpdate.EndDate = DateTime.UtcNow;
                    pathwayToUpdate.ModifiedBy = amendedRegistration.CreatedBy;
                    pathwayToUpdate.ModifiedOn = DateTime.UtcNow;

                    pathwayToUpdate.TqRegistrationSpecialisms.Where(s => s.Status == RegistrationSpecialismStatus.Active).ToList().ForEach(specialismToUpdate =>
                    {
                        specialismToUpdate.Status = RegistrationSpecialismStatus.InActive;
                        specialismToUpdate.EndDate = DateTime.UtcNow;
                        specialismToUpdate.ModifiedBy = amendedRegistration.CreatedBy;
                        specialismToUpdate.ModifiedOn = DateTime.UtcNow;
                    });
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
                        var existingSpecialismsInDb = existingPathwayRecordInDb.TqRegistrationSpecialisms.Where(s => s.Status == RegistrationSpecialismStatus.Active);
                        var specialismsToAdd = importPathwayRecord.TqRegistrationSpecialisms.Where(s => !existingSpecialismsInDb.Any(r => r.TlSpecialismId == s.TlSpecialismId)).ToList();
                        var specialismsToUpdate = existingSpecialismsInDb.Where(s => !importPathwayRecord.TqRegistrationSpecialisms.Any(r => r.TlSpecialismId == s.TlSpecialismId)).ToList();

                        specialismsToUpdate.ForEach(s =>
                        {
                            s.Status = RegistrationSpecialismStatus.InActive;
                            s.EndDate = DateTime.UtcNow;
                            s.ModifiedBy = amendedRegistration.CreatedBy;
                            s.ModifiedOn = DateTime.UtcNow;
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
            return new Tuple<bool, bool>(hasBothPathwayAndSpecialismsRecordsChanged, hasOnlySpecialismsRecordChanged);
        }

        private RegistrationValidationError GetRegistrationValidationError(long uln, string errorMessage)
        {
            return new RegistrationValidationError
            {
                Uln = uln.ToString(),
                ErrorMessage = errorMessage
            };
        }
    }
}
