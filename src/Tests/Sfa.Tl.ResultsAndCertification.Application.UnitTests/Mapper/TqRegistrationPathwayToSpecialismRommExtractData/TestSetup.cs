using AutoMapper;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.CoreRommExtract;
using Sfa.Tl.ResultsAndCertification.Models.SpecialRommExtraction;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.TqRegistrationPathwayToSpecialismRommExtractData
{
    public abstract class TestSetup : BaseTest<IMapper>
    {
        protected IMapper Mapper;
        protected TqRegistrationPathway Source;
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

        private TqRegistrationPathway InitialiseSource()
        {
            return new TqRegistrationPathway
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
                    new TqRegistrationSpecialism()
                    {
                         TqSpecialismAssessments = new TqSpecialismAssessment[]
                         {
                             new TqSpecialismAssessment()
                             {
                                  AssessmentSeries = new AssessmentSeries()
                                  {
                                      Name = "Autumn 2023"
                                  },
                                  TqSpecialismResults = new List<TqSpecialismResult>()
                             }
                        }
                    }
                }
            };
        }

        protected void SetSourceResults(TqSpecialismResult[] results)
        {
            TqSpecialismAssessment assessment = Source.TqRegistrationSpecialisms.SelectMany(p => p.TqSpecialismAssessments).FirstOrDefault();

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
            Destination.UniqueLearnerNumber.Should().Be(Source.TqRegistrationProfile.UniqueLearnerNumber);
            Destination.StudentStartYear.Should().Be(Source.AcademicYear);
            Destination.AssessmentSeries.Should().Be(Source.TqRegistrationSpecialisms.SelectMany(p => p.TqSpecialismAssessments).Single().AssessmentSeries.Name);
            Destination.AoName.Should().Be(Source.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.Name);
            Destination.SpecialismCode.Should().Be(Source.TqProvider.TqAwardingOrganisation.TlPathway.LarId);
        }
    }
}