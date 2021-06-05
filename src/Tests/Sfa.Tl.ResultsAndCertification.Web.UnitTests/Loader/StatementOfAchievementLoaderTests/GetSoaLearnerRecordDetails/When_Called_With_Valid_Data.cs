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
            _address = new Address { AddressId = 1, DepartmentName = "Operations", OrganisationName = "College Ltd", AddressLine1 = "10, House", AddressLine2 = "Street", Town = "Birmingham", Postcode = "B1 1AA" };

            _expectedApiResult = new SoaLearnerRecordDetails
            {
                ProfileId = 99,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Now.AddYears(-20),
                ProviderName = "Barsley College",
                ProviderUkprn = 879456123,

                TlevelTitle = "Design, Surveying and Planning for Construction",
                RegistrationPathwayId = 1,
                PathwayName = "Design, Surveying and Planning for Construction",
                PathwayCode = "60358300",
                PathwayGrade = "A*",
                SpecialismName = "Building Services Design",
                SpecialismCode = "ZTLOS003",
                SpecialismGrade = "None",

                IsEnglishAndMathsAchieved = false,
                HasLrsEnglishAndMaths = true,
                IsSendLearner = false,
                IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration,

                ProviderAddress = _address,
                Status = RegistrationPathwayStatus.Withdrawn
            };

            InternalApiClient.GetSoaLearnerRecordDetailsAsync(ProviderUkprn, ProfileId).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {

            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.LearnerName.Should().Be($"{_expectedApiResult.Firstname} {_expectedApiResult.Lastname}");
            ActualResult.DateofBirth.Should().Be(_expectedApiResult.DateofBirth);
            ActualResult.ProviderDisplayName.Should().Be($"{_expectedApiResult.ProviderName} ({_expectedApiResult.ProviderUkprn})");
            ActualResult.ProviderName.Should().Be(_expectedApiResult.ProviderName);
            ActualResult.ProviderUkprn.Should().Be(_expectedApiResult.ProviderUkprn);

            ActualResult.TlevelTitle.Should().Be(_expectedApiResult.TlevelTitle);
            ActualResult.RegistrationPathwayId.Should().Be(_expectedApiResult.RegistrationPathwayId);
            ActualResult.PathwayDisplayName.Should().Be($"{_expectedApiResult.PathwayName} ({_expectedApiResult.PathwayCode})");
            ActualResult.PathwayName.Should().Be(_expectedApiResult.PathwayName);
            ActualResult.PathwayCode.Should().Be(_expectedApiResult.PathwayCode);
            ActualResult.PathwayGrade.Should().Be(_expectedApiResult.PathwayGrade);
            ActualResult.SpecialismDisplayName.Should().Be($"{_expectedApiResult.SpecialismName} ({_expectedApiResult.SpecialismCode})");
            ActualResult.SpecialismName.Should().Be(_expectedApiResult.SpecialismName);
            ActualResult.SpecialismCode.Should().Be(_expectedApiResult.SpecialismCode);
            ActualResult.SpecialismGrade.Should().Be(_expectedApiResult.SpecialismGrade);

            ActualResult.IsEnglishAndMathsAchieved.Should().Be(_expectedApiResult.IsEnglishAndMathsAchieved);
            ActualResult.HasLrsEnglishAndMaths.Should().Be(_expectedApiResult.HasLrsEnglishAndMaths);
            ActualResult.IsSendLearner.Should().Be(_expectedApiResult.IsSendLearner);
            ActualResult.IndustryPlacementStatus.Should().Be(_expectedApiResult.IndustryPlacementStatus);
            ActualResult.ProviderAddress.Should().BeEquivalentTo(_address);

            // validation properties
            ActualResult.HasPathwayResult.Should().BeTrue();
            ActualResult.IsIndustryPlacementAdded.Should().BeTrue();
            ActualResult.IsLearnerRegistered.Should().BeTrue();
            ActualResult.IsNotWithdrawn.Should().BeFalse();
            ActualResult.IsIndustryPlacementCompleted.Should().BeTrue();
        }
    }
}