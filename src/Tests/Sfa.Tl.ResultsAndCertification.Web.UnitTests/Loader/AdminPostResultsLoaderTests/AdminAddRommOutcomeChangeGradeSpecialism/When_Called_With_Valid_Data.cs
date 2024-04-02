using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminPostResultsLoaderTests;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminPostResultsLoaderTests.AdminAddRommOutcomeChangeGradeSpecialism
{
    public class When_Called_With_Valid_Data : AdminPostResultsLoaderBaseTest
    {
        private const int RegistrationPathwayId = 1;
        private const int SpecialismAssessmentId = 888;

        private AdminLearnerRecord _apiResult;
        private List<LookupData> _grades;

        private AdminAddRommOutcomeChangeGradeSpecialismViewModel _result;

        public override void Given()
        {
            _apiResult = CreateAdminLearnerRecordWithSpecialismAssessment(RegistrationPathwayId, SpecialismAssessmentId);
            _grades = CreateSpecialismGrades();
            _grades = _grades.Where(t => !t.Code.Equals(Constants.SpecialismComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase)
            && !t.Code.Equals(Constants.SpecialismComponentGradeXNoResultCode, StringComparison.InvariantCultureIgnoreCase)
            && !t.Code.Equals(Constants.NotReceived, StringComparison.InvariantCultureIgnoreCase)).ToList();


            ApiClient.GetAdminLearnerRecordAsync(RegistrationPathwayId).Returns(_apiResult);
            ApiClient.GetLookupDataAsync(LookupCategory.SpecialismComponentGrade).Returns(_grades);
        }

        public async override Task When()
        {
            _result = await Loader.GetAdminAddRommOutcomeChangeGradeSpecialismAsync(RegistrationPathwayId, SpecialismAssessmentId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetAdminLearnerRecordAsync(RegistrationPathwayId);
            ApiClient.Received(1).GetLookupDataAsync(LookupCategory.SpecialismComponentGrade);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();

            Specialism specialism = _apiResult.Pathway.Specialisms.First(p => p.Assessments.Any(a => a.Id == SpecialismAssessmentId));
            Assessment specialismAssessment = specialism.Assessments.First(a => a.Id == SpecialismAssessmentId);
            Provider provider = _apiResult.Pathway.Provider;

            _result.RegistrationPathwayId.Should().Be(RegistrationPathwayId);
            _result.SpecialismAssessmentId.Should().Be(SpecialismAssessmentId);
            _result.SpecialismName.Should().Be($"{specialism.Name} ({specialism.LarId})");

            _result.Learner.Should().Be($"{_apiResult.Firstname} {_apiResult.Lastname}");
            _result.Uln.Should().Be(_apiResult.Uln);
            _result.Provider.Should().Be($"{provider.Name} ({provider.Ukprn})");
            _result.Tlevel.Should().Be(_apiResult.Pathway.Name);
            _result.StartYear.Should().Be($"{_apiResult.Pathway.AcademicYear} to {_apiResult.Pathway.AcademicYear + 1}");

            _result.SummaryLearner.Id.Should().Be(AdminAddPathwayResult.Summary_Learner_Id);
            _result.SummaryLearner.Title.Should().Be(AdminAddPathwayResult.Summary_Learner_Text);
            _result.SummaryLearner.Value.Should().Be(_result.Learner);

            _result.SummaryUln.Id.Should().Be(AdminAddPathwayResult.Summary_ULN_Id);
            _result.SummaryUln.Title.Should().Be(AdminAddPathwayResult.Summary_ULN_Text);
            _result.SummaryUln.Value.Should().Be(_result.Uln.ToString());

            _result.SummaryProvider.Id.Should().Be(AdminAddPathwayResult.Summary_Provider_Id);
            _result.SummaryProvider.Title.Should().Be(AdminAddPathwayResult.Summary_Provider_Text);
            _result.SummaryProvider.Value.Should().Be(_result.Provider);

            _result.SummaryTlevel.Id.Should().Be(AdminAddPathwayResult.Summary_TLevel_Id);
            _result.SummaryTlevel.Title.Should().Be(AdminAddPathwayResult.Summary_TLevel_Text);
            _result.SummaryTlevel.Value.Should().Be(_result.Tlevel);

            _result.SummaryStartYear.Id.Should().Be(AdminAddPathwayResult.Summary_StartYear_Id);
            _result.SummaryStartYear.Title.Should().Be(AdminAddPathwayResult.Summary_StartYear_Text);
            _result.SummaryStartYear.Value.Should().Be(_result.StartYear);

            _result.ExamPeriod.Should().Be(specialismAssessment.SeriesName);
            _result.Grade.Should().BeNull();

            _result.SelectedGradeId.Should().BeNull();
            _result.Grades.Should().BeEquivalentTo(_grades);

            BackLinkModel backLink = _result.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.AdminAddCoreRommOutcome);
            backLink.RouteAttributes.Should().BeEquivalentTo(new Dictionary<string, string>
            {
                [Constants.RegistrationPathwayId] = RegistrationPathwayId.ToString(),
                [Constants.AssessmentId] = SpecialismAssessmentId.ToString()
            });
        }
    }
}