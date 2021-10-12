using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IUcasDataService
    {
        Task<UcasData> GetUcasEntriesAsync();
    }
}
