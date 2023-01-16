using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetIndustryPlacementViewModel
{
    public class When_Status_IsNot_CompletedWithSpecialConsideration : TestSetup
    {
        private Models.Contracts.TrainingProvider.LearnerRecordDetails _expectedApiResult;
        public List<SummaryItemModel> _expectedSummaryDetails;

        public override void Given() 
        {
            _expectedApiResult = new Models.Contracts.TrainingProvider.LearnerRecordDetails
            {
                ProfileId = ProfileId,
                TlPathwayId = PathwayId,
                Uln = 123456789,
                Name = "Test user",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College",
                ProviderUkprn = ProviderUkprn,
                TlevelTitle = "Course name (4561237)",
                AcademicYear = 2020,
                AwardingOrganisationName = "Pearson",
                MathsStatus = Common.Enum.SubjectStatus.Achieved,
                EnglishStatus = Common.Enum.SubjectStatus.NotSpecified,
                IsLearnerRegistered = true,
                IndustryPlacementId = 10,
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed,
                IndustryPlacementDetails = "{\"IndustryPlacementStatus\":\"Completed\",\"HoursSpentOnPlacement\":null,\"SpecialConsiderationReasons\":[]}",
            };

            InternalApiClient.GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId, null)
                .Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IpCompletion.Should().NotBeNull();
            
            var ipCompletion = ActualResult.IpCompletion;
            ipCompletion.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ipCompletion.RegistrationPathwayId.Should().Be(_expectedApiResult.ProfileId);
            ipCompletion.PathwayId.Should().Be(_expectedApiResult.ProfileId);
            ipCompletion.AcademicYear.Should().Be(_expectedApiResult.AcademicYear);
            ipCompletion.LearnerName.Should().Be(_expectedApiResult.Name);
            ipCompletion.IndustryPlacementStatus.Should().Be(_expectedApiResult.IndustryPlacementStatus);
            ipCompletion.IsChangeJourney.Should().BeFalse();
            ipCompletion.IsChangeMode.Should().BeFalse();

            ActualResult.SpecialConsideration.Should().BeNull();
        }
    }
}
