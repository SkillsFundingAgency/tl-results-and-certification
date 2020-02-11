using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces
{
    public interface IAwardingOrganisationLoader
    {
        Task<IEnumerable<YourTlevelsViewModel>> GetTlevelsByAwardingOrganisationAsync();
    }
}
