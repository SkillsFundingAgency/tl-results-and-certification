using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.PrsGradeChangeRequest
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private readonly bool _expectedApiResult = true;

        public override void Given()
        {
            ViewModel = new PrsGradeChangeRequestViewModel
            {
                Firstname = "John",
                Lastname = "Smith",
                Uln = 1234567890,
                ProviderUkprn = 10000536,
                ComponentType = ComponentType.Core,
                CoreName = "Digital Production, Design and Development",
                ExamPeriod = "Summer 2024",
                Grade = "A",
                ChangeRequestData = "Test"
            };

            InternalApiClient.PrsGradeChangeRequestAsync(Arg.Is<Models.Contracts.PostResultsService.PrsGradeChangeRequest>(x =>
                x.LearnerName == ViewModel.LearnerName &&
                x.Uln == ViewModel.Uln &&
                x.ProviderUkprn == ViewModel.ProviderUkprn &&
                x.ComponentType == ViewModel.ComponentType &&
                x.ComponentName == ViewModel.CoreName &&
                x.ExamPeriod == ViewModel.ExamPeriod &&
                x.Grade == ViewModel.Grade &&
                x.RequestedMessage == ViewModel.ChangeRequestData &&
                x.RequestedUserEmailAddress == Email))
             .Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            InternalApiClient.Received(1).PrsGradeChangeRequestAsync(Arg.Is<Models.Contracts.PostResultsService.PrsGradeChangeRequest>(x =>
                x.LearnerName == ViewModel.LearnerName &&
                x.Uln == ViewModel.Uln &&
                x.ProviderUkprn == ViewModel.ProviderUkprn &&
                x.ComponentType == ViewModel.ComponentType &&
                x.ComponentName == ViewModel.CoreName &&
                x.ExamPeriod == ViewModel.ExamPeriod &&
                x.Grade == ViewModel.Grade &&
                x.RequestedMessage == ViewModel.ChangeRequestData &&
                x.RequestedUserEmailAddress == Email));
        }

        [Fact]
        public void Then_True_Returned()
        {
            ActualResult.Should().BeTrue();
        }
    }
}