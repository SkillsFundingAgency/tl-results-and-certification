using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.AppealCoreGrade
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private readonly bool _expectedApiResult = true;

        public override void Given()
        {
            //AppealCoreGradeViewModel = new PrsAddAppealViewModel
            //{
            //    ProfileId = 1,
            //    PathwayAssessmentId = 2,
            //    PathwayResultId = 3
            //};

            //InternalApiClient.PrsActivityAsync(Arg.Is<PrsActivityRequest>(x =>
            //                    x.ProfileId == AppealCoreGradeViewModel.ProfileId &&
            //                    x.AssessentId == AppealCoreGradeViewModel.PathwayAssessmentId &&
            //                    x.ResultId == AppealCoreGradeViewModel.PathwayResultId &&
            //                    x.ComponentType == ComponentType.Core &&
            //                    x.PrsStatus == PrsStatus.BeingAppealed &&
            //                    x.AoUkprn == AoUkprn &&
            //                    x.PerformedBy == $"{Givenname} {Surname}"
            //                    ))
            //    .Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_True_Returned()
        {
            //ActualResult.Should().BeTrue();
        }
    }
}