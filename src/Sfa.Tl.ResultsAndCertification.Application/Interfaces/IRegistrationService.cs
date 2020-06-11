using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IRegistrationService
    {
        Task ValidateRegistrationTlevelsAsync(object p);
        object TransformRegistrationModel();
        Task<object> CompareAndProcessRegistrations();
    }
}
