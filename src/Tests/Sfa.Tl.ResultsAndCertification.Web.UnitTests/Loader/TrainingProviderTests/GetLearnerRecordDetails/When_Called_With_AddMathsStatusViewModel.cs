using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.GetLearnerRecordDetails
{
    public class When_Called_With_AddMathsStatusViewModel : TestSetup
    {
        private Models.Contracts.TrainingProvider.LearnerRecordDetails _expectedApiResult;
        protected AddMathsStatusViewModel ActualResult { get; set; }

        public override void Given()
        {
            ProviderUkprn = 9874561231;
            ProfileId = 1;

            _expectedApiResult = new Models.Contracts.TrainingProvider.LearnerRecordDetails
            {
                ProfileId = ProfileId,
                RegistrationPathwayId = 222,
                Uln = 123456789,
                Name = "Test user",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College",
                ProviderUkprn = ProviderUkprn,
                TlevelTitle = "Course name (4561237)",
                AcademicYear = 2020,
                AwardingOrganisationName = "Pearson",
                MathsStatus = Common.Enum.SubjectStatus.NotSpecified,
                EnglishStatus = Common.Enum.SubjectStatus.Achieved,
                IsLearnerRegistered = true,
                IndustryPlacementId = 1,
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed
            };
            InternalApiClient.GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId).Returns(_expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetLearnerRecordDetailsAsync<AddMathsStatusViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.LearnerName.Should().Be(_expectedApiResult.Name);
            ActualResult.IsAchieved.Should().BeNull();
            ActualResult.SubjectStatus.Should().Be(_expectedApiResult.MathsStatus);
            ActualResult.IsValid.Should().BeTrue();
        }
    }
}
