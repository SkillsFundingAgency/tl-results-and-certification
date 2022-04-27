using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.UpdateLearnerSubjectAsync
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private bool _expectedApiResult;

        public override void Given()
        {
            CreateMapper();
            ProviderUkprn = 987654321;

            AddEnglishStatusViewModel = new AddEnglishStatusViewModel
            {
                ProfileId = 1,
                LearnerName = "John Smith",
                IsAchieved = true,
            };

            _expectedApiResult = true;

            InternalApiClient.UpdateLearnerSubjectAsync(Arg.Is<UpdateLearnerSubjectRequest>(
                    x => x.ProfileId == AddEnglishStatusViewModel.ProfileId &&
                    x.SubjectStatus == SubjectStatus.Achieved &&
                    x.SubjectType == SubjectType.English &&
                    x.PerformedBy == $"{Givenname} {Surname}"))
                .Returns(_expectedApiResult);

            Loader = new TrainingProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
