﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.AdminAddSpecialismAssessmentEntry
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            ViewModel = new AdminReviewChangesSpecialismAssessmentViewModel()
            {

                ContactName = "firstname",

                AdminOccupationalSpecialismViewModel = new AdminOccupationalSpecialismViewModel()
                {
                    Uln = 1100000001,
                    Provider = "provider-name (10000536)",
                    TlevelName = "t-level-name",
                    StartYear = 2022,
                    DisplayStartYear = "2022 to 2023",
                    RegistrationPathwayId = 1,
                    AssessmentYearTo = "Autumn 2023",
                    AssessmentSeriesId=6
                    
                },

                ChangeReason = "change-reason",
                Day = "01",
                Month = "01",
                Year = "1970",
                ZendeskId ="122356761"
                
            };

            
            InternalApiClient
                .ProcessAddSpecialismAssessmentRequestAsync(Arg.Is<ReviewAddSpecialismAssessmentRequest>(
                    x => x.RegistrationPathwayId == ViewModel.AdminOccupationalSpecialismViewModel.RegistrationPathwayId && 
                    x.ChangeReason == ViewModel.ChangeReason && 
                    x.ChangeType == Common.Enum.ChangeType.AddAssessment &&
                    x.ContactName == ViewModel.ContactName &&
                    x.RequestDate == $"{ViewModel.Year}/{ViewModel.Month}/{ViewModel.Day}" &&
                    x.ZendeskId == ViewModel.ZendeskId &&
                    x.AddSpecialismDetails.SpecialismAssessmentTo == ViewModel.AdminOccupationalSpecialismViewModel.AssessmentYearTo &&
                    x.AddSpecialismDetails.SpecialismAssessmentFrom == $"No assessment entry recorded for {ViewModel.AdminOccupationalSpecialismViewModel.AssessmentYearTo.ToLower()}" &&
                    x.AddSpecialismDetails.AssessmentSeriesId == ViewModel.AdminOccupationalSpecialismViewModel.AssessmentSeriesId))
                    
                .Returns(true);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeTrue();
            
        }
    }
}
