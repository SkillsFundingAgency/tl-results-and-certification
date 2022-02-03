using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ResultServiceTests
{
    public abstract class ResultServiceBaseTest : BaseTest<TqPathwayResult>
    {
        protected long AoUkprn = 10011881;
        protected ResultService ResultService;
        protected TlRoute Route;
        protected TlPathway Pathway;
        protected TlSpecialism Specialism;
        protected IList<TlSpecialism> Specialisms;
        protected TlProvider TlProvider;
        protected TlAwardingOrganisation TlAwardingOrganisation;
        protected TqAwardingOrganisation TqAwardingOrganisation;
        protected TqProvider TqProvider;
        protected IList<TlProvider> TlProviders;
        protected IList<TqProvider> TqProviders;
        protected IList<AssessmentSeries> AssessmentSeries;
        protected IList<TlLookup> TlLookup;
        protected IList<TlLookup> PathwayComponentGrades;
        protected IList<TlLookup> SpecialismComponentGrades;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected IResultRepository ResultRepository;
        protected ILogger<ResultRepository> ResultRepositoryLogger;
        protected IRepository<TqPathwayResult> PathwayResultRepository;
        protected ILogger<GenericRepository<TqPathwayResult>> PathwayResultRepositoryLogger;
        protected IRepository<TqSpecialismResult> SpecialismResultRepository;
        protected ILogger<GenericRepository<TqSpecialismResult>> SpecialismResultRepositoryLogger;
        protected IRepository<TqPathwayAssessment> PathwayAssessmentRepository;
        protected IRepository<TqSpecialismAssessment> SpecialismAssessmentRepository;
        protected IRepository<AssessmentSeries> AssessmentSeriesRepository;
        protected ILogger<GenericRepository<AssessmentSeries>> AssessmentSeriesRepositoryLogger;
        protected ILogger<GenericRepository<TqPathwayAssessment>> PathwayAssessmentRepositoryLogger;
        protected IRepository<TlLookup> TlLookupRepository;
        protected ILogger<GenericRepository<TlLookup>> TlLookupRepositoryLogger;
        protected IMapper ResultMapper;
        protected ILogger<ResultService> ResultServiceLogger;

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ResultMapper).Assembly));
            ResultMapper = new Mapper(mapperConfig);
        }

        protected virtual void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, Route);
            Specialism = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, Pathway).First();
            TqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, TlProvider);
            AssessmentSeries = AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null, true);
            TlLookup = TlLookupDataProvider.CreateTlLookupList(DbContext, null, true);
            PathwayComponentGrades = TlLookup.Where(x => x.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToList();
            SpecialismComponentGrades = TlLookupDataProvider.CreateSpecialismGradeTlLookupList(DbContext, null, true);
            DbContext.SaveChangesAsync();
        }

        public List<TqRegistrationProfile> SeedRegistrationsData(Dictionary<long, RegistrationPathwayStatus> ulns, TqProvider tqProvider = null)
        {
            var profiles = new List<TqRegistrationProfile>();

            foreach (var uln in ulns)
            {
                profiles.Add(SeedRegistrationData(uln.Key, uln.Value, tqProvider));
            }
            return profiles;
        }

        public TqRegistrationProfile SeedRegistrationData(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());
            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                tqRegistrationPathway.Status = status;
                tqRegistrationPathway.EndDate = DateTime.UtcNow.AddDays(-1);
                tqRegistrationSpecialism.IsOptedin = true;
                tqRegistrationSpecialism.EndDate = DateTime.UtcNow.AddDays(-1);
            }

            DbContext.SaveChanges();
            return profile;
        }

        public List<TqPathwayAssessment> SeedPathwayAssessmentsData(List<TqPathwayAssessment> pathwayAssessments, bool saveChanges = true)
        {
            var tqPathwayAssessment = PathwayAssessmentDataProvider.CreateTqPathwayAssessments(DbContext, pathwayAssessments);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqPathwayAssessment;
        }

        public List<TqPathwayResult> SeedPathwayResultsData(List<TqPathwayResult> pathwayResults, bool saveChanges = true)
        {
            var tqPathwayResults = TqPathwayResultDataProvider.CreateTqPathwayResults(DbContext, pathwayResults);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqPathwayResults;
        }

        public List<TqPathwayAssessment> GetPathwayAssessmentsDataToProcess(List<TqRegistrationPathway> pathwayRegistrations, bool seedPathwayAssessmentsAsActive = true, bool isHistorical = false)
        {
            var tqPathwayAssessments = new List<TqPathwayAssessment>();

            foreach (var (pathwayRegistration, index) in pathwayRegistrations.Select((value, i) => (value, i)))
            {
                if (isHistorical)
                {
                    // Historical record
                    var pathwayAssessment = new TqPathwayAssessmentBuilder().Build(pathwayRegistration, AssessmentSeries[index]);
                    pathwayAssessment.IsOptedin = false;
                    pathwayAssessment.EndDate = DateTime.UtcNow.AddDays(-1);

                    var tqPathwayAssessmentHistorical = PathwayAssessmentDataProvider.CreateTqPathwayAssessment(DbContext, pathwayAssessment);
                    tqPathwayAssessments.Add(tqPathwayAssessmentHistorical);
                }

                var activePathwayAssessment = new TqPathwayAssessmentBuilder().Build(pathwayRegistration, AssessmentSeries[index]);
                var tqPathwayAssessment = PathwayAssessmentDataProvider.CreateTqPathwayAssessment(DbContext, activePathwayAssessment);
                if (!seedPathwayAssessmentsAsActive)
                {
                    tqPathwayAssessment.IsOptedin = pathwayRegistration.Status == RegistrationPathwayStatus.Withdrawn;
                    tqPathwayAssessment.EndDate = DateTime.UtcNow;
                }

                tqPathwayAssessments.Add(tqPathwayAssessment);
            }
            return tqPathwayAssessments;
        }

        public List<TqPathwayResult> GetPathwayResultsDataToProcess(List<TqPathwayAssessment> pathwayAssessments, bool seedPathwayResultsAsActive = true, bool isHistorical = false, PrsStatus? prsStatus = null)
        {
            var tqPathwayResults = new List<TqPathwayResult>();

            foreach (var (pathwayAssessment, index) in pathwayAssessments.Select((value, i) => (value, i)))
            {
                if (isHistorical)
                {
                    // Historical record
                    var pathwayResult = new TqPathwayResultBuilder().Build(pathwayAssessment);
                    pathwayResult.IsOptedin = false;
                    pathwayResult.EndDate = DateTime.UtcNow.AddDays(-1);

                    var tqPathwayResultHistorical = TqPathwayResultDataProvider.CreateTqPathwayResult(DbContext, pathwayResult);
                    tqPathwayResults.Add(tqPathwayResultHistorical);
                }

                var activePathwayResult = new TqPathwayResultBuilder().Build(pathwayAssessment);
                var tqPathwayResult = TqPathwayResultDataProvider.CreateTqPathwayResult(DbContext, activePathwayResult);
                if (!seedPathwayResultsAsActive)
                {
                    tqPathwayResult.IsOptedin = pathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn;
                    tqPathwayResult.EndDate = DateTime.UtcNow;
                }
                else
                {
                    tqPathwayResult.PrsStatus = prsStatus;
                }
                tqPathwayResults.Add(tqPathwayResult);
            }
            return tqPathwayResults;
        }

        public List<TqPathwayResult> GetPathwayResultsDataToProcess(List<TqPathwayAssessment> pathwayAssessments)
        {
            var tqPathwayResults = new List<TqPathwayResult>();

            foreach (var (pathwayAssessment, index) in pathwayAssessments.Select((value, i) => (value, i)))
            {
                var pathwayResult = new TqPathwayResultBuilder().Build(pathwayAssessment, PathwayComponentGrades[index]);
                var tqPathwayResult = TqPathwayResultDataProvider.CreateTqPathwayResult(DbContext, pathwayResult);
                tqPathwayResults.Add(tqPathwayResult);
            }
            return tqPathwayResults;
        }

        public List<TqSpecialismAssessment> GetSpecialismAssessmentsDataToProcess(List<TqRegistrationSpecialism> specialismsRegistrations, bool seedSpecialismAssessmentsAsActive = true, bool isHistorical = false)
        {
            var tqSpecialismAssessments = new List<TqSpecialismAssessment>();

            foreach (var (specialismRegistration, index) in specialismsRegistrations.Select((value, i) => (value, i)))
            {
                if (isHistorical)
                {
                    // Historical record
                    var specialismAssessment = new TqSpecialismAssessmentBuilder().Build(specialismRegistration, AssessmentSeries[index]);
                    specialismAssessment.IsOptedin = false;
                    specialismAssessment.EndDate = DateTime.UtcNow.AddDays(-1);

                    var tqSpecialismAssessmentHistorical = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, specialismAssessment);
                    tqSpecialismAssessments.Add(tqSpecialismAssessmentHistorical);
                }

                var activeSpecialismAssessment = new TqSpecialismAssessmentBuilder().Build(specialismRegistration, AssessmentSeries[index]);
                var tqSpecialismAssessment = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, activeSpecialismAssessment);
                if (!seedSpecialismAssessmentsAsActive)
                {
                    tqSpecialismAssessment.IsOptedin = specialismRegistration.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn;
                    tqSpecialismAssessment.EndDate = DateTime.UtcNow;
                }

                tqSpecialismAssessments.Add(tqSpecialismAssessment);
            }
            return tqSpecialismAssessments;
        }

        public List<TqSpecialismAssessment> SeedSpecialismAssessmentsData(List<TqSpecialismAssessment> specialismAssessments, bool saveChanges = true)
        {
            var tqSpecialismAssessments = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessments(DbContext, specialismAssessments);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqSpecialismAssessments;
        }

        public List<TqSpecialismResult> GetSpecialismResultsDataToProcess(List<TqSpecialismAssessment> specialismAssessments)
        {
            var tqSpecialismResults = new List<TqSpecialismResult>();

            foreach (var (specialismAssessment, index) in specialismAssessments.Select((value, i) => (value, i)))
            {
                var specialismResult = new TqSpecialismResultBuilder().Build(specialismAssessment, SpecialismComponentGrades[0]);
                var tqPathwayResult = TqSpecialismResultDataProvider.CreateTqSpecialismResult(DbContext, specialismResult);
                tqSpecialismResults.Add(tqPathwayResult);
            }
            return tqSpecialismResults;
        }

        public List<TqSpecialismResult> GetSpecialismResultsDataToProcess(List<TqSpecialismAssessment> specialismAssessments, bool seedSpecialismResultsAsActive = true, bool isHistorical = false, PrsStatus? prsStatus = null)
        {
            var tqSpecialismResults = new List<TqSpecialismResult>();

            foreach (var (specialismAssessment, index) in specialismAssessments.Select((value, i) => (value, i)))
            {
                if (isHistorical)
                {
                    // Historical record
                    var specialismResult = new TqSpecialismResultBuilder().Build(specialismAssessment);
                    specialismResult.IsOptedin = false;
                    specialismResult.EndDate = DateTime.UtcNow.AddDays(-1);

                    var tqSpecialismResultHistorical = TqSpecialismResultDataProvider.CreateTqSpecialismResult(DbContext, specialismResult);
                    tqSpecialismResults.Add(tqSpecialismResultHistorical);
                }

                var activeSpecialismResult = new TqSpecialismResultBuilder().Build(specialismAssessment);
                var tqSpecialismResult = TqSpecialismResultDataProvider.CreateTqSpecialismResult(DbContext, activeSpecialismResult);
                if (!seedSpecialismResultsAsActive)
                {
                    tqSpecialismResult.IsOptedin = specialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn;
                    tqSpecialismResult.EndDate = DateTime.UtcNow;
                }
                else
                {
                    tqSpecialismResult.PrsStatus = prsStatus;
                }
                tqSpecialismResults.Add(tqSpecialismResult);
            }
            return tqSpecialismResults;
        }

        public List<TqSpecialismResult> SeedSpecialismResultsData(List<TqSpecialismResult> specialismResults, bool saveChanges = true)
        {
            var tqSpecialismResults = TqSpecialismResultDataProvider.CreateTqSpecialismResults(DbContext, specialismResults);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqSpecialismResults;
        }
    }
}
