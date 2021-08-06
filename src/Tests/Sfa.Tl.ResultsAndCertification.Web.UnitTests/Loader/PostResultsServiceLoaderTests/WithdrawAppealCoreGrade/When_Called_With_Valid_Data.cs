using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.WithdrawAppealCoreGrade
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private readonly bool _expectedApiResult = true;
        public override void Given()
        {
            ViewModel = new AppealOutcomePathwayGradeViewModel
            {
                ProfileId = 1,
                PathwayAssessmentId = 2,
                PathwayResultId = 3,
            };

            InternalApiClient.AppealGradeAsync(Arg.Is<AppealGradeRequest>(x =>
                                x.ProfileId == ViewModel.ProfileId &&
                                x.AssessentId == ViewModel.PathwayAssessmentId &&
                                x.ResultId == ViewModel.PathwayResultId &&
                                x.ComponentType == ComponentType.Core &&
                                x.PrsStatus == PrsStatus.Withdraw &&
                                x.AoUkprn == AoUkprn &&
                                x.PerformedBy == $"{Givenname} {Surname}"
                                ))
                .Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_True_Returned()
        {
            ActualResult.Should().BeTrue();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            InternalApiClient.Received(1).AppealGradeAsync(Arg.Is<AppealGradeRequest>(x =>
                                x.ProfileId == ViewModel.ProfileId &&
                                x.AssessentId == ViewModel.PathwayAssessmentId &&
                                x.ResultId == ViewModel.PathwayResultId &&
                                x.ComponentType == ComponentType.Core &&
                                x.PrsStatus == PrsStatus.Withdraw &&
                                x.ResultLookupId == 0 &&
                                x.AoUkprn == AoUkprn &&
                                x.PerformedBy == $"{Givenname} {Surname}"
                                ));
        }
    }
}