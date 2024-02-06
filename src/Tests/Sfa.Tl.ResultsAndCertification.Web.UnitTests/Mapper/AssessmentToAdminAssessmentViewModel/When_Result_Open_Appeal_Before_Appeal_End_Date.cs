using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System;
using Xunit;
using LearnerRecord = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.LearnerRecord;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Mapper.AssessmentToAdminAssessmentViewModel
{
    public class When_Result_Open_Appeal_Before_Appeal_End_Date : AdminDashboardMapperTestBase
    {
        private readonly int _registrationPathwayId = 1;
        private readonly DateTime _today = new(2024, 2, 2);

        private static readonly Assessment _assessment = new()
        {
            SeriesName = "Summer 2023",
            ComponentType = ComponentType.Core,
            RommEndDate = new(2024, 2, 1),
            AppealEndDate = new(2024, 2, 10),
            Result = new Result
            {
                Id = 1,
                Grade = "A",
                GradeCode = "PCG2",
                PrsStatus = PrsStatus.Reviewed,
                LastUpdatedOn = new DateTime(2023, 12, 31),
                LastUpdatedBy = "Steve Morris"
            }
        };

        public override void Setup()
        {
            Setup(_registrationPathwayId, _today, _assessment);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();

            Result.RegistrationPathwayId.Should().Be(RegistrationPathwayId);
            Result.ExamPeriod.Should().Be(_assessment.SeriesName);
            Result.Grade.Should().Be(_assessment.Result.Grade);
            Result.PrsDisplayText.Should().BeEmpty();
            Result.LastUpdated.Should().Be(_assessment.Result.LastUpdatedOn.ToDobFormat());
            Result.UpdatedBy.Should().Be(_assessment.Result.LastUpdatedBy);
            Result.IsResultChangeAllowed.Should().BeTrue();

            Result.ActionButton.Should().NotBeNull();
            Result.ActionButton.Text.Should().Be(LearnerRecord.Action_Button_Open_Appeal);
        }
    }
}