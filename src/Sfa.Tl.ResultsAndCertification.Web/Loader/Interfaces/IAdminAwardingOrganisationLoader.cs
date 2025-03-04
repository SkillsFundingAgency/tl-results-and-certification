using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAwardingOrganisation;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAdminAwardingOrganisationLoader
    {
        Task<AdminSelectAwardingOrganisationViewModel> GetSelectAwardingOrganisationViewModelAsync();

        Task<string> GetAwardingOrganisationDisplayName(long awardingOrganisationUkprn);
    }
}