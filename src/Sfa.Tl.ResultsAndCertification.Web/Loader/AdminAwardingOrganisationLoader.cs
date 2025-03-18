using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAwardingOrganisation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader
{
    public class AdminAwardingOrganisationLoader : IAdminAwardingOrganisationLoader
    {
        private readonly IResultsAndCertificationInternalApiClient _internalApiClient;

        public AdminAwardingOrganisationLoader(IResultsAndCertificationInternalApiClient internalApiClient)
        {
            _internalApiClient = internalApiClient;
        }

        public async Task<AdminSelectAwardingOrganisationViewModel> GetSelectAwardingOrganisationViewModelAsync()
        {
            IEnumerable<AwardingOrganisationMetadata> awardingOrganisations = await _internalApiClient.GetAllAwardingOrganisationsAsync();

            return new AdminSelectAwardingOrganisationViewModel
            {
                AwardingOrganisations = awardingOrganisations.ToArray()
            };
        }

        public async Task<string> GetAwardingOrganisationDisplayName(long awardingOrganisationUkprn)
        {
            AwardingOrganisationMetadata awardingOrganisation = await _internalApiClient.GetAwardingOrganisationByUkprnAsync(awardingOrganisationUkprn);
            return awardingOrganisation == null ? string.Empty : awardingOrganisation.DisplayName;
        }
    }
}