using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
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

        public async Task<RegistrationProcessResponse> CompareAndProcessRegistrationsAsync(IList<TqRegistrationProfile> registrationsToProcess)
        {
            var response = new RegistrationProcessResponse();
            var ulnComparer = new TqRegistrationUlnEqualityComparer();
            var comparer = new TqRegistrationRecordEqualityComparer();

            var amendedRegistrations = new List<TqRegistrationProfile>();
            var amendedRegistrationsToIgnore = new List<TqRegistrationProfile>();
            var amendedPathwayRecords = new List<TqRegistrationPathway>();
            var amendedSpecialismRecords = new List<TqRegistrationSpecialism>();

            var existingRegistrationsFromDb = await _tqRegistrationRepository.GetRegistrationProfilesAsync(registrationsToProcess);

            var newRegistrations = registrationsToProcess.Except(existingRegistrationsFromDb, ulnComparer).ToList();
            var matchedRegistrations = registrationsToProcess.Intersect(existingRegistrationsFromDb, ulnComparer).ToList();
            var unchangedRegistrations = matchedRegistrations.Intersect(existingRegistrationsFromDb, comparer).ToList();

            if (matchedRegistrations.Count != unchangedRegistrations.Count)
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
                        var activePathwayRegistrationsInDb = existingRegistration.TqRegistrationPathways.Where(p => p.Status == RegistrationPathwayStatus.Active)
                        .Select(x =>
                        {
                            x.TqRegistrationSpecialisms = x.TqRegistrationSpecialisms.Where(s => s.Status == RegistrationSpecialismStatus.Active).ToList();
                            return x;
                        });

                        var pathwaysToAdd = amendedRegistration.TqRegistrationPathways.Where(mp => !activePathwayRegistrationsInDb.Any(ap => ap.TqProviderId == mp.TqProviderId));
                        var pathwaysToUpdate = (pathwaysToAdd.Any() ? activePathwayRegistrationsInDb : activePathwayRegistrationsInDb.Where(s => amendedRegistration.TqRegistrationPathways.Any(r => r.TqProviderId == s.TqProviderId))).ToList();

                        if (pathwaysToUpdate.Any())
                        {
                            var hasProviderChanged = !pathwaysToUpdate.Any(x => amendedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TlProviderId == x.TqProvider.TlProviderId));
                            var hasPathwayChanged = !pathwaysToUpdate.Any(x => amendedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TqAwardingOrganisation.TlPathwayId == x.TqProvider.TqAwardingOrganisation.TlPathwayId));
                            var hasRegistrationDateChanged = !pathwaysToUpdate.Any(x => amendedRegistration.TqRegistrationPathways.Any(r => r.RegistrationDate == x.RegistrationDate));

                            // check if there is an active registration for another AO, if so show error message and reject the file
                            var hasAoChanged = !pathwaysToUpdate.Any(x => amendedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId == x.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId));

                            if (hasAoChanged || hasPathwayChanged)
                            {
                                response.ValidationErrors.Add(GetRegistrationValidationError(amendedRegistration.UniqueLearnerNumber, hasAoChanged ? ValidationMessages.ActiveUlnWithDifferentAo : ValidationMessages.CoreForUlnCannotBeChangedYet));
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
                            }
                        }

                        if (response.IsValid)
                        {
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
                            else if(!hasTqRegistrationProfileRecordChanged && !hasBothPathwayAndSpecialismsRecordsChanged && !hasOnlySpecialismsRecordChanged)
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
            }

            response.BulkUploadStats = new BulkUploadStats
            {
                TotalRecordsCount = registrationsToProcess.Count,
                NewRecordsCount = newRegistrations.Count,
                AmendedRecordsCount = amendedRegistrations.Count,
                UnchangedRecordsCount = unchangedRegistrations.Count
            };
            return response;
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
                            AcademicYear = registration.StartDate.Year, // TODO: Need to calcualate based on the requirements
                            RegistrationDate = registration.StartDate,
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

        private static IList<TqRegistrationSpecialism> MapSpecialisms(RegistrationRecordResponse registration, string performedBy, int specialismStartIndex)
        {
            return registration.TlSpecialismLarIds.Select((x, index) => new TqRegistrationSpecialism
            {
                Id =  index - specialismStartIndex,
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
