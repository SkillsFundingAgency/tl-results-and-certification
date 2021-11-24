﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetAssessmentDetails
{
    public class When_Called_With_Single_Specialism_Series_Open : TestSetup
    {
        private IList<AssessmentSeriesDetails> _assessmentSeriesDetails;

        public override void Given()
        {
            expectedApiResult = new LearnerRecord
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = System.DateTime.UtcNow.AddYears(-29),
                Gender = "M",
                Pathway = new Pathway
                {
                    Id = 2,
                    LarId = "89564123",
                    Name = "Test Pathway",
                    Title = "Test Pathwya title",
                    AcademicYear = 2020,
                    Status = RegistrationPathwayStatus.Active,
                    Provider = new Provider
                    {
                        Id = 1,
                        Ukprn = 456123987,
                        Name = "Provider Name",
                        DisplayName = "Provider display name",
                    },
                    Specialisms = new List<Specialism>
                    {
                        new Specialism
                        {
                            Id = 5,
                            LarId = "ZT2158963",
                            Name = "Specialism Name",
                            Assessments = new List<Assessment>()                            
                        }
                    }
                }
            };

            _assessmentSeriesDetails = new List<AssessmentSeriesDetails>
            {
                new AssessmentSeriesDetails
                {
                    Id = 1,
                    ComponentType = ComponentType.Core,
                    Name = "Autumn 2021",
                    Description = "Autumn 2021",
                    StartDate = System.DateTime.UtcNow.AddDays(10),
                    EndDate = System.DateTime.UtcNow.AddDays(30),
                    AppealEndDate = System.DateTime.UtcNow.AddDays(40),
                    Year = 2021
                },
                new AssessmentSeriesDetails
                {
                    Id = 2,
                    ComponentType = ComponentType.Specialism,
                    Name = "Summer 2022",
                    Description = "Summer 2022",
                    StartDate = System.DateTime.UtcNow.AddDays(-1),
                    EndDate = System.DateTime.UtcNow.AddDays(10),
                    AppealEndDate = System.DateTime.UtcNow.AddDays(20),
                    Year = 2022
                }
            };

            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId).Returns(expectedApiResult);
            InternalApiClient.GetAssessmentSeriesAsync().Returns(_assessmentSeriesDetails);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetAssessmentDetailsAsync<AssessmentDetailsViewModel>(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var coreAssessmentSeriesName = _assessmentSeriesDetails[0].Name;
            var specialismAssessmentSeriesName = _assessmentSeriesDetails[1].Name;
            ActualResult.Should().NotBeNull();

            ActualResult.ProfileId.Should().Be(expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(expectedApiResult.Uln);
            ActualResult.Firstname.Should().Be(expectedApiResult.Firstname);
            ActualResult.Lastname.Should().Be(expectedApiResult.Lastname);
            ActualResult.DateofBirth.Should().Be(expectedApiResult.DateofBirth);
            ActualResult.ProviderName.Should().Be(expectedApiResult.Pathway.Provider.Name);
            ActualResult.ProviderUkprn.Should().Be(expectedApiResult.Pathway.Provider.Ukprn);
            ActualResult.TlevelTitle.Should().Be(expectedApiResult.Pathway.Title);
            ActualResult.PathwayStatus.Should().Be(expectedApiResult.Pathway.Status);

            ActualResult.IsCoreEntryEligible.Should().BeFalse();
            ActualResult.PathwayId.Should().Be(expectedApiResult.Pathway.Id);
            ActualResult.PathwayDisplayName.Should().Be($"{expectedApiResult.Pathway.Name} ({expectedApiResult.Pathway.LarId})");
            ActualResult.NextAvailableCoreSeries.Should().Be(coreAssessmentSeriesName);

            ActualResult.PathwayAssessment.Should().BeNull();
            ActualResult.PreviousPathwayAssessment.Should().BeNull();
            ActualResult.SpecialismDetails.Should().NotBeNullOrEmpty();

            foreach (var expectedSpecialism in expectedApiResult.Pathway.Specialisms)
            {
                var actualSpecialism = ActualResult.SpecialismDetails.FirstOrDefault(s => s.Id == expectedSpecialism.Id);

                actualSpecialism.Should().NotBeNull();

                actualSpecialism.Id.Should().Be(expectedSpecialism.Id);
                actualSpecialism.LarId.Should().Be(expectedSpecialism.LarId);
                actualSpecialism.Name.Should().Be(expectedSpecialism.Name);
                actualSpecialism.DisplayName.Should().Be($"{expectedSpecialism.Name} ({expectedSpecialism.LarId})");
                actualSpecialism.Assessments.Should().BeNullOrEmpty();                
            }

            ActualResult.IsSpecialismEntryEligible.Should().BeTrue();
            ActualResult.HasCurrentSpecialismAssessmentEntry.Should().BeFalse();
            ActualResult.IsResitForSpecialism.Should().BeFalse();
            ActualResult.NextAvailableSpecialismSeries.Should().Be(specialismAssessmentSeriesName);
            ActualResult.IsCoreResultExist.Should().BeFalse();
            ActualResult.HasAnyOutstandingPathwayPrsActivities.Should().BeFalse();
            ActualResult.IsIndustryPlacementExist.Should().BeFalse();

            ActualResult.HasCurrentCoreAssessmentEntry.Should().BeFalse();
            ActualResult.HasResultForCurrentCoreAssessment.Should().BeFalse();
            ActualResult.HasPreviousCoreAssessment.Should().BeFalse();
            ActualResult.HasResultForPreviousCoreAssessment.Should().BeFalse();
            ActualResult.NeedCoreResultForPreviousAssessmentEntry.Should().BeFalse();
            ActualResult.DisplayMultipleSpecialismsCombined.Should().BeFalse();
            ActualResult.IsSpecialismRegistered.Should().BeTrue();
            ActualResult.SpecialismDisplayName.Should().BeNull();
            ActualResult.DisplaySpecialisms.Should().NotBeNullOrEmpty();
            ActualResult.IsResitForSpecialism.Should().BeFalse();
            ActualResult.DisplayMultipleSpecialismsCombined.Should().BeFalse();

            var expectedDisplaySpecialisms = new List<SpecialismViewModel>();
            foreach (var specialism in expectedApiResult.Pathway.Specialisms)
            {
                expectedDisplaySpecialisms.Add(new SpecialismViewModel
                {
                    Id = specialism.Id,
                    LarId = specialism.LarId,
                    Name = specialism.Name,
                    DisplayName = $"{specialism.Name} ({specialism.LarId})"
                });
            }

            ActualResult.DisplaySpecialisms.Count().Should().Be(expectedDisplaySpecialisms.Count());

            foreach (var expectedDisplaySpecialism in expectedDisplaySpecialisms)
            {
                var actualDisplaySpecialism = ActualResult.DisplaySpecialisms.FirstOrDefault(s => s.Id == expectedDisplaySpecialism.Id);

                actualDisplaySpecialism.Should().NotBeNull();
                actualDisplaySpecialism.Id.Should().Be(expectedDisplaySpecialism.Id);
                actualDisplaySpecialism.LarId.Should().Be(expectedDisplaySpecialism.LarId);
                actualDisplaySpecialism.Name.Should().Be(expectedDisplaySpecialism.Name);
                actualDisplaySpecialism.DisplayName.Should().Be($"{expectedDisplaySpecialism.Name} ({expectedDisplaySpecialism.LarId})");
            }
        }
    }
}
