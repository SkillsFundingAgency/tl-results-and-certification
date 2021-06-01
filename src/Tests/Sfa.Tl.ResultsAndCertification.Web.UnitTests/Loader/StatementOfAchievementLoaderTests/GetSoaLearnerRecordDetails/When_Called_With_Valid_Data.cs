using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using System;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.StatementOfAchievementLoaderTests.GetSoaLearnerRecordDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private SoaLearnerRecordDetails _expectedApiResult;
        private Address _address;

        public override void Given()
        {
            _address = new Address { DepartmentName = "Operations", OrganisationName = "College Ltd", AddressLine1 = "10, House", AddressLine2 = "Street", Town = "Birmingham", Postcode = "B1 1AA" };

            _expectedApiResult = new SoaLearnerRecordDetails
            {
                ProfileId = 99,
                Uln = 1234567890,
                LearnerName = "John Smith",
                DateofBirth = DateTime.Now.AddYears(-20),
                ProviderName = "Barsley College",

                TlevelTitle = "Design, Surveying and Planning for Construction",
                PathwayName = "Design, Surveying and Planning for Construction(60358300)",
                PathwayGrade = "A*",
                SpecialismName = "Building Services Design (ZTLOS003)",
                SpecialismGrade = "None",

                IsEnglishAndMathsAchieved = false,
                HasLrsEnglishAndMaths = true,
                IsSendLearner = false,
                IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration,

                ProviderAddress = _address,
                Status = RegistrationPathwayStatus.Withdrawn,
            };

            InternalApiClient.GetSoaLearnerRecordDetailsAsync(ProviderUkprn, ProfileId).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {

            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.LearnerName.Should().Be(_expectedApiResult.LearnerName);
            ActualResult.DateofBirth.Should().Be(_expectedApiResult.DateofBirth);
            ActualResult.ProviderName.Should().Be(_expectedApiResult.ProviderName);

            ActualResult.TlevelTitle.Should().Be(_expectedApiResult.TlevelTitle);
            ActualResult.PathwayName.Should().Be(_expectedApiResult.PathwayName);
            ActualResult.PathwayGrade.Should().Be(_expectedApiResult.PathwayGrade);
            ActualResult.SpecialismName.Should().Be(_expectedApiResult.SpecialismName);
            ActualResult.SpecialismGrade.Should().Be(_expectedApiResult.SpecialismGrade);

            ActualResult.IsEnglishAndMathsAchieved.Should().Be(_expectedApiResult.IsEnglishAndMathsAchieved);
            ActualResult.HasLrsEnglishAndMaths.Should().Be(_expectedApiResult.HasLrsEnglishAndMaths);
            ActualResult.IsSendLearner.Should().Be(_expectedApiResult.IsSendLearner);
            ActualResult.IndustryPlacementStatus.Should().Be(_expectedApiResult.IndustryPlacementStatus);

            // validation properties
            ActualResult.HasPathwayResult.Should().BeTrue();
            ActualResult.IsIndustryPlacementAdded.Should().BeTrue();
            ActualResult.IsLearnerRegistered.Should().BeTrue();
            ActualResult.IsNotWithdrawn.Should().BeFalse();
            ActualResult.IsIndustryPlacementCompleted.Should().BeTrue();
        }
    }
}