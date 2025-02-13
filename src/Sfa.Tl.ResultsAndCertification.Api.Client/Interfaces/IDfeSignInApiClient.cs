using Sfa.Tl.ResultsAndCertification.Models.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface IDfeSignInApiClient
    {
        Task<DfeUserInfo> GetDfeSignInUserInfo(string organisationId, string userId);
        Task<IEnumerable<DfeUsers>> GetDfeUsersAllProviders(IEnumerable<long> ukPrns);
        Task<DfeUsers> GetDfeUsersForProvider(string ukPrn);
    }
}