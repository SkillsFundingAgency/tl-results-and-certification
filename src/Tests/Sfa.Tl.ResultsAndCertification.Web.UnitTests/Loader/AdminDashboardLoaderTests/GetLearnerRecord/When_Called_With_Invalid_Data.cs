using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
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
    public class When_Called_With_Invalid_Data : BaseTest<AdminDashboardLoader>
    {
        private IResultsAndCertificationInternalApiClient _internalApiClient;
        private AdminDashboardLoader Loader;

        private AdminLearnerRecordViewModel _expectedResult;
        private AdminLearnerRecordViewModel _actualResult;
        private int PathwayId;

        public override void Setup()
        {
            _internalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AdminDashboardMapper).Assembly));
            var mapper = new AutoMapper.Mapper(mapperConfig);

            Loader = new AdminDashboardLoader(_internalApiClient, mapper);
        }

        public override void Given()
        {
            PathwayId = -1;
        }

        public async override Task When()
        {
            _actualResult = await Loader.GetAdminLearnerRecordAsync<AdminLearnerRecordViewModel>(PathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            _internalApiClient.Received(1).GetAdminLearnerRecordAsync(Arg.Any<int>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().BeNull();
        }
    }
}
