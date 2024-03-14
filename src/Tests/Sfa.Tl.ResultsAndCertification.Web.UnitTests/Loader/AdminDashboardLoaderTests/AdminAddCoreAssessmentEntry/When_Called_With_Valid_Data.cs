using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.AdminAddCoreAssessmentEntry
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private bool ExpectedResult { get; set; }
        private bool ExpectedApiResult { get; set; }

        public override void Given()
        {
            ViewModel = new AdminReviewChangesCoreAssessmentViewModel()
            {

                ContactName = "firstname",
                AdminCoreComponentViewModel = new AdminCoreComponentViewModel()
                {
                    Uln = 1100000001,
                    Provider = "provider-name (10000536)",
                    TlevelName = "t-level-name",
                    StartYear = 2022,
                    DisplayStartYear = "2022 to 2023",
                    RegistrationPathwayId = 1,
                    AssessmentYearTo = "Autumn 2023"
                    
                },

                ChangeReason = "change-reason",
                Day = "01",
                Month = "01",
                Year = "1970",
                 
                ZendeskId ="122356761"
                
            };

            
            InternalApiClient
                .ProcessAddCoreAssessmentRequestAsync(Arg.Is<ReviewAddCoreAssessmentRequest>(
                    x => x.RegistrationPathwayId == ViewModel.AdminCoreComponentViewModel.RegistrationPathwayId && 
                    x.ChangeReason == ViewModel.ChangeReason && 
                    x.ChangeType == Common.Enum.ChangeType.AddPathwayAssessment &&
                    x.ContactName == ViewModel.ContactName &&
                    x.RequestDate == Convert.ToDateTime(ViewModel.RequestDate) &&
                    x.ZendeskId == ViewModel.ZendeskId &&
                    x.AddCoreAssessmentDetails.CoreAssessmentTo == ViewModel.AdminCoreComponentViewModel.AssessmentYearTo &&
                    x.AddCoreAssessmentDetails.CoreAssessmentFrom == $"{ReviewChangeAssessment.No_Assessment_Recorded} {ViewModel.AdminCoreComponentViewModel.AssessmentYearTo.ToLower()}"))
                    
                .Returns(true);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeTrue();
            
        }
    }
}
