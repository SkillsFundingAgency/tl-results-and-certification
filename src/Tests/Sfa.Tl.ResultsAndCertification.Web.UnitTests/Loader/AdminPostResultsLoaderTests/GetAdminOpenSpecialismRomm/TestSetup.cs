using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminPostResultsLoaderTests.GetAdminOpenSpecialismRomm
{
    public abstract class TestSetup : AdminPostResultsLoaderBaseTest
    {
        protected const int RegistrationPathwayId = 1;
        protected const int SpecialismAssessmentId = 125;

        protected AdminOpenSpecialismRommViewModel Result { get; private set; }

        public override async Task When()
        {
            Result = await Loader.GetAdminOpenSpecialismRommAsync(RegistrationPathwayId, SpecialismAssessmentId);
        }

        protected AdminLearnerRecord CreateAdminLearnerRecordWithSpecialismAssessment(
            int registrationPathwayId,
            int specialismAssessmentId,
            RegistrationPathwayStatus status,
            string grade,
            string gradeCode)
        {
            var learnerRecord = CreateAdminLearnerRecord(registrationPathwayId, status);

            learnerRecord.Pathway.Specialisms = new[]
            {
                new Specialism
                {
                    Id = 1,
                    LarId = "ZTLOS001",
                    Name = "Surveying and Design for Construction and the Built Environment",
                    Assessments = new[]
                    {
                        new Assessment
                        {
                            Id = specialismAssessmentId,
                            SeriesId = 1,
                            SeriesName = "Autum 2023",
                            ResultEndDate = new DateTime(2024, 1, 1),
                            RommEndDate = new DateTime(2024, 2, 1),
                            AppealEndDate = new DateTime(2024, 3, 1),
                            LastUpdatedOn = new DateTime(2023, 9, 15),
                            LastUpdatedBy = "test-user",
                            ComponentType = ComponentType.Core,
                            Result = new Result
                            {
                                Id = 1,
                                Grade = grade,
                                GradeCode = gradeCode,
                                PrsStatus = PrsStatus.NotSpecified,
                                LastUpdatedOn = new DateTime(2023, 9, 15),
                                LastUpdatedBy = "test-user"
                            }
                        }
                    }
                }
            };

            return learnerRecord;
        }

        protected void AssertResult(AdminLearnerRecord apiResult)
        {
            Result.Should().NotBeNull();

            Specialism specialism = apiResult.Pathway.Specialisms.First(p => p.Assessments.Any(a => a.Id == SpecialismAssessmentId));
            Assessment specialismAssessment = specialism.Assessments.First(a => a.Id == SpecialismAssessmentId);
            Provider provider = apiResult.Pathway.Provider;

            Result.RegistrationPathwayId.Should().Be(RegistrationPathwayId);
            Result.SpecialismAssessmentId.Should().Be(SpecialismAssessmentId);
            Result.SpecialismName.Should().Be($"{specialism.Name} ({specialism.LarId})");

            Result.Learner.Should().Be($"{apiResult.Firstname} {apiResult.Lastname}");
            Result.Uln.Should().Be(apiResult.Uln);
            Result.Provider.Should().Be($"{provider.Name} ({provider.Ukprn})");
            Result.Tlevel.Should().Be(apiResult.Pathway.Name);
            Result.StartYear.Should().Be($"{apiResult.Pathway.AcademicYear} to {apiResult.Pathway.AcademicYear + 1}");

            Result.SummaryLearner.Id.Should().Be(AdminOpenSpecialismRomm.Summary_Learner_Id);
            Result.SummaryLearner.Title.Should().Be(AdminOpenSpecialismRomm.Summary_Learner_Text);
            Result.SummaryLearner.Value.Should().Be(Result.Learner);

            Result.SummaryUln.Id.Should().Be(AdminOpenSpecialismRomm.Summary_ULN_Id);
            Result.SummaryUln.Title.Should().Be(AdminOpenSpecialismRomm.Summary_ULN_Text);
            Result.SummaryUln.Value.Should().Be(Result.Uln.ToString());

            Result.SummaryProvider.Id.Should().Be(AdminOpenSpecialismRomm.Summary_Provider_Id);
            Result.SummaryProvider.Title.Should().Be(AdminOpenSpecialismRomm.Summary_Provider_Text);
            Result.SummaryProvider.Value.Should().Be(Result.Provider);

            Result.SummaryTlevel.Id.Should().Be(AdminOpenSpecialismRomm.Summary_TLevel_Id);
            Result.SummaryTlevel.Title.Should().Be(AdminOpenSpecialismRomm.Summary_TLevel_Text);
            Result.SummaryTlevel.Value.Should().Be(Result.Tlevel);

            Result.SummaryStartYear.Id.Should().Be(AdminOpenSpecialismRomm.Summary_StartYear_Id);
            Result.SummaryStartYear.Title.Should().Be(AdminOpenSpecialismRomm.Summary_StartYear_Text);
            Result.SummaryStartYear.Value.Should().Be(Result.StartYear);

            Result.ExamPeriod.Should().Be(specialismAssessment.SeriesName);
            Result.Grade.Should().Be(specialismAssessment.Result.Grade);

            Result.SummaryExamPeriod.Id.Should().Be(AdminOpenSpecialismRomm.Summary_Exam_Period_Id);
            Result.SummaryExamPeriod.Title.Should().Be(AdminOpenSpecialismRomm.Summary_Exam_Period_Text);
            Result.SummaryExamPeriod.Value.Should().Be(Result.ExamPeriod);

            Result.SummaryGrade.Id.Should().Be(AdminOpenSpecialismRomm.Summary_Grade_Id);
            Result.SummaryGrade.Title.Should().Be(AdminOpenSpecialismRomm.Summary_Grade_Text);
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
