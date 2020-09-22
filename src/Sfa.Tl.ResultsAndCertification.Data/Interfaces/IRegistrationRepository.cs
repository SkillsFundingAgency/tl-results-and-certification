using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IRegistrationRepository : IRepository<TqRegistrationProfile>
    {
        Task<TqRegistrationProfile> GetActiveRegistrationProfileAsync(long aoUkprn, int profileId);
        Task<TqRegistrationProfile> GetRegistrationDataWithHistoryAsync(long aoUkprn, int profileId);
        Task<RegistrationDetails> GetRegistrationDetailsByProfileIdAsync(long aoUkprn, int profileId);
        Task<IList<TqRegistrationProfile>> GetRegistrationProfilesAsync(IList<TqRegistrationProfile> registrations);
        Task<bool> BulkInsertOrUpdateTqRegistrations(List<TqRegistrationProfile> profileEntities, List<TqRegistrationPathway> pathwayEntities, List<TqRegistrationSpecialism> specialismEntities);
        
        Task<ManageRegistration> GetRegistrationAsync(long aoUkprn, int profileId);
        Task<TqRegistrationPathway> GetRegistrationPathwayDetails(long aoUkprn, int profileId);
        Task<int> UpdateRegistrationWithSpecifedCollectionsOnlyAsync(TqRegistrationPathway entity, params Expression<Func<TqRegistrationPathway, object>>[] properties);

        Task<TqRegistrationPathway> GetRegistrationAsync(long aoUkprn, int profileId, RegistrationPathwayStatus? status = null);
    }
}
