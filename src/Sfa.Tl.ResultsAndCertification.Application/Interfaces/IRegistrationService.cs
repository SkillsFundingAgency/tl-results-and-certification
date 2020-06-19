using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IRegistrationService
    {
        Task<IList<RegistrationCsvRecordResponse>> ValidateRegistrationTlevelsAsync(long aoUkprn, IList<RegistrationCsvRecordResponse> registrationData);
        object TransformRegistrationModel();
        Task<object> CompareAndProcessRegistrations();        
    }
}
