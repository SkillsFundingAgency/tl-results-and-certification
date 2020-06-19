using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
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
        public RegistrationService(IProviderRepository providerRespository)
        {
            _tqProviderRepository = providerRespository;
        }

        public async Task<object> CompareAndProcessRegistrations()
        {
            return await Task.Run(() => new object());
        }

        public object TransformRegistrationModel()
        {
            return new object();
        }

        public async Task<IList<RegistrationCsvRecordResponse>> ValidateRegistrationTlevelsAsync(long aoUkprn, IList<RegistrationCsvRecordResponse> validRegistrationsData)
        {
            var aoProviderTlevels = await GetAllTLevelsByAoUkprnAsync(aoUkprn);

            // Below are other than duplicated
            validRegistrationsData.ToList().ForEach(registrationData =>
            {
                // Validation: AO not registered for the T level. 
                var isProviderRegisteredWithAwardingOrganisation = aoProviderTlevels.Any(t => t.ProviderUkprn == registrationData.Ukprn);
                if (!isProviderRegisteredWithAwardingOrganisation)
                {
                    AddStage3ValidationError(registrationData, "Provider not registered for AO.");
                    return;
                }

                // Validation: Provider not registered for the T level
                var technicalQualification = aoProviderTlevels.FirstOrDefault(tq => tq.ProviderUkprn == registrationData.Ukprn && tq.PathwayLarId == registrationData.Core);

                if (technicalQualification == null)
                {
                    AddStage3ValidationError(registrationData, "Core is not registered with Provider.");
                    return;
                }
                
                if (registrationData.Specialisms.Count() > 0)
                {
                    var specialismCodes = technicalQualification.TlSpecialismLarIds.Select(x => x.Value);
                    var invalidSpecialismCodes = registrationData.Specialisms.Except(specialismCodes, StringComparer.InvariantCultureIgnoreCase);

                    if (invalidSpecialismCodes.Any())
                    {
                        AddStage3ValidationError(registrationData, "Specialism not valid with core.");
                        return;
                    }
                }

                registrationData.TqProviderId = technicalQualification.TqProviderId;
                registrationData.TqAwardingOrganisationId = technicalQualification.TqAwardingOrganisationId;
                registrationData.TlSpecialismLarIds = technicalQualification.TlSpecialismLarIds;
                registrationData.TlAwardingOrganisatonId = technicalQualification.TlAwardingOrganisatonId;
                registrationData.TlProviderId = technicalQualification.TlProviderId;
            });

            return validRegistrationsData;
        }

        private static void AddStage3ValidationError(RegistrationCsvRecordResponse x, string errorMessage)
        {
            x.ValidationErrors.Add(new RegistrationValidationError
            {
                RowNum = x.RowNum.ToString(),
                Uln = x.Uln.ToString(),
                ErrorMessage = errorMessage
            });
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
                    TlSpecialisms = t.TqAwardingOrganisation.TlPathway.TlSpecialisms.Select(s => s.Id).ToList(),
                    TlSpecialismLarIds = t.TqAwardingOrganisation.TlPathway.TlSpecialisms.Select(s => new KeyValuePair<int, string>(s.Id, s.LarId)).ToList()
                }).ToListAsync();

            return result;
        }
    }
}
