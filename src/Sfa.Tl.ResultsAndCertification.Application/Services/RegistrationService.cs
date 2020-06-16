using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class RegistrationService : IRegistrationService
    {
        public RegistrationService()
        { }

        public async Task<object> CompareAndProcessRegistrations()
        {
            return await Task.Run(() => new object());
        }

        public object TransformRegistrationModel()
        {
            return new object();
        }

        public async Task ValidateRegistrationTlevelsAsync(object p)
        {
            await Task.Run(() => true);
        }
    }
}
