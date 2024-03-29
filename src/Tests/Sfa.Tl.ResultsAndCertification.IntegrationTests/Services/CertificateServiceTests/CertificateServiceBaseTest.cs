﻿using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.CertificateServiceTests
{
    public abstract class CertificateServiceBaseTest : BaseTest<OverallResult>
    {
        protected TlRoute Route;
        protected TlPathway Pathway;
        protected TlSpecialism Specialism;
        protected IList<TlSpecialism> Specialisms;
        protected TlProvider TlProvider;
        protected TlAwardingOrganisation TlAwardingOrganisation;
        protected TqAwardingOrganisation TqAwardingOrganisation;
        protected TqProvider TqProvider;
        protected IList<TlProvider> TlProviders;
        protected TlProviderAddress TlProviderAddress;
        protected IList<TqProvider> TqProviders;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected CertificateService CertificateService;

        protected virtual void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, Route);
            Specialism = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, Pathway).First();
            TqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TlProviderAddress = TlProviderAddressDataProvider.CreateTlProviderAddress(DbContext, new TlProviderAddressBuilder().Build(TlProvider));
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, TlProvider);
            TlProviders = new List<TlProvider> { TlProvider };
            TqProviders = new List<TqProvider> { TqProvider };

            DbContext.SaveChanges();
        }

        protected virtual void CreateService()
        {
            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
            {
                CertificatePrintingBatchSettings = new CertificatePrintingBatchSettings
                {
                    ProvidersBatchSize = 2
                }
            };

            var overallResultLogger = new Logger<GenericRepository<OverallResult>>(new NullLoggerFactory());
            var overallResultRepository = new GenericRepository<OverallResult>(overallResultLogger, DbContext);

            var certificateRepositoryLogger = new Logger<CertificateRepository>(new NullLoggerFactory());
            var certificateRepository = new CertificateRepository(certificateRepositoryLogger, DbContext);
 
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(CertificateMapper).Assembly));
            var mapper = new Mapper(mapperConfig);

            // Create Service class to test. 
            CertificateService = new CertificateService(ResultsAndCertificationConfiguration, overallResultRepository, certificateRepository, mapper);
        }

        public List<TqRegistrationProfile> SeedRegistrationsData(Dictionary<long, RegistrationPathwayStatus> ulns, TqProvider tqProvider = null, bool isCouplet = false)
        {
            var profiles = new List<TqRegistrationProfile>();

            foreach (var uln in ulns)
            {
                profiles.Add(SeedRegistrationData(uln.Key, uln.Value, tqProvider, isCouplet));
            }
            return profiles;
        }

        public TqRegistrationProfile SeedRegistrationData(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null, bool isCouplet = false)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProvider);
            var tqRegistrationSpecialisms = isCouplet ? RegistrationsDataProvider.CreateTqRegistrationSpecialisms(DbContext, tqRegistrationPathway)
                : new List<TqRegistrationSpecialism> { RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism) };

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                tqRegistrationPathway.Status = status;
                tqRegistrationPathway.EndDate = DateTime.UtcNow.AddDays(-1);
                foreach (var tqRegistrationSpecialism in tqRegistrationSpecialisms)
                {
                    tqRegistrationSpecialism.IsOptedin = true;
                    tqRegistrationSpecialism.EndDate = DateTime.UtcNow.AddDays(-1);
                }
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

        public void SeedTqProvider(Provider provider)
        {
            var pro = new TlProviderBuilder().BuildList().FirstOrDefault(x => x.UkPrn == (long)provider);
            var tlProvider = ProviderDataProvider.CreateTlProvider(DbContext, pro);
            TlProviders.Add(tlProvider);
            TlProviderAddressDataProvider.CreateTlProviderAddress(DbContext, new TlProviderAddressBuilder().Build(tlProvider));
            TqProviders.Add(ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, tlProvider));

            DbContext.SaveChanges();
        }

        public void SetRegistrationProviders(List<TqRegistrationProfile> registrations, List<long> ulns, Provider provider)
        {
            foreach (var uln in ulns)
            {
                var reg = registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln);
                reg.TqRegistrationPathways.FirstOrDefault().TqProvider = TqProviders.FirstOrDefault(x => x.TlProvider.UkPrn == (long)provider);
            }

            DbContext.SaveChanges();
        }

        public static void AssertTlProvider(TlProvider actualTlProvider, TlProvider expectedTlProvider)
        {
            actualTlProvider.Name.Should().Be(expectedTlProvider.Name);
            actualTlProvider.DisplayName.Should().Be(expectedTlProvider.DisplayName);
            actualTlProvider.IsActive.Should().Be(expectedTlProvider.IsActive);
            actualTlProvider.CreatedBy.Should().Be(expectedTlProvider.CreatedBy);
            actualTlProvider.CreatedOn.Should().Be(expectedTlProvider.CreatedOn);
        }

        public static void AssertOverallResult(OverallResult actualOverallResult, OverallResult expectedOverallResult)
        {
            actualOverallResult.TqRegistrationPathwayId.Should().Be(expectedOverallResult.TqRegistrationPathwayId);
            actualOverallResult.Details.Should().Be(expectedOverallResult.Details);
            actualOverallResult.ResultAwarded.Should().Be(expectedOverallResult.ResultAwarded);
            actualOverallResult.CalculationStatus.Should().Be(expectedOverallResult.CalculationStatus);
            actualOverallResult.PrintAvailableFrom.Should().Be(expectedOverallResult.PrintAvailableFrom);
            actualOverallResult.PublishDate.Should().Be(expectedOverallResult.PublishDate);
            if (expectedOverallResult.EndDate == null)
            {
                actualOverallResult.IsOptedin.Should().BeTrue();
                actualOverallResult.EndDate.Should().BeNull();
            }
            else
            {
                actualOverallResult.IsOptedin.Should().BeFalse();
                actualOverallResult.EndDate.Should().NotBeNull();
            }
            actualOverallResult.CertificateType.Should().Be(expectedOverallResult.CertificateType);
            actualOverallResult.CertificateStatus.Should().Be(expectedOverallResult.CertificateStatus);
        }

        public static void AssertProfile(TqRegistrationProfile actualProfile, TqRegistrationProfile expectedProfile)
        {
            actualProfile.Firstname.Should().Be(expectedProfile.Firstname);
            actualProfile.Lastname.Should().Be(expectedProfile.Lastname);
            actualProfile.DateofBirth.Should().Be(expectedProfile.DateofBirth);
            actualProfile.Gender.Should().Be(expectedProfile.Gender);
            actualProfile.UniqueLearnerNumber.Should().Be(expectedProfile.UniqueLearnerNumber);
            actualProfile.MathsStatus.Should().Be(expectedProfile.MathsStatus);
            actualProfile.EnglishStatus.Should().Be(expectedProfile.EnglishStatus);
            actualProfile.IsLearnerVerified.Should().Be(expectedProfile.IsLearnerVerified);
            actualProfile.EnglishStatus.Should().Be(expectedProfile.EnglishStatus);
            actualProfile.CreatedOn.Should().Be(expectedProfile.CreatedOn);
            actualProfile.CreatedBy.Should().Be(expectedProfile.CreatedBy);
        }

        public enum Provider
        {
            BarsleyCollege = 10000536,
            BishopBurtonCollege = 10000721,
            WalsallCollege = 10007315            
        }
    }
}
