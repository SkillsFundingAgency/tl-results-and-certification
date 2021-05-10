using Sfa.Tl.ResultsAndCertification.Models.Contracts.OrdnanceSurvey;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces
{
    public interface IOrdnanceSurveyApiClient
    {
        Task<PostcodeLookupResult> GetAddressesByPostcode(string postcode);
        Task<PostcodeLookupResult> GetAddressByUprn(long uprn);
    }
}