using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.ResultsAndCertificationInternalApiClientTest
{
    public class When_GetNotification_Called : ResultsAndCertificationInternalApiClientBaseTest
    {
        private const int NotificationId = 1;
        private GetNotificationResponse _result;

        private readonly GetNotificationResponse _mockHttpResult = new()
        {
            Id = NotificationId,
            Title = "title",
            Content = "content",
            Target = NotificationTarget.Both,
            Start = new DateTime(2024, 1, 1),
            End = new DateTime(2024, 1, 2)
        };

        private ResultsAndCertificationInternalApiClient _apiClient;

        public override void Given()
        {
            _apiClient = CreateApiClient(() => new MockHttpMessageHandler<GetNotificationResponse>(_mockHttpResult, string.Format(ApiConstants.GetNotification, NotificationId), HttpStatusCode.OK));
        }

        public async override Task When()
        {
            _result = await _apiClient.GetNotificationAsync(NotificationId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeEquivalentTo(_mockHttpResult);
        }
    }
}