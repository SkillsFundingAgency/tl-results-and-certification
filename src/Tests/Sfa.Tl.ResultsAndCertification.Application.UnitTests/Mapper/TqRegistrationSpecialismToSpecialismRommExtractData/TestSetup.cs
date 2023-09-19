using AutoMapper;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.SpecialRommExtraction;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.TqRegistrationSpecialismToSpecialismRommExtractData
{
    public abstract class TestSetup : BaseTest<IMapper>
    {
        protected IMapper Mapper;
        protected TqRegistrationSpecialism Source;
        protected SpecialRommExtractionData Destination;


        public override void Setup()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(SpecialismRommExtractMapper).Assembly));
            Mapper = mapperConfig.CreateMapper();

            Source = InitialiseSource();
        }

        public override Task When()
        {
            Destination = Mapper.Map<SpecialRommExtractionData>(Source);
            return Task.CompletedTask;
        }

        private TqRegistrationSpecialism InitialiseSource()
        {
            return new TqRegistrationSpecialism()
            {
                TlSpecialism = new TlSpecialism() {
                    Id = 1,
                    LarId = "The_Lar_Id",
                    Name = "Civil Engineering"
                },
                TqRegistrationPathway = new TqRegistrationPathway
                {
                    TqRegistrationProfile = new TqRegistrationProfile
                    {
                        UniqueLearnerNumber = 1234567890,
                    },
                    AcademicYear = 2023,
                    TqProvider = new TqProvider
                    {
                        TqAwardingOrganisation = new TqAwardingOrganisation
                        {
                            TlAwardingOrganisaton = new TlAwardingOrganisation
                            {
                                Name = "The_Awarding_Organisation_Name"
                            },
                            TlPathway = new TlPathway
                            {
                                LarId = "The_Lar_Id"
                            }
                        }
                    },
                    TqRegistrationSpecialisms = new TqRegistrationSpecialism[]
                    {
                        new TqRegistrationSpecialism(){
                            
                            TlSpecialismId = 1,
                            TlSpecialism = new TlSpecialism()
                            {
                                Id = 1,
                                LarId = "The_Lar_Id",
                                Name = "Civil Engineering"
                            },
                            TqSpecialismAssessments = new TqSpecialismAssessment[]
                            {
                                new TqSpecialismAssessment()
                                {
                                    AssessmentSeries = new AssessmentSeries()
                                    {
                                        Name = "Summer 2023"
                                    },
                                    TqSpecialismResults = new List<TqSpecialismResult>()
                                }
                            }
                        }
                    }
                },
                TqSpecialismAssessments = new TqSpecialismAssessment[]
                 {
                    new TqSpecialismAssessment()
                    {
                        AssessmentSeries = new AssessmentSeries()
                        {
                            Name = "Autumn 2023"
                        },
                        TqSpecialismResults = new List<TqSpecialismResult>()
                    },
                    new TqSpecialismAssessment()
                    {
                        AssessmentSeries = new AssessmentSeries()
                        {
                            Name = "Summer 2023"
                        },
                        TqSpecialismResults = new List<TqSpecialismResult>()
                    }
                },
                TlSpecialismId = 1
            };
        }

        protected void SetSourceResults(TqSpecialismResult[] results)
        {
            TqSpecialismAssessment assessment = Source.TqSpecialismAssessments.FirstOrDefault();

            foreach (TqSpecialismResult result in results)
            {
                assessment.TqSpecialismResults.Add(result);
            }
        }

        protected TqSpecialismResult CreateTqSpecialismResult(int id, string grade, bool isOptedin = true, PrsStatus? prsStatus = null, DateTime? createdOn = null)
        {
            return new TqSpecialismResult
            {
                Id = id,
                PrsStatus = prsStatus,
                IsOptedin = isOptedin,
                TlLookup = new TlLookup
                {
                    Value = grade
                },
                CreatedOn = createdOn ?? DateTime.UtcNow
            };
        }

        protected void AssertDirectPropertyMappings()
        {
            Destination.UniqueLearnerNumber.Should().Be(Source.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber);
            Destination.StudentStartYear.Should().Be(Source.TqRegistrationPathway.AcademicYear);
            Destination.AssessmentSeries.Should().Be(Source.TqRegistrationPathway.TqRegistrationSpecialisms.SelectMany(p => p.TqSpecialismAssessments).Single().AssessmentSeries.Name);
            Destination.AoName.Should().Be(Source.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.Name);
            Destination.SpecialismCode.Should().Be(Source.TqRegistrationPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId);
        }
    }
}