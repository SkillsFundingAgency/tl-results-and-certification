﻿using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Data.Interfaces
{
    public interface IRegistrationRepository : IRepository<TqRegistrationProfile>
    {
        Task<TqRegistrationPathway> GetRegistrationAsync(long aoUkprn, int profileId);
        Task<TqRegistrationPathway> GetRegistrationLiteAsync(long aoUkprn, int profileId, bool includeProfile = true, bool includeIndustryPlacements = false, bool includeOverallResults = false, bool includePrintCertificates = false);
        Task<TqRegistrationPathway> GetRegistrationLiteByProviderUkprnAsync(long providerUkprn, int profileId, bool includeProfile = true, bool includeIndustryPlacements = false, bool includeOverallResults = false);
        Task<TqRegistrationProfile> GetRegistrationDataWithHistoryAsync(long aoUkprn, int profileId);
        Task<IList<TqRegistrationProfile>> GetRegistrationProfilesByIdsAsync(HashSet<int> profileIds, bool includeQualificationAchieved = false);
        Task<IList<TqRegistrationPathway>> GetRegistrationPathways();
        Task<IList<TqRegistrationPathway>> GetRegistrationPathwaysByAssesmentSeriesYear(string[] assesmentSeriesYears);
        Task<IList<TqRegistrationSpecialism>> GetSpecialismRegistrationPathwaysByAssesmentSeriesYear(string[] assesmentSeriesYears);

        // Bulk process methods below.
        Task<IList<TqRegistrationProfile>> GetRegistrationProfilesAsync(IList<TqRegistrationProfile> registrations);
        Task<bool> BulkInsertOrUpdateTqRegistrations(List<TqRegistrationProfile> profileEntities, List<TqRegistrationPathway> pathwayEntities, List<TqRegistrationSpecialism> specialismEntities);
    }
}