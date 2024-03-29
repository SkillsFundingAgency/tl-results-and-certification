﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetResultWithdrawnViewModel
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = new ResultDetails
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.Now.AddYears(-30),
                TlevelTitle = "Tlevel title",
                ProviderName = "Test Provider",
                ProviderUkprn = 1234567,
                PathwayName = "Pathway",
                PathwayLarId = "7654321",
                PathwayAssessmentSeries = "Summer 2021",
                AppealEndDate = DateTime.Today.AddDays(7),
                PathwayAssessmentId = 11,
                PathwayResultId = 123,
                PathwayResult = "A",
                PathwayPrsStatus = PrsStatus.BeingAppealed,
                Status = RegistrationPathwayStatus.Active
            };

            InternalApiClient.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.Uln.Should().Be(expectedApiResult.Uln);
            ActualResult.Firstname.Should().Be(expectedApiResult.Firstname);
            ActualResult.Lastname.Should().Be(expectedApiResult.Lastname);
            ActualResult.DateofBirth.Should().Be(expectedApiResult.DateofBirth);
            ActualResult.TlevelTitle.Should().Be(expectedApiResult.TlevelTitle);
            ActualResult.ProviderName.Should().Be(expectedApiResult.ProviderName);
            ActualResult.ProviderUkprn.Should().Be(expectedApiResult.ProviderUkprn);
        }
    }
}