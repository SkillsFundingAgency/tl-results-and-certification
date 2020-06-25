using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
            var existingRegistrationsFromDb = await _tqRegistrationRepository.GetRegistrationProfilesAsync(registrationsToProcess);

            var newRegistrations = registrationsToProcess.Except(existingRegistrationsFromDb, ulnComparer);
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
                        var activePathwayRegistrationsInDb = existingRegistration.TqRegistrationPathways.Where(p => p.Status == (int)RegistrationPathwayStatus.Active);
                        var pathwaysToAdd = modifiedRegistration.TqRegistrationPathways.Where(mp => !activePathwayRegistrationsInDb.Any(ap => ap.TqProviderId == mp.TqProviderId || ap.StartDate.Date == mp.StartDate.Date));
                        var pathwaysToUpdate = (pathwaysToAdd.Any() ? activePathwayRegistrationsInDb : activePathwayRegistrationsInDb.Where(s => modifiedRegistration.TqRegistrationPathways.Any(r => r.TqProviderId == s.TqProviderId))).ToList();

                        if (pathwaysToUpdate.Any())
                        {
                            var hasPathwayChanged = !pathwaysToUpdate.Any(x => modifiedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TqAwardingOrganisation.TlPathwayId == x.TqProvider.TqAwardingOrganisation.TlPathwayId));

                            // check if there is an active registration for another AO, if so show error message and reject the file
                            var hasAoChanged = !pathwaysToUpdate.Any(x => modifiedRegistration.TqRegistrationPathways.Any(r => r.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId == x.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId));

                            if (hasAoChanged || hasPathwayChanged)
                            {
                                response.ValidationErrors.Add(GetRegistrationValidationError(modifiedRegistration.UniqueLearnerNumber, hasAoChanged ? ValidationMessages.ActiveUlnWithDifferentAo : ValidationMessages.CoreForUlnCannotBeChangedYet));
                            }
                        }
                    }
                });
            }
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
                            AcademicYear = 1234,
                            StartDate = registration.StartDate,
                            Status = (int)RegistrationPathwayStatus.Active,
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
                    TlSpecialismLarIds = technicalQualification.TlSpecialismLarIds,
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
                Status = (int)RegistrationSpecialismStatus.Active,
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
