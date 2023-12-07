using AutoMapper;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.GetLearnerRecord
{
    public class When_Called_With_Valid_Data : BaseTest<AdminDashboardLoader>
    {
        private IResultsAndCertificationInternalApiClient _internalApiClient;
        private AdminDashboardLoader Loader;

        private AdminSearchLearnerDetailsListViewModel _expectedResult;
        private AdminSearchLearnerDetailsListViewModel _actualResult;
        private int PathwayId;

        public override void Setup()
        {
            _internalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AdminDashboardMapper).Assembly));
            var mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new AdminDashboardLoader(_internalApiClient, mapper);
        }


        private Models.OverallResults.OverallResultDetail _expectedOverallResult;
        private Models.Contracts.AdminDashboard.AdminLearnerRecord _expectedApiResult;

        protected AdminLearnerRecordViewModel ActualResult { get; set; }

        public override void Given()
        {
            PathwayId = 1;



            _expectedApiResult = new Models.Contracts.AdminDashboard.AdminLearnerRecord
            {
                ProfileId = PathwayId,
                RegistrationPathwayId = 222,
                Uln = 786787689,
                Name = "John smith",
                DateofBirth = DateTime.UtcNow.AddYears(-15),
                ProviderName = "Barnsley College",
                TlevelName = "Education and Early Years(60358294)",
                AcademicYear = 2021,
                AwardingOrganisationName = "NCFE",
                MathsStatus = Common.Enum.SubjectStatus.Achieved,
                EnglishStatus = Common.Enum.SubjectStatus.Achieved,
                IsLearnerRegistered = true,
                IndustryPlacementId = 1,
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed
            };
            _internalApiClient.GetAdminLearnerRecordAsync(PathwayId).Returns(_expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetAdminLearnerRecordAsync(PathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            _internalApiClient.Received(1).GetAdminLearnerRecordAsync(PathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.RegistrationPathwayId.Should().Be(_expectedApiResult.RegistrationPathwayId);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.LearnerName.Should().Be(_expectedApiResult.Name);
            ActualResult.DateofBirth.Should().Be(_expectedApiResult.DateofBirth);
            ActualResult.ProviderName.Should().Be(_expectedApiResult.ProviderName);
            ActualResult.ProviderUkprn.Should().Be(_expectedApiResult.ProviderUkprn);
            ActualResult.TlevelName.Should().Be(_expectedApiResult.TlevelName);
            ActualResult.StartYear.Should().Be($"{_expectedApiResult.AcademicYear} to {_expectedApiResult.AcademicYear + 1}");
            ActualResult.AwardingOrganisationName.Should().Be(_expectedApiResult.AwardingOrganisationName);
            ActualResult.MathsStatus.Should().Be(_expectedApiResult.MathsStatus);
            ActualResult.EnglishStatus.Should().Be(_expectedApiResult.EnglishStatus);
            ActualResult.IsLearnerRegistered.Should().Be(_expectedApiResult.IsLearnerRegistered);
            ActualResult.IndustryPlacementId.Should().Be(_expectedApiResult.IndustryPlacementId);
            ActualResult.IndustryPlacementStatus.Should().Be(_expectedApiResult.IndustryPlacementStatus);
        }
    }
}

