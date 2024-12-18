using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminNotificationLoaderTests
{
    public class When_GetEditBanner_Called_Returns_Expected : AdminNotificationLoaderBaseTest
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

        private AdminEditNotificationViewModel _result;

        public override void Given()
        {
            ApiClient.GetNotificationAsync(NotificationId).Returns(_apiResponse);
        }

        public override async Task When()
        {
            _result = await Loader.GetEditNotificationViewModel(NotificationId);
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
            _result.Target.Should().Be(_apiResponse.Target);

            _result.StartDay.Should().Be(_apiResponse.Start.Day.ToString());
            _result.StartMonth.Should().Be(_apiResponse.Start.Month.ToString());
            _result.StartYear.Should().Be(_apiResponse.Start.Year.ToString());

            _result.EndDay.Should().Be(_apiResponse.End.Day.ToString());
            _result.EndMonth.Should().Be(_apiResponse.End.Month.ToString());
            _result.EndYear.Should().Be(_apiResponse.End.Year.ToString());
        }
    }
}