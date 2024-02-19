using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.GetAdminLearnerRecordWithCoreComponents
{
    public class When_Called_With_Core_Previous_Previous_Entries : AdminDashboardLoaderTestsBase
    {
        private const int RegistrationPathwayId = 1;

        private AdminLearnerRecord _apiResultLearnerRecord;
        private List<AssessmentSeriesDetails> _apiResultAssessmentSeries;
        private AdminCoreComponentViewModel _result;

        public override void Given()
        {
            _apiResultLearnerRecord = CreateAdminLearnerRecord(RegistrationPathwayId);
            ApiClient.GetAdminLearnerRecordAsync(RegistrationPathwayId).Returns(_apiResultLearnerRecord);

            _apiResultAssessmentSeries = CreateAssessmentSeries();
            ApiClient.GetAssessmentSeriesAsync().Returns(_apiResultAssessmentSeries);
        }

        public async override Task When()
        {
            _result = await Loader.GetAdminLearnerRecordWithCoreComponents(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetAdminLearnerRecordAsync(RegistrationPathwayId);
            ApiClient.Received(1).GetAssessmentSeriesAsync();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();

            _result.ValidPathwayAssessmentSeries.Should().NotBeNull();
            _result.ValidPathwayAssessmentSeries.Count().Should().Be(2);
        }

        private List<AssessmentSeriesDetails> CreateAssessmentSeries()
        {
            var _assessmentSeriesStartDate = DateTime.Today.Date;
            var _assessmentSeriesEndtDate = _assessmentSeriesStartDate.AddMonths(1) ;

            var _previousAssessmentSeriesStartDate = _assessmentSeriesEndtDate.AddDays(-1);
            var _previousAssessmentSeriesEndtDate = _previousAssessmentSeriesStartDate.AddMonths(-1) ;


            return new List<AssessmentSeriesDetails>
            {
                new AssessmentSeriesDetails
                {
                    Id = 6,
                    ComponentType = Common.Enum.ComponentType.Core,
                    Name = "Autumn 2023",
                    Description = "Autumn 2023",
                    Year = 2023,
                    StartDate = _assessmentSeriesStartDate,
                    EndDate = _assessmentSeriesEndtDate
                },
                 new AssessmentSeriesDetails
                {
                    Id = 5,
                    ComponentType = Common.Enum.ComponentType.Core,
                    Name = "Summer 2023",
                    Description = "Summer 2023",
                    Year = 2023,
                    StartDate = _previousAssessmentSeriesStartDate,
                    EndDate = _previousAssessmentSeriesEndtDate
                }
            };
        }
    }
}