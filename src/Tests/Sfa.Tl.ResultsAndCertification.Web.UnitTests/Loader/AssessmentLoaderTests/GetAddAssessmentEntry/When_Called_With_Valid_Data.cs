using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetAddAssessmentEntry
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private LearnerRecord _expectedApiLearnerResult;
        private AvailableAssessmentSeries _expectedApiAvailableAssessmentSeries;
        
        public override void Given()
        {
            _expectedApiLearnerResult = new LearnerRecord
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
                }
            };

            _expectedApiAvailableAssessmentSeries = new AvailableAssessmentSeries 
            {
                ProfileId = 1,
                AssessmentSeriesId = 11, 
                AssessmentSeriesName = "Summer 2021"
            };

            ComponentIds = _expectedApiLearnerResult.Pathway.Id.ToString();
            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId).Returns(_expectedApiLearnerResult);
            InternalApiClient.GetAvailableAssessmentSeriesAsync(AoUkprn, ProfileId, ComponentType.Core, ComponentIds).Returns(_expectedApiAvailableAssessmentSeries);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Should().NotBeNull();

            // Profile info
            ActualResult.ProfileId.Should().Be(_expectedApiLearnerResult.ProfileId);
            ActualResult.Uln.Should().Be(_expectedApiLearnerResult.Uln);
            ActualResult.Firstname.Should().Be(_expectedApiLearnerResult.Firstname);
            ActualResult.Lastname.Should().Be(_expectedApiLearnerResult.Lastname);
            ActualResult.DateofBirth.Should().Be(_expectedApiLearnerResult.DateofBirth);
            ActualResult.ProviderName.Should().Be(_expectedApiLearnerResult.Pathway.Provider.Name);
            ActualResult.ProviderUkprn.Should().Be(_expectedApiLearnerResult.Pathway.Provider.Ukprn);
            ActualResult.TlevelTitle.Should().Be(_expectedApiLearnerResult.Pathway.Title);

            // Series Info
            ActualResult.AssessmentSeriesId.Should().Be(_expectedApiAvailableAssessmentSeries.AssessmentSeriesId);
            ActualResult.AssessmentSeriesName.Should().Be(_expectedApiAvailableAssessmentSeries.AssessmentSeriesName.ToLower());
        }
    }
}
