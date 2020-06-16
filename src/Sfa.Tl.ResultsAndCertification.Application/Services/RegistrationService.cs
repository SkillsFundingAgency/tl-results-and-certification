using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        public RegistrationService()
        { }

        public Task<object> CompareAndProcessRegistrations()
        {
            throw new NotImplementedException();
        }

        public object TransformRegistrationModel()
        {
            return new object();
        }

        public async Task ValidateRegistrationTlevelsAsync(object p)
        {
        }
    }
}
