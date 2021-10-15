using Sfa.Tl.ResultsAndCertification.Models.Contracts.Ucas;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface IUcasApiClient
    {
        Task<string> GetTokenAsync();
        Task<string> SendData(UcasDataRequest request);
    }
}
