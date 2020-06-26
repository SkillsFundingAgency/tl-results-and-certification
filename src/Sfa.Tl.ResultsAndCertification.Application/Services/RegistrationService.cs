using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Comparer;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
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
        public RegistrationService(IProviderRepository providerRespository, IRegistrationRepository tqRegistrationRepository)
        {
            _tqProviderRepository = providerRespository;
            _tqRegistrationRepository = tqRegistrationRepository;
        }

        public async Task<RegistrationProcessResponse> CompareAndProcessRegistrations(IList<TqRegistrationProfile> registrationsToProcess)
        {
            var response = new RegistrationProcessResponse();
            var ulnComparer = new TqRegistrationUlnEqualityComparer();
            var comparer = new TqRegistrationRecordEqualityComparer();

            var modifiedRegistrations = new List<TqRegistrationProfile>();
            var modifiedRegistrationsToIgnore = new List<TqRegistrationProfile>();
            var modifiedPathwayRecords = new List<TqRegistrationPathway>();
            var modifiedSpecialismRecords = new List<TqRegistrationSpecialism>();

            var existingRegistrationsFromDb = await _tqRegistrationRepository.GetRegistrationProfilesAsync(registrationsToProcess);

            var newRegistrations = registrationsToProcess.Except(existingRegistrationsFromDb, ulnComparer).ToList();
            var matchedRegistrations = registrationsToProcess.Intersect(existingRegistrationsFromDb, ulnComparer).ToList();
            var sameOrDuplicateRegistrations = matchedRegistrations.Intersect(existingRegistrationsFromDb, comparer).ToList();

            if (matchedRegistrations.Count != sameOrDuplicateRegistrations.Count)
            {
                var tqRegistrationProfileComparer = new TqRegistrationProfileEqualityComparer();

                modifiedRegistrations = matchedRegistrations.Except(sameOrDuplicateRegistrations, comparer).ToList();

                modifiedRegistrations.ForEach(modifiedRegistration =>
                {
                    var existingRegistration = existingRegistrationsFromDb.FirstOrDefault(existingRegistration => existingRegistration.UniqueLearnerNumber == modifiedRegistration.UniqueLearnerNumber);

                    if (existingRegistration != null)
                    {
                        var hasBothPathwayAndSpecialismsRecordsChanged = false;
                        var hasOnlySpecialismsRecordChanged = false;
                        var hasTqRegistrationProfileRecordChanged = !tqRegistrationProfileComparer.Equals(modifiedRegistration, existingRegistration);

                        var activePathwayRegistrationsInDb = existingRegistration.TqRegistrationPathways.Where(p => p.Status == RegistrationPathwayStatus.Active);
                        var pathwaysToAdd = modifiedRegistration.TqRegistrationPathways.Where(mp => !activePathwayRegistrationsInDb.Any(ap => ap.TqProviderId == mp.TqProviderId || ap.RegistrationDate == mp.RegistrationDate));
                        var pathwaysToUpdate = (pathwaysToAdd.Any() ? activePathwayRegistrationsInDb : activePathwayRegistrationsInDb.Where(s => modifiedRegistration.TqRegistrationPathways.Any(r => r.TqProviderId == s.TqProviderId))).ToList();

                        if (pathwaysToUpdate.Any())
                        {
                            var hasProviderChanged = !pathwaysToUpdate.Any(x => modifiedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TlProviderId == x.TqProvider.TlProviderId));
                            var hasPathwayChanged = !pathwaysToUpdate.Any(x => modifiedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TqAwardingOrganisation.TlPathwayId == x.TqProvider.TqAwardingOrganisation.TlPathwayId));
                            var hasRegistrationDateChanged = !pathwaysToUpdate.Any(x => modifiedRegistration.TqRegistrationPathways.Any(r => r.RegistrationDate == x.RegistrationDate));

                            // check if there is an active registration for another AO, if so show error message and reject the file
                            var hasAoChanged = !pathwaysToUpdate.Any(x => modifiedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId == x.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId));

                            if (hasAoChanged || hasPathwayChanged)
                            {
                                response.ValidationErrors.Add(GetRegistrationValidationError(modifiedRegistration.UniqueLearnerNumber, hasAoChanged ? ValidationMessages.ActiveUlnWithDifferentAo : ValidationMessages.CoreForUlnCannotBeChangedYet));
                            }

                            if (response.IsValid)
                            {
                                // change existing TqRegistrationPathway record status and related TqRegistrationSpecialism records status to "Changed"
                                if (hasProviderChanged || hasRegistrationDateChanged)
                                {
                                    pathwaysToUpdate.ForEach(pathwayToUpdate =>
                                    {
                                        pathwayToUpdate.Status = hasProviderChanged ? RegistrationPathwayStatus.Transferred : RegistrationPathwayStatus.InActive;
                                        pathwayToUpdate.EndDate = DateTime.UtcNow;
                                        pathwayToUpdate.ModifiedBy = modifiedRegistration.CreatedBy;
                                        pathwayToUpdate.ModifiedOn = DateTime.UtcNow;

                                        pathwayToUpdate.TqRegistrationSpecialisms.Where(s => s.Status == RegistrationSpecialismStatus.Active).ToList().ForEach(specialismToUpdate =>
                                        {
                                            specialismToUpdate.Status = RegistrationSpecialismStatus.InActive;
                                            specialismToUpdate.EndDate = DateTime.UtcNow;
                                            specialismToUpdate.ModifiedBy = modifiedRegistration.CreatedBy;
                                            specialismToUpdate.ModifiedOn = DateTime.UtcNow;
                                        });
                                    });
                                    hasBothPathwayAndSpecialismsRecordsChanged = true;
                                }
                                else
                                {
                                    foreach (var importPathwayRecord in modifiedRegistration.TqRegistrationPathways)
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
                                                s.ModifiedBy = modifiedRegistration.CreatedBy;
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
                            }
                        }

                        if (response.IsValid)
                        {
                            modifiedRegistration.Id = existingRegistration.Id;
                            modifiedRegistration.TqRegistrationPathways.ToList().ForEach(p => p.TqRegistrationProfileId = existingRegistration.Id);

                            if (hasTqRegistrationProfileRecordChanged && hasBothPathwayAndSpecialismsRecordsChanged)
                            {
                                modifiedRegistration.TqRegistrationPathways = pathwaysToAdd.Concat(pathwaysToUpdate).ToList();
                            }
                            else if (hasTqRegistrationProfileRecordChanged && !hasBothPathwayAndSpecialismsRecordsChanged && hasOnlySpecialismsRecordChanged)
                            {
                                pathwaysToUpdate.ForEach(p => { modifiedSpecialismRecords.AddRange(p.TqRegistrationSpecialisms); });
                                modifiedRegistration.TqRegistrationPathways.Clear();
                            }
                            else if (hasTqRegistrationProfileRecordChanged && !hasBothPathwayAndSpecialismsRecordsChanged && !hasOnlySpecialismsRecordChanged)
                            {
                                modifiedRegistration.TqRegistrationPathways.Clear();
                            }
                            else if (hasBothPathwayAndSpecialismsRecordsChanged && !hasOnlySpecialismsRecordChanged)
                            {
                                modifiedPathwayRecords.AddRange(pathwaysToAdd.Concat(pathwaysToUpdate));
                                modifiedRegistrationsToIgnore.Add(modifiedRegistration);
                            }
                            else if (!hasBothPathwayAndSpecialismsRecordsChanged && hasOnlySpecialismsRecordChanged)
                            {
                                pathwaysToUpdate.ForEach(p => { modifiedSpecialismRecords.AddRange(p.TqRegistrationSpecialisms); });
                                modifiedRegistrationsToIgnore.Add(modifiedRegistration);
                            }
                            else if(!hasTqRegistrationProfileRecordChanged && !hasBothPathwayAndSpecialismsRecordsChanged && !hasOnlySpecialismsRecordChanged)
                            {
                                modifiedRegistrationsToIgnore.Add(modifiedRegistration);
                            }
                        }
                    }
                });
            }

            if (response.IsValid)
            {
                var registrationsToSendToDB = newRegistrations.Concat(modifiedRegistrations.Except(modifiedRegistrationsToIgnore, ulnComparer)).ToList();
                response.IsSuccess = await _tqRegistrationRepository.BulkInsertOrUpdateTqRegistrations(registrationsToSendToDB, modifiedPathwayRecords, modifiedSpecialismRecords);
            }

            response.BulkUploadStats = new BulkUploadStats
            {
                TotalRecordsCount = registrationsToProcess.Count,
                NewRecordsCount = newRegistrations.Count,
                UpdatedRecordsCount = modifiedRegistrations.Count,
                DuplicateRecordsCount = sameOrDuplicateRegistrations.Count
            };
            return response;
        }

        public IList<TqRegistrationProfile> TransformRegistrationModel(IList<RegistrationRecordResponse> registrationsData, string performedBy)
        {
            var registrationProfiles = new List<TqRegistrationProfile>();

            foreach (var registration in registrationsData)
            {
                registrationProfiles.Add(new TqRegistrationProfile
                {
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
                            TqProviderId = registration.TqProviderId,
                            AcademicYear = registration.StartDate.Year, // TODO: Need to calcualate based on the requirements
                            RegistrationDate = registration.StartDate,
                            StartDate = DateTime.UtcNow,
                            Status = RegistrationPathwayStatus.Active,
                            IsBulkUpload = true,
                            TqRegistrationSpecialisms = MapSpecialisms(registration, performedBy),
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
            }
            return registrationProfiles;
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
                    response.Add(AddStage3ValidationError(registrationData, ValidationMessages.ProviderNotRegisteredWithAo));
                    continue;
                }

                var technicalQualification = aoProviderTlevels.FirstOrDefault(tq => tq.ProviderUkprn == registrationData.ProviderUkprn && tq.PathwayLarId == registrationData.CoreCode);
                if (technicalQualification == null)
                {
                    response.Add(AddStage3ValidationError(registrationData, ValidationMessages.CoreNotRegisteredWithProvider));
                    continue;
                }

                if (registrationData.SpecialismCodes.Count() > 0)
                {
                    var specialismCodes = technicalQualification.TlSpecialismLarIds.Select(x => x.Value);
                    var invalidSpecialismCodes = registrationData.SpecialismCodes.Except(specialismCodes, StringComparer.InvariantCultureIgnoreCase);

                    if (invalidSpecialismCodes.Any())
                    {
                        response.Add(AddStage3ValidationError(registrationData, ValidationMessages.SpecialismNotValidWithCore));
                        continue;
                    }
                }

                response.Add(new RegistrationRecordResponse
                {
                    Uln = registrationData.Uln,
                    FirstName = registrationData.FirstName,
                    LastName = registrationData.LastName,
                    DateOfBirth = registrationData.DateOfBirth,
                    StartDate = registrationData.StartDate,
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

        private static RegistrationRecordResponse AddStage3ValidationError(RegistrationCsvRecordResponse registrationCsvRecordResponse, string errorMessage)
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

        private static IList<TqRegistrationSpecialism> MapSpecialisms(RegistrationRecordResponse registration, string performedBy)
        {
            return registration.TlSpecialismLarIds.Select(x => new TqRegistrationSpecialism
            {
                TlSpecialismId = x.Key,
                StartDate = DateTime.UtcNow,
                Status = RegistrationSpecialismStatus.Active,
                IsBulkUpload = true,
                CreatedBy = performedBy,
                CreatedOn = DateTime.UtcNow,
            }).ToList();
        }

        private static RegistrationValidationError GetRegistrationValidationError(long uln, string errorMessage)
        {
            return new RegistrationValidationError
            {
                Uln = uln.ToString(),
                ErrorMessage = errorMessage
            };
        }
    }
}
