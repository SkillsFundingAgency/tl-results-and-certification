using FluentAssertions;
using Newtonsoft.Json;
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
    public class When_AddNotification_Called : ResultsAndCertificationInternalApiClientBaseTest
    {
        private AddNotificationResponse _result;

        private readonly AddNotificationRequest _request = new()
        {
            Title = "title",
            Content = "content",
            Target = NotificationTarget.Both,
            Start = new DateTime(2024, 1, 1),
            End = new DateTime(2024, 1, 2),
            CreatedBy = "test-user"
        };

        private readonly AddNotificationResponse _mockHttpResult = new()
        {
            Success = true,
            NotificationId = 1
        };

        private ResultsAndCertificationInternalApiClient _apiClient;

        public override void Given()
        {
            _apiClient = CreateApiClient(() => new MockHttpMessageHandler<AddNotificationResponse>(_mockHttpResult, ApiConstants.AddNotification, HttpStatusCode.OK, JsonConvert.SerializeObject(_request)));
        }

        public async override Task When()
        {
            _result = await _apiClient.AddNotificationAsync(_request);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeEquivalentTo(_mockHttpResult);
        }
    }
}