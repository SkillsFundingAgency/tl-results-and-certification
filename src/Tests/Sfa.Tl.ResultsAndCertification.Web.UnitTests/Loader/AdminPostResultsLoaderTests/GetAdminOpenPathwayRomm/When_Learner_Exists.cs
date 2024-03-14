using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminPostResultsLoaderTests.GetAdminOpenPathwayRomm
{
    public class When_Learner_Exists : TestSetup
    {
        private AdminLearnerRecord _apiResult;

        public override void Given()
        {
            _apiResult = CreateAdminLearnerRecordWithPathwayAssessment(RegistrationPathwayId, PathwayAssessmentId);
            ApiClient.GetAdminLearnerRecordAsync(RegistrationPathwayId).Returns(_apiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetAdminLearnerRecordAsync(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();

            Assessment pathwayAssessment = _apiResult.Pathway.PathwayAssessments.First();
            Provider provider = _apiResult.Pathway.Provider;

            Result.RegistrationPathwayId.Should().Be(RegistrationPathwayId);
            Result.PathwayAssessmentId.Should().Be(pathwayAssessment.Id);
            Result.PathwayName.Should().Be($"{_apiResult.Pathway.Name} ({_apiResult.Pathway.LarId})");

            Result.Learner.Should().Be($"{_apiResult.Firstname} {_apiResult.Lastname}");
            Result.Uln.Should().Be(_apiResult.Uln);
            Result.Provider.Should().Be($"{provider.Name} ({provider.Ukprn})");
            Result.Tlevel.Should().Be(_apiResult.Pathway.Name);
            Result.StartYear.Should().Be($"{_apiResult.Pathway.AcademicYear} to {_apiResult.Pathway.AcademicYear + 1}");

            Result.SummaryLearner.Id.Should().Be(AdminAddPathwayResult.Summary_Learner_Id);
            Result.SummaryLearner.Title.Should().Be(AdminAddPathwayResult.Summary_Learner_Text);
            Result.SummaryLearner.Value.Should().Be(Result.Learner);

            Result.SummaryUln.Id.Should().Be(AdminAddPathwayResult.Summary_ULN_Id);
            Result.SummaryUln.Title.Should().Be(AdminAddPathwayResult.Summary_ULN_Text);
            Result.SummaryUln.Value.Should().Be(Result.Uln.ToString());

            Result.SummaryProvider.Id.Should().Be(AdminAddPathwayResult.Summary_Provider_Id);
            Result.SummaryProvider.Title.Should().Be(AdminAddPathwayResult.Summary_Provider_Text);
            Result.SummaryProvider.Value.Should().Be(Result.Provider);

            Result.SummaryTlevel.Id.Should().Be(AdminAddPathwayResult.Summary_TLevel_Id);
            Result.SummaryTlevel.Title.Should().Be(AdminAddPathwayResult.Summary_TLevel_Text);
            Result.SummaryTlevel.Value.Should().Be(Result.Tlevel);

            Result.SummaryStartYear.Id.Should().Be(AdminAddPathwayResult.Summary_StartYear_Id);
            Result.SummaryStartYear.Title.Should().Be(AdminAddPathwayResult.Summary_StartYear_Text);
            Result.SummaryStartYear.Value.Should().Be(Result.StartYear);

            Result.ExamPeriod.Should().Be(pathwayAssessment.SeriesName);
            Result.Grade.Should().Be(pathwayAssessment.Result.Grade);

            Result.SummaryExamPeriod.Id.Should().Be(AdminAddPathwayResult.Summary_Exam_Period_Id);
            Result.SummaryExamPeriod.Title.Should().Be(AdminAddPathwayResult.Summary_Exam_Period_Text);
            Result.SummaryExamPeriod.Value.Should().Be(Result.ExamPeriod);

            Result.SummaryGrade.Id.Should().Be(AdminAddPathwayResult.Summary_Grade_Id);
            Result.SummaryGrade.Title.Should().Be(AdminAddPathwayResult.Summary_Grade_Text);
            Result.SummaryGrade.Value.Should().Be(Result.Grade);

            Result.DoYouWantToOpenRomm.Should().NotHaveValue();

            BackLinkModel backLink = Result.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.AdminLearnerRecord);
            backLink.RouteAttributes.Should().BeEquivalentTo(new Dictionary<string, string>
            {
                [Constants.PathwayId] = RegistrationPathwayId.ToString()
            });
        }
    }
}