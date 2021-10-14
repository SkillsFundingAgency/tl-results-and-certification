using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface IUcasApiClient
    {
        Task<string> GetTokenAsync();
        Task<bool> SendData(string fileName, byte[] data);
    }
}
