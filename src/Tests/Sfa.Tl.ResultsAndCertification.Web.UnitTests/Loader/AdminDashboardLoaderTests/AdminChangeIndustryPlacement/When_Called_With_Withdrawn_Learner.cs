using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.AdminChangeIndustryPlacement
{
    public class When_Called_With_Withdrawn_Learner : BaseTest<AdminDashboardLoader>
    {
        private IResultsAndCertificationInternalApiClient _internalApiClient;
        private AdminDashboardLoader Loader;
        private int PathwayId;

        public override void Setup()
        {
            _internalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AdminDashboardMapper).Assembly));
            var mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new AdminDashboardLoader(_internalApiClient, mapper);
        }
        private Models.Contracts.AdminDashboard.AdminLearnerRecord _expectedApiResult;

        protected AdminChangeIndustryPlacementViewModel ActualResult { get; set; }

        public override void Given()
        {
            PathwayId = 1;

            _expectedApiResult = new Models.Contracts.AdminDashboard.AdminLearnerRecord
            {
                PathwayId = PathwayId,
                RegistrationPathwayId = 1,
                Uln = 786787689,
                Name = "John smith",
                DateofBirth = DateTime.UtcNow.AddYears(-15),
                ProviderName = "Barnsley College",
                TlevelName = "Education and Early Years(60358294)",
                AcademicYear = 2023,
                AwardingOrganisationName = "NCFE",
                MathsStatus = Common.Enum.SubjectStatus.Achieved,
                EnglishStatus = Common.Enum.SubjectStatus.Achieved,
                IsLearnerRegistered = true,
                IndustryPlacementId = 1,
                IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.Completed,
                AcademicStartYearsToBe = new List<int> { 2021, 2022 },
                RegistrationPathwayStatus = Common.Enum.RegistrationPathwayStatus.Withdrawn

            };
            _internalApiClient.GetAdminLearnerRecordAsync(PathwayId).Returns(_expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetAdminLearnerRecordAsync<AdminChangeIndustryPlacementViewModel>(PathwayId);
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
            ActualResult.PathwayId.Should().Be(_expectedApiResult.RegistrationPathwayId);
            ActualResult.RegistrationPathwayId.Should().Be(_expectedApiResult.RegistrationPathwayId);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.Learner.Should().Be($"{_expectedApiResult.FirstName} {_expectedApiResult.LastName}");
            ActualResult.ProviderName.Should().Be(_expectedApiResult.ProviderName);
            ActualResult.TlevelName.Should().Be(_expectedApiResult.TlevelName);
            ActualResult.LearnerRegistrationPathwayStatus.Should().Be(_expectedApiResult.RegistrationPathwayStatus.ToString());
        }
    }
}

