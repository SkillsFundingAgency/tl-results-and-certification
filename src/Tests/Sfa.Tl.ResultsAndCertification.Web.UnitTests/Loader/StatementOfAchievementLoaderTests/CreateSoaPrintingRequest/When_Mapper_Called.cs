﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.StatementOfAchievementLoaderTests.CreateSoaPrintingRequest
{
    public class When_Mapper_Called : TestSetup
    {
        private LearningDetails _expectedLearningDetails;
        private SoaPrintingDetails _expectedSoaPrintingDetails;

        public override void Given()
        {
            CreateMapper();
            ProviderUkprn = 987654321;

            SoaLearnerRecordDetailsViewModel = new SoaLearnerRecordDetailsViewModel
            {
                ProfileId = 10,
                Uln = 1234567890,
                LearnerName = "John Smith",
                DateofBirth = DateTime.Now.AddYears(-20),
                ProviderName = "Barsley College",
                ProviderUkprn = 456789123,

                TlevelTitle = "T Level in Design, Surveying and Planning for Construction",
                RegistrationPathwayId = 1,
                PathwayDisplayName = "Design, Surveying and Planning for Construction (60358300)",
                PathwayName = "Design, Surveying and Planning for Construction",
                PathwayCode = "60358300",
                PathwayGrade = "A*",
                SpecialismDisplayName = "Building Services Design (ZTLOS003)",
                SpecialismName = "Building Services Design",
                SpecialismCode = "ZTLOS003",
                SpecialismGrade = "None",

                MathsStatus = SubjectStatus.NotSpecified,
                EnglishStatus = SubjectStatus.NotSpecified,
                IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted,

                HasPathwayResult = false,
                IsNotWithdrawn = false,
                IsLearnerRegistered = true,
                IsIndustryPlacementAdded = true,
                IsIndustryPlacementCompleted = false,

                ProviderAddress = new AddressViewModel { AddressId = 10, DepartmentName = "Operations", OrganisationName = "College Ltd", AddressLine1 = "10, House", AddressLine2 = "Street", Town = "Birmingham", Postcode = "B1 1AA" },
            };

            _expectedLearningDetails = new LearningDetails
            {
                TLevelTitle = "Design, Surveying and Planning for Construction",
                Grade = string.Empty,
                Date = DateTime.UtcNow.ToCertificateDateFormat(),
                Core = SoaLearnerRecordDetailsViewModel.PathwayName,
                CoreGrade = SoaLearnerRecordDetailsViewModel.PathwayGrade ?? Constants.NotCompleted,
                OccupationalSpecialism = new List<OccupationalSpecialismDetails>
                {
                    new OccupationalSpecialismDetails
                    {
                        Specialism = SoaLearnerRecordDetailsViewModel.SpecialismName ?? string.Empty,
                        Grade = SoaLearnerRecordDetailsViewModel.SpecialismGrade.Equals("None", StringComparison.InvariantCultureIgnoreCase) ? Constants.NotCompleted : SoaLearnerRecordDetailsViewModel.SpecialismGrade
                    }
                },
                IndustryPlacement = SoaLearnerRecordDetailsViewModel.IsIndustryPlacementCompleted ? Constants.IndustryPlacementCompleted : Constants.IndustryPlacementNotCompleted,
                EnglishAndMaths = string.Empty
            };

            _expectedSoaPrintingDetails = new SoaPrintingDetails
            {
                Uln = SoaLearnerRecordDetailsViewModel.Uln,
                Name = SoaLearnerRecordDetailsViewModel.LearnerName,
                Dateofbirth = SoaLearnerRecordDetailsViewModel.DateofBirth.ToDobFormat(),
                ProviderName = SoaLearnerRecordDetailsViewModel.ProviderDisplayName,
                TlevelTitle = SoaLearnerRecordDetailsViewModel.TlevelTitle,
                Core = SoaLearnerRecordDetailsViewModel.PathwayDisplayName,
                CoreGrade = SoaLearnerRecordDetailsViewModel.PathwayGrade,
                Specialism = SoaLearnerRecordDetailsViewModel.SpecialismDisplayName,
                SpecialismGrade = SoaLearnerRecordDetailsViewModel.SpecialismGrade,
                IndustryPlacement = SoaLearnerRecordDetailsViewModel.GetIndustryPlacementDisplayText,
                ProviderAddress = new Models.Contracts.ProviderAddress.Address
                {
                    AddressId = SoaLearnerRecordDetailsViewModel.ProviderAddress.AddressId,
                    DepartmentName = SoaLearnerRecordDetailsViewModel.ProviderAddress.DepartmentName,
                    OrganisationName = SoaLearnerRecordDetailsViewModel.ProviderAddress.OrganisationName,
                    AddressLine1 = SoaLearnerRecordDetailsViewModel.ProviderAddress.AddressLine1,
                    AddressLine2 = SoaLearnerRecordDetailsViewModel.ProviderAddress.AddressLine2,
                    Town = SoaLearnerRecordDetailsViewModel.ProviderAddress.Town,
                    Postcode = SoaLearnerRecordDetailsViewModel.ProviderAddress.Postcode
                }
            };
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var result = Mapper.Map<SoaPrintingRequest>(SoaLearnerRecordDetailsViewModel, opt => opt.Items["providerUkprn"] = ProviderUkprn);

            result.Should().NotBeNull();
            result.ProviderUkprn.Should().Be(ProviderUkprn);
            result.AddressId.Should().Be(SoaLearnerRecordDetailsViewModel.ProviderAddress.AddressId);
            result.RegistrationPathwayId.Should().Be(SoaLearnerRecordDetailsViewModel.RegistrationPathwayId);
            result.Uln.Should().Be(SoaLearnerRecordDetailsViewModel.Uln);
            result.LearnerName.Should().Be(SoaLearnerRecordDetailsViewModel.LearnerName);
            result.LearningDetails.Should().BeEquivalentTo(_expectedLearningDetails);
            result.SoaPrintingDetails.Should().BeEquivalentTo(_expectedSoaPrintingDetails);
            result.PerformedBy.Should().Be($"{Givenname} {Surname}");
        }
    }
}