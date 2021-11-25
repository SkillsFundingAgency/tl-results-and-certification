using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetActiveAssessmentEntryDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private LearnerRecord _expectedApiLearnerResult;

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

            ExpectedApiResult = new AssessmentEntryDetails
            {
                ProfileId = 1,
                Uln = 1234567890,
                AssessmentId = AssessmentId,
                AssessmentSeriesName = "Summer 2022"
            };
                        
            InternalApiClient.GetActiveAssessmentEntryDetailsAsync(AoUkprn, AssessmentId, componentType).Returns(ExpectedApiResult);
            InternalApiClient.GetLearnerRecordAsync(AoUkprn, ProfileId).Returns(_expectedApiLearnerResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
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

            // Assessment details info
            ActualResult.AssessmentId.Should().Be(ExpectedApiResult.AssessmentId);
            ActualResult.AssessmentSeriesName.Should().Be(ExpectedApiResult.AssessmentSeriesName.ToLowerInvariant());
        }
    }
}
