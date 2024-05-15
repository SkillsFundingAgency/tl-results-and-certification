using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface ISearchRegistrationLoader
    {
        Task<SearchRegistrationViewModel> CreateSearchRegistration(SearchRegistrationType type);

        Task<SearchRegistrationDetailsListViewModel> GetSearchRegistrationDetailsListAsync(long aoUkprn, SearchRegistrationType type, SearchRegistrationCriteriaViewModel searchCriteria);
    }
}