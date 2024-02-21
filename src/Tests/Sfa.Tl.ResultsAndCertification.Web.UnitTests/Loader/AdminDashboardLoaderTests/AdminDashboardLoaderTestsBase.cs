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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests
{
    public abstract class AdminDashboardLoaderTestsBase : BaseTest<AdminDashboardLoader>
    {
        protected IResultsAndCertificationInternalApiClient ApiClient;
        protected AdminDashboardLoader Loader;

        public override void Setup()
        {
            ApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            var mapper = CreateMapper();

            Loader = new AdminDashboardLoader(ApiClient, mapper);
        }

        private AutoMapper.Mapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AdminDashboardMapper).Assembly));
            return new AutoMapper.Mapper(mapperConfig);
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

        protected AdminLearnerRecord CreateAdminLearnerRecord(int registrationPathwayId, int pathwayAssessmentId)
        {
            var learnerRecord = CreateAdminLearnerRecord(registrationPathwayId);

            learnerRecord.Pathway.PathwayAssessments = new Assessment[]
            {
                new Assessment
                {
                    Id = pathwayAssessmentId,
                    SeriesId = 1,
                    SeriesName = "Autum 2023",
                    ResultEndDate = new DateTime(2024, 1, 1),
                    RommEndDate = new DateTime(2024, 2, 1),
                    AppealEndDate = new DateTime(2024, 3, 1),
                    LastUpdatedOn = new DateTime(2023, 9, 15),
                    LastUpdatedBy = "test-user",
                    ComponentType = ComponentType.Core,
                    Result = new Result
                    {
                        Id = 4567,
                        Grade = "A",
                        GradeCode = "PCG2",
                        PrsStatus = PrsStatus.NotSpecified,
                        LastUpdatedOn = new DateTime(2023, 12, 31),
                        LastUpdatedBy = "test-user"
                    }
                }
            };

            return learnerRecord;
        }
    }
}