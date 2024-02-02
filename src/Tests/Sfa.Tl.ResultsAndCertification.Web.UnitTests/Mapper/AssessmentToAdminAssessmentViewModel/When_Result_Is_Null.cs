using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Mapper.AssessmentToAdminAssessmentViewModel
{
    public class When_Result_Is_Null : AdminDashboardMapperTestBase
    {
        private readonly int _registrationPathwayId = 1;
        private readonly DateTime _today = new(2024, 2, 2);

        private static readonly Assessment _assessment = new()
        {
            SeriesName = "Summer 2023",
            ComponentType = ComponentType.Core,
            Result = null
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
            Result.Grade.Should().BeNull();
            Result.PrsDisplayText.Should().BeEmpty();
            Result.LastUpdated.Should().BeNull();
            Result.UpdatedBy.Should().BeNull();
            Result.ActionButton.Should().BeNull();
        }
    }
}