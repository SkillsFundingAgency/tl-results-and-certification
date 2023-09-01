using AutoMapper;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.CoreRommExtract;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Mapper.TqRegistrationPathwayToCoreRommExtractData
{
    public abstract class TestSetup : BaseTest<IMapper>
    {
        protected IMapper Mapper;
        protected TqRegistrationPathway Source;
        protected CoreRommExtractData Destination;

        public override void Setup()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(CoreRommExtractMapper).Assembly));
            Mapper = mapperConfig.CreateMapper();

            Source = InitialiseSource();
        }

        public override Task When()
        {
            Destination = Mapper.Map<CoreRommExtractData>(Source);
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
                TqPathwayAssessments = new TqPathwayAssessment[]
                {
                    new TqPathwayAssessment
                    {
                        IsOptedin = true,
                        AssessmentSeries = new AssessmentSeries
                        {
                            Year = 2023
                        },
                        TqPathwayResults = new List<TqPathwayResult>()
                    }
                }
            };
        }

        protected void SetSourceResults(TqPathwayResult[] results)
        {
            TqPathwayAssessment assessment = Source.TqPathwayAssessments.Single();

            foreach (TqPathwayResult result in results)
            {
                assessment.TqPathwayResults.Add(result);
            }
        }

        protected TqPathwayResult CreateTqPathwayResult(int id, string grade, bool isOptedin = true, PrsStatus? prsStatus = null, DateTime? createdOn = null)
        {
            return new TqPathwayResult
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
            Destination.AssessmentSeries.Should().Be(Source.TqPathwayAssessments.Single().AssessmentSeries.Name);
            Destination.AoName.Should().Be(Source.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.Name);
            Destination.CoreCode.Should().Be(Source.TqProvider.TqAwardingOrganisation.TlPathway.LarId);
        }
    }
}