using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        public async Task<IList<RegistrationRecordResponse>> ValidateRegistrationTlevelsAsync(long aoUkprn, IList<RegistrationCsvRecordResponse> validRegistrationsData)
        {
            var result = new List<RegistrationRecordResponse>();
            var aoProviderTlevels = await GetAllTLevelsByAoUkprnAsync(aoUkprn);

            // Below are other than duplicated
            validRegistrationsData.ToList().ForEach(registrationData =>
            {                
                // Validation: AO not registered for the T level. 
                var isProviderRegisteredWithAwardingOrganisation = aoProviderTlevels.Any(t => t.ProviderUkprn == registrationData.ProviderUkprn);
                if (!isProviderRegisteredWithAwardingOrganisation)
                {
                    result.Add(AddStage3ValidationError(registrationData, ValidationMessages.ProviderNotRegisteredWithAo));
                    return;
                }

                // Validation: Provider not registered for the T level
                var technicalQualification = aoProviderTlevels.FirstOrDefault(tq => tq.ProviderUkprn == registrationData.ProviderUkprn && tq.PathwayLarId == registrationData.CoreCode);
                if (technicalQualification == null)
                {
                    result.Add(AddStage3ValidationError(registrationData, ValidationMessages.CoreNotRegisteredWithProvider));
                    return;
                }
                
                if (registrationData.SpecialismCodes.Count() > 0)
                {
                    var specialismCodes = technicalQualification.TlSpecialismLarIds.Select(x => x.Value);
                    var invalidSpecialismCodes = registrationData.SpecialismCodes.Except(specialismCodes, StringComparer.InvariantCultureIgnoreCase);

                    if (invalidSpecialismCodes.Any())
                    {
                        result.Add(AddStage3ValidationError(registrationData, ValidationMessages.SpecialismNotValidWithCore));
                        return;
                    }
                }

                result.Add(new RegistrationRecordResponse
                {
                    TqProviderId = technicalQualification.TqProviderId,
                    TqAwardingOrganisationId = technicalQualification.TqAwardingOrganisationId,
                    TlSpecialismLarIds = technicalQualification.TlSpecialismLarIds,
                    TlAwardingOrganisatonId = technicalQualification.TlAwardingOrganisatonId,
                    TlProviderId = technicalQualification.TlProviderId
                });
            });

            return result;
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

           
            //x.ValidationErrors.Add(new RegistrationValidationError
            //{
            //    RowNum = registrationCsvRecordResponse.RowNum.ToString(),
            //    Uln = registrationCsvRecordResponse.Uln.ToString(),
            //    ErrorMessage = errorMessage
            //});
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
