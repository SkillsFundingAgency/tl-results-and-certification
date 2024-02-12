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
    public class When_Called_With_Valid_Data : AdminDashboardLoaderTestsBase
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

            _result.RegistrationPathwayId.Should().Be(_apiResultLearnerRecord.RegistrationPathwayId);
            _result.Uln.Should().Be(_apiResultLearnerRecord.Uln);
            _result.LearnerName.Should().Be($"{_apiResultLearnerRecord.Firstname} {_apiResultLearnerRecord.Lastname}");
            _result.TlevelName.Should().Be(_apiResultLearnerRecord.Pathway.Name);
            _result.Provider.Should().Be($"{_apiResultLearnerRecord.Pathway.Provider.Name} ({_apiResultLearnerRecord.Pathway.Provider.Ukprn})");
            _result.StartYear.Should().Be(_apiResultLearnerRecord.Pathway.AcademicYear);
        }

        private List<AssessmentSeriesDetails> CreateAssessmentSeries()
        {
            return new List<AssessmentSeriesDetails>
            {
                new AssessmentSeriesDetails
                {
                    Id = 6,
                    ComponentType = Common.Enum.ComponentType.Core,
                    Name = "Autumn 2023",
                    Description = "Autumn 2023",
                    Year = 2023,
                    StartDate = DateTime.Parse("2023/08/08"),
                    EndDate = DateTime.Parse("2024/03/11")
                }
            };
        }
    }
}