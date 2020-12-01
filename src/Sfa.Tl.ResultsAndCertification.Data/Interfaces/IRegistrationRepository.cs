using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IRegistrationRepository : IRepository<TqRegistrationProfile>
    {
        Task<TqRegistrationPathway> GetRegistrationAsync(long aoUkprn, int profileId);
        Task<TqRegistrationPathway> GetRegistrationLiteAsync(long aoUkprn, int profileId, bool includeProfile = true);
        Task<TqRegistrationProfile> GetRegistrationDataWithHistoryAsync(long aoUkprn, int profileId);
        
        // Bulk process methods below.
        Task<IList<TqRegistrationProfile>> GetRegistrationProfilesAsync(IList<TqRegistrationProfile> registrations);
        Task<bool> BulkInsertOrUpdateTqRegistrations(List<TqRegistrationProfile> profileEntities, List<TqRegistrationPathway> pathwayEntities, List<TqRegistrationSpecialism> specialismEntities);
    }
}
