using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System;
using System.Collections.Generic;
using System.Security.Claims;

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

            var config = new ResultsAndCertificationConfiguration
            {
                DocumentRerequestInDays = 21,
            };

            Loader = new AdminDashboardLoader(ApiClient, mapper, config);
        }

        private static AutoMapper.Mapper CreateMapper()
        {
            string Givenname = "test";
            string Surname = "user";
            string Email = "test.user@test.com";

            IHttpContextAccessor httpContextAccessor = Substitute.For<IHttpContextAccessor>();

            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, Givenname),
                    new Claim(ClaimTypes.Surname, Surname),
                    new Claim(ClaimTypes.Email, Email)
                }))
            });

            var mapperConfig = new MapperConfiguration(c =>
                {
                    c.AddMaps(typeof(AdminDashboardMapper).Assembly);

                    c.ConstructServicesUsing(type =>
                       {
                           if (type.Equals(typeof(UserNameResolver<ReviewChangeStartYearViewModel, ReviewChangeStartYearRequest>)))
                           {
                               return new UserNameResolver<ReviewChangeStartYearViewModel, ReviewChangeStartYearRequest>(httpContextAccessor);
                           }
                           else if (type.Equals(typeof(UserNameResolver<AdminReviewChangesIndustryPlacementViewModel, ReviewChangeIndustryPlacementRequest>)))
                           {
                               return new UserNameResolver<AdminReviewChangesIndustryPlacementViewModel, ReviewChangeIndustryPlacementRequest>(httpContextAccessor);
                           }
                           else if (type.Equals(typeof(UserNameResolver<AdminAddPathwayResultReviewChangesViewModel, AddPathwayResultRequest>)))
                           {
                               return new UserNameResolver<AdminAddPathwayResultReviewChangesViewModel, AddPathwayResultRequest>(httpContextAccessor);
                           }
                           else if (type.Equals(typeof(UserNameResolver<AdminAddSpecialismResultReviewChangesViewModel, AddSpecialismResultRequest>)))
                           {
                               return new UserNameResolver<AdminAddSpecialismResultReviewChangesViewModel, AddSpecialismResultRequest>(httpContextAccessor);
                           }
                           else if (type.Equals(typeof(UserNameResolver<AdminReviewChangesSpecialismAssessmentViewModel, ReviewAddSpecialismAssessmentRequest>)))
                           {
                               return new UserNameResolver<AdminReviewChangesSpecialismAssessmentViewModel, ReviewAddSpecialismAssessmentRequest>(httpContextAccessor);
                           }
                           else if (type.Equals(typeof(UserNameResolver<AdminReviewChangesCoreAssessmentViewModel, ReviewAddCoreAssessmentRequest>)))
                           {
                               return new UserNameResolver<AdminReviewChangesCoreAssessmentViewModel, ReviewAddCoreAssessmentRequest>(httpContextAccessor);
                           }
                           else
                           {
                               return null;
                           }
                       });
                });

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

        protected AdminLearnerRecord CreateAdminLearnerRecordWithPathwayAssessment(int registrationPathwayId, int pathwayAssessmentId)
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
                    ComponentType = ComponentType.Core
                }
            };

            return learnerRecord;
        }

        protected AdminLearnerRecord CreateAdminLearnerRecordWithSpecialismAssessment(int registrationPathwayId, int specialismAssessmentId)
        {
            var learnerRecord = CreateAdminLearnerRecord(registrationPathwayId);

            learnerRecord.Pathway.Specialisms = new[]
            {
                new Specialism
                {
                    Id = 1,
                    LarId = "ZTLOS001",
                    Name = "Surveying and Design for Construction and the Built Environment",
                    Assessments = new[]
                    {
                        new Assessment
                        {
                            Id = specialismAssessmentId,
                            SeriesId = 1,
                            SeriesName = "Autum 2023",
                            ResultEndDate = new DateTime(2024, 1, 1),
                            RommEndDate = new DateTime(2024, 2, 1),
                            AppealEndDate = new DateTime(2024, 3, 1),
                            LastUpdatedOn = new DateTime(2023, 9, 15),
                            LastUpdatedBy = "test-user",
                            ComponentType = ComponentType.Core
                    }
                }
                }
            };

            return learnerRecord;
        }

        protected List<LookupData> CreatePathwayGrades()
        {
            return new List<LookupData>
            {
                new LookupData
                {
                    Id = 1,
                    Code = "PCG1",
                    Value = "A*"
                },
                new LookupData
                {
                    Id = 2,
                    Code = "PCG2",
                    Value = "A"
                },
                new LookupData
                {
                    Id = 3,
                    Code = "PCG3",
                    Value = "B"
                },
                new LookupData
                {
                    Id = 4,
                    Code = "PCG4",
                    Value = "C"
                },
                new LookupData
                {
                    Id = 5,
                    Code = "PCG5",
                    Value = "D"
                },
                new LookupData
                {
                    Id = 6,
                    Code = "PCG6",
                    Value = "E"
                },
                new LookupData
                {
                    Id = 7,
                    Code = "PCG7",
                    Value = "Unclassified"
                },
                new LookupData
                {
                    Id = 25,
                    Code = "PCG8",
                    Value = "Q - pending result"
                },
                new LookupData
                {
                    Id = 26,
                    Code = "PCG9",
                    Value = "X - no result"
                }
            };
        }

        protected List<LookupData> CreateSpecialismGrades()
        {
            return new List<LookupData>
            {
                new LookupData
                {
                    Id = 10,
                    Code = "SCG1",
                    Value = "Distinction"
                },
                new LookupData
                {
                    Id = 11,
                    Code = "SCG2",
                    Value = "Merit"
                },
                new LookupData
                {
                    Id = 12,
                    Code = "SCG3",
                    Value = "Pass"
                },
                new LookupData
                {
                    Id = 13,
                    Code = "SCG4",
                    Value = "Unclassified"
                },
                new LookupData
                {
                    Id = 14,
                    Code = "SCG5",
                    Value = "Q - pending result"
                },
                new LookupData
                {
                    Id = 15,
                    Code = "SCG6",
                    Value = "X - no result"
                }
            };
        }
    }
}