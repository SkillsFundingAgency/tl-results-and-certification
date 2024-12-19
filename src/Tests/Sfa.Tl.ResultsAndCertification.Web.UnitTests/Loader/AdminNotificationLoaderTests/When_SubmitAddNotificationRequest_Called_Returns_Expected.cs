using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminNotificationLoaderTests
{
    public class When_SubmitAddNotificationRequest_Called_Returns_Expected : AdminNotificationLoaderBaseTest
    {
        private readonly AdminAddNotificationViewModel _request = new()
        {
            Title = "title",
            Content = "content",
            Target = NotificationTarget.Both,
            StartDay = "1",
            StartMonth = "1",
            StartYear = "2024",
            EndDay = "10",
            EndMonth = "7",
            EndYear = "2025"
        };

        private readonly AddNotificationResponse _apiResponse = new()
        {
            Success = true,
            NotificationId = 1
        };

        private AddNotificationResponse _result;

        private readonly Expression<Predicate<AddNotificationRequest>> _apiPredicate =
            r => r.Title == "title"
            && r.Content == "content"
            && r.Target == NotificationTarget.Both
            && r.Start == new DateTime(2024, 1, 1)
            && r.End == new DateTime(2025, 7, 10)
            && !string.IsNullOrEmpty(r.CreatedBy);

        public override void Given()
        {
            ApiClient
                .AddNotificationAsync(Arg.Is(_apiPredicate))
                .Returns(_apiResponse);
        }

        public override async Task When()
        {
            _result = await Loader.SubmitAddNotificationRequest(_request);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).AddNotificationAsync(Arg.Is(_apiPredicate));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeEquivalentTo(_apiResponse);
        }
    }
}