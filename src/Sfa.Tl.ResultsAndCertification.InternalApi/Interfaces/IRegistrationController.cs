using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.Interfaces
{
    public interface IRegistrationController
    {
        Task<BulkRegistrationResponse> ProcessBulkRegistrationsAsync(BulkRegistrationRequest request);
    }
}
