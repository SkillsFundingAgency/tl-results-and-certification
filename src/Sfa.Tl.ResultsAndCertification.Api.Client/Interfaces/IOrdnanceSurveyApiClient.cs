using Sfa.Tl.ResultsAndCertification.Models.Contracts.OrdnanceSurvey;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface IOrdnanceSurveyApiClient
    {
        Task<PostcodeLookupResult> GetAddressesByPostcodeAsync(string postcode);
        Task<PostcodeLookupResult> GetAddressByUprnAsync(long uprn);
    }
}