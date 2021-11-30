using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetAssessmentDetails
{
    public class When_Called_With_AssessmentUlnWithdrawnViewModel : TestSetup
    {
        private AssessmentUlnWithdrawnViewModel _actualResult;
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
                    AcademicYear = 2021,
                    Status = RegistrationPathwayStatus.Active,
                    Provider = new Provider
                    {
                        Id = 1,
                        Ukprn = 456123987,
                        Name = "Provider Name",
                        DisplayName = "Provider display name",
                    },
                    PathwayAssessments = new List<Assessment>
                    {
                        new Assessment
                        {
                            Id = 3,
                            SeriesId = 1,
                            SeriesName = "Summer 2021",
                            AppealEndDate = System.DateTime.UtcNow.AddDays(10),
                            LastUpdatedBy = "System",
                            LastUpdatedOn = System.DateTime.UtcNow,
                            Results = new List<Result>
                            {
                                new Result
                                {
                                    Id = 1,
                                    Grade = "A",
                                    PrsStatus = null,
                                    LastUpdatedBy = "System",
                                    LastUpdatedOn = System.DateTime.UtcNow
                                }
                            }
                        }
                    },
                    Specialisms = new List<Specialism>
                    {
                        new Specialism
                        {
                            Id = 5,
                            LarId = "ZT2158963",
                            Name = "Specialism Name",
                            Assessments = new List<Assessment>
                            {
                                new Assessment
                                {
                                    Id = 4,
                                    SeriesId = 2,
                                    SeriesName = "Autumn 2021",
                                    AppealEndDate = System.DateTime.UtcNow.AddDays(30),
                                    LastUpdatedBy = "System",
                                    LastUpdatedOn = System.DateTime.UtcNow,
                                    Results = new List<Result>()
                                }
                            }
                        }
                    },
                    IndustryPlacements = new List<IndustryPlacement>
                    {
                        new IndustryPlacement
                        {
                            Id = 7,
                            Status = IndustryPlacementStatus.Completed
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
                    Name = "Summer 2021",
                    Description = "Summer 2021",
                    StartDate = System.DateTime.UtcNow.AddDays(-1),
                    EndDate = System.DateTime.UtcNow.AddDays(10),
                    AppealEndDate = System.DateTime.UtcNow.AddDays(20),
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
            _actualResult = await Loader.GetAssessmentDetailsAsync<AssessmentUlnWithdrawnViewModel>(AoUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();

            _actualResult.ProfileId.Should().Be(expectedApiResult.ProfileId);
            _actualResult.Uln.Should().Be(expectedApiResult.Uln);
            _actualResult.Firstname.Should().Be(expectedApiResult.Firstname);
            _actualResult.Lastname.Should().Be(expectedApiResult.Lastname);
            _actualResult.DateofBirth.Should().Be(expectedApiResult.DateofBirth);
            _actualResult.ProviderName.Should().Be(expectedApiResult.Pathway.Provider.Name);
            _actualResult.ProviderUkprn.Should().Be(expectedApiResult.Pathway.Provider.Ukprn);
            _actualResult.TlevelTitle.Should().Be(expectedApiResult.Pathway.Title);
            _actualResult.ProviderDisplayName.Should().Be($"{expectedApiResult.Pathway.Provider.Name}<br/>({expectedApiResult.Pathway.Provider.Ukprn})");
        }
    }
}
