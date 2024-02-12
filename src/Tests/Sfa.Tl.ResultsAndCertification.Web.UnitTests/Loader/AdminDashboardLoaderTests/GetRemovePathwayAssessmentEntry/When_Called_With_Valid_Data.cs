using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.GetRemovePathwayAssessmentEntry
{
    public class When_Called_With_Valid_Data : AdminDashboardLoaderTestsBase
    {
        private const int RegistrationPathwayId = 1;
        private const int PathwayAssessmentId = 125;

        private AdminLearnerRecord _apiResult;
        private AdminRemovePathwayAssessmentEntryViewModel _result;

        public override void Given()
        {
            _apiResult = CreateAdminLearnerRecord(RegistrationPathwayId, PathwayAssessmentId);
            ApiClient.GetAdminLearnerRecordAsync(RegistrationPathwayId).Returns(_apiResult);
        }

        public async override Task When()
        {
            _result = await Loader.GetRemovePathwayAssessmentEntryAsync(RegistrationPathwayId, PathwayAssessmentId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetAdminLearnerRecordAsync(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();

            Assessment pathwayAssessment = _apiResult.Pathway.PathwayAssessments.First();
            Provider provider = _apiResult.Pathway.Provider;

            _result.RegistrationPathwayId.Should().Be(RegistrationPathwayId);
            _result.PathwayAssessmentId.Should().Be(PathwayAssessmentId);
            _result.PathwayName.Should().Be(_apiResult.Pathway.Name);

            _result.Learner.Should().Be($"{_apiResult.Firstname} {_apiResult.Lastname}");
            _result.Uln.Should().Be(_apiResult.Uln);
            _result.Provider.Should().Be($"{provider.Name} ({provider.Ukprn})");
            _result.Tlevel.Should().Be(_apiResult.Pathway.Name);
            _result.StartYear.Should().Be($"{_apiResult.Pathway.AcademicYear} to {_apiResult.Pathway.AcademicYear + 1}");

            _result.SummaryLearner.Value.Should().Be()


            _result.AwardingOrganisationName.Should().Be(_apiResult.AwardingOrganisation.DisplayName);
            _result.MathsStatus.Should().Be(_apiResult.MathsStatus);
            _result.EnglishStatus.Should().Be(_apiResult.EnglishStatus);
            _result.RegistrationPathwayStatus.Should().Be(_apiResult.Pathway.Status);
            _result.IsLearnerRegistered.Should().BeTrue();
            _result.IndustryPlacementId.Should().Be(_apiResult.Pathway.IndustryPlacements.Single().Id);
            _result.IndustryPlacementStatus.Should().Be(_apiResult.Pathway.IndustryPlacements.Single().Status);
        }

        /*
         *   public int RegistrationPathwayId { get; set; }


        public SummaryItemModel SummaryLearner
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_Learner_Id, RemoveAssessmentEntryCore.Summary_Learner_Text, Learner);

        public SummaryItemModel SummaryUln
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_ULN_Id, RemoveAssessmentEntryCore.Summary_ULN_Text, Uln.ToString());

        public SummaryItemModel SummaryProvider
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_Provider_Id, RemoveAssessmentEntryCore.Summary_Provider_Text, Provider);

        public SummaryItemModel SummaryTlevel
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_TLevel_Id, RemoveAssessmentEntryCore.Summary_TLevel_Text, Tlevel);

        public SummaryItemModel SummaryStartYear
            => CreateSummaryItemModel(RemoveAssessmentEntryCore.Summary_StartYear_Id, RemoveAssessmentEntryCore.Summary_StartYear_Text, StartYear);

        private static SummaryItemModel CreateSummaryItemModel(string id, string title, string value)
             => new()
             {
                 Id = id,
                 Title = title,
                 Value = value
             };

        #endregion

        #region Assessment

        public string ExamPeriod { get; set; }

        public string Grade { get; set; }

        public string LastUpdated { get; set; }

        public string UpdatedBy { get; set; }

        #endregion

        public bool CanAssessmentEntryBeRemoved { get; set; }

        [Required(ErrorMessageResourceType = typeof(RemoveAssessmentEntryCore), ErrorMessageResourceName = "Validation_Message")]
        public bool? DoYouWantToRemoveThisAssessmentEntry { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminLearnerRecord,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
        };
         */
    }
}