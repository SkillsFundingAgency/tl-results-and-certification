using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminPostResultsLoaderTests
{
    public abstract class AdminPostResultsLoaderBaseTest : BaseTest<AdminPostResultsLoader>
    {
        protected IResultsAndCertificationInternalApiClient ApiClient;
        protected AdminPostResultsLoader Loader;

        public override void Setup()
        {
            ApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AdminChangeLogMapper).Assembly));
            var mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new AdminPostResultsLoader(ApiClient, mapper);
        }

        protected AdminLearnerRecord CreateAdminLearnerRecord(int registrationPathwayId)
        {
            return new AdminLearnerRecord
            {
                RegistrationPathwayId = registrationPathwayId,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = new DateTime(2008, 1, 6),
                MathsStatus = SubjectStatus.Achieved,
                EnglishStatus = SubjectStatus.Achieved,
                OverallCalculationStatus = CalculationStatus.Completed,
                AwardingOrganisation = new AwardingOrganisation
                {
                    Id = 1,
                    Ukprn = 10009696,
                    Name = "Ncfe",
                    DisplayName = "NCFE"
                },
                Pathway = new Pathway
                {
                    Id = 4,
                    LarId = "60358294",
                    Title = "T Level in Education and Early Years",
                    Name = "Education and Early Years",
                    StartYear = 2020,
                    AcademicYear = 2023,
                    Status = RegistrationPathwayStatus.Active,
                    Provider = new Provider
                    {
                        Id = 2,
                        Ukprn = 10000536,
                        Name = "Barnsley College",
                        DisplayName = "Barnsley College"
                    },
                    IndustryPlacements = new IndustryPlacement[]
                    {
                        new IndustryPlacement
                        {
                            Id= 250,
                            Status = IndustryPlacementStatus.Completed
                        }
                    }
                }
            };
        }
    }
}