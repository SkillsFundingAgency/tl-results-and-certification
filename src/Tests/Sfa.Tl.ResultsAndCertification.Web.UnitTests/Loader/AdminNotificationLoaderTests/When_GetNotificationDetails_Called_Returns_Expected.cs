using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminNotificationLoaderTests
{
    public class When_GetNotificationDetails_Called_Returns_Expected : AdminNotificationLoaderBaseTest
    {
        private const int NotificationId = 1;

        private readonly GetNotificationResponse _apiResponse = new()
        {
            Id = 1,
            Title = "title",
            Content = "content",
            Target = NotificationTarget.AwardingOrganisation,
            Start = new DateTime(2024, 12, 1),
            End = new DateTime(2024, 12, 31)
        };

        private AdminNotificationDetailsViewModel _result;

        public override void Given()
        {
            ApiClient.GetNotificationAsync(NotificationId).Returns(_apiResponse);
        }

        public override async Task When()
        {
            _result = await Loader.GetNotificationDetailsViewModel(NotificationId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetNotificationAsync(NotificationId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();

            _result.NotificationId.Should().Be(_apiResponse.Id);
            _result.Title.Should().Be(_apiResponse.Title);
            _result.Content.Should().Be(_apiResponse.Content);

            _result.SummaryTarget.Id.Should().Be("target");
            _result.SummaryTarget.Title.Should().Be(AdminNotificationDetails.Label_Target);
            _result.SummaryTarget.Value.Should().Be("Awarding organisation");

            _result.SummaryStartDate.Id.Should().Be("startdate");
            _result.SummaryStartDate.Title.Should().Be(AdminNotificationDetails.Label_Start_Date);
            _result.SummaryStartDate.Value.Should().Be(_apiResponse.Start.ToDobFormat());

            _result.SummaryEndDate.Id.Should().Be("enddate");
            _result.SummaryEndDate.Title.Should().Be(AdminNotificationDetails.Label_End_Date);
            _result.SummaryEndDate.Value.Should().Be(_apiResponse.End.ToDobFormat());

            _result.BackLink.RouteName.Should().Be(RouteConstants.AdminFindNotification);
            _result.BackLink.RouteAttributes.Should().BeEmpty();

            _result.SuccessBanner.Should().BeNull();

            _result.DashboardBanner.Should().NotBeNull();
            _result.DashboardBanner.Message.Should().Be(_apiResponse.Content);
        }
    }
}