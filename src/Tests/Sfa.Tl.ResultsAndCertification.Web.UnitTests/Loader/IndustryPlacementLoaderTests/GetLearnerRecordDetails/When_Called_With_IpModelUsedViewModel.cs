using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetLearnerRecordDetails
{
    public class When_Called_With_IpModelUsedViewModel : TestSetup
    {
        private Models.Contracts.TrainingProvider.LearnerRecordDetails _expectedApiResult;
        protected IpModelUsedViewModel ActualResult { get; set; }

        public override void Given()
        {
            ProviderUkprn = 9874561231;
            ProfileId = 1;
            PathwayId = 20;

            _expectedApiResult = new Models.Contracts.TrainingProvider.LearnerRecordDetails
            {
                ProfileId = ProfileId,
                RegistrationPathwayId = PathwayId,
                Uln = 123456789,
                Name = "Test user",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College",
                ProviderUkprn = ProviderUkprn,
                TlevelTitle = "Course name (4561237)",
                AcademicYear = 2020,
                AwardingOrganisationName = "Pearson",
                MathsStatus = Common.Enum.SubjectStatus.Achieved,
                EnglishStatus = Common.Enum.SubjectStatus.NotSpecified,
                IsLearnerRegistered = true,
                IndustryPlacementId = 0,
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed
            };
            InternalApiClient.GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId).Returns(_expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetLearnerRecordDetailsAsync<IpModelUsedViewModel>(ProviderUkprn, ProfileId);
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
            ActualResult.IsIpModelUsed.Should().BeNull();
            ActualResult.IsValid.Should().BeTrue();
        }
    }
}
