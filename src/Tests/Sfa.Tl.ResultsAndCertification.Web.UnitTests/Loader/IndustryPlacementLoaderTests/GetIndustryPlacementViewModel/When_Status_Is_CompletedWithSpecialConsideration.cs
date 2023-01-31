using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetIndustryPlacementViewModel
{
    public class When_Status_Is_CompletedWithSpecialConsideration : TestSetup
    {
        private Models.Contracts.TrainingProvider.LearnerRecordDetails _expectedApiResult;
        private List<IpLookupData> _scIpLookupData;
        private List<IpLookupDataViewModel> _expectedReasonsViewModel;

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
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration,
                IndustryPlacementDetails = "{\"IndustryPlacementStatus\":\"Completed\",\"HoursSpentOnPlacement\":99,\"SpecialConsiderationReasons\":[1,2,4]}",
            };

            InternalApiClient.GetLearnerRecordDetailsAsync(ProviderUkprn, ProfileId, null)
                .Returns(_expectedApiResult);

            _scIpLookupData = new List<IpLookupData>
            {
                new IpLookupData { Id = 1, Name = "Reason 1" },
                new IpLookupData { Id = 2, Name = "Reason 2" },
                new IpLookupData { Id = 4, Name = "Reason 4" }
            };
            InternalApiClient.GetIpLookupDataAsync(Common.Enum.IpLookupType.SpecialConsideration).Returns(_scIpLookupData);

            _expectedReasonsViewModel = new List<IpLookupDataViewModel>
            {
                new IpLookupDataViewModel { Id = 1, Name = "Reason 1", IsSelected = true },
                new IpLookupDataViewModel { Id = 2, Name = "Reason 2", IsSelected = true },
                new IpLookupDataViewModel { Id = 4, Name = "Reason 4", IsSelected = true }
            };
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

            ActualResult.SpecialConsideration.Should().NotBeNull();
            var ipSpecialConsiderationHours = ActualResult.SpecialConsideration.Hours;
            ipSpecialConsiderationHours.Hours.Should().Be("99");
            ipSpecialConsiderationHours.IsChangeMode.Should().BeFalse();
            ipSpecialConsiderationHours.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ipSpecialConsiderationHours.LearnerName.Should().Be(_expectedApiResult.Name);

            var ipSpecialConsiderationReasons = ActualResult.SpecialConsideration.Reasons;
            ipSpecialConsiderationReasons.LearnerName.Should().Be(_expectedApiResult.Name); 
            ipSpecialConsiderationReasons.AcademicYear.Should().Be(_expectedApiResult.AcademicYear);
            ipSpecialConsiderationReasons.IsChangeMode.Should().BeFalse();
            ipSpecialConsiderationReasons.IsReasonSelected.Should().BeTrue();
            ipSpecialConsiderationReasons.ReasonsList.Should().BeEquivalentTo(_expectedReasonsViewModel);
        }
    }
}
