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
    public class When_UpdateNotification_Called : ResultsAndCertificationInternalApiClientBaseTest
    {
        private bool _result;

        private readonly UpdateNotificationRequest _request = new()
        {
            NotificationId = 1,
            Title = "title",
            Content = "content",
            Target = NotificationTarget.Both,
            Start = new DateTime(2024, 1, 1),
            End = new DateTime(2024, 1, 2),
            ModifiedBy = "test-user"
        };

        private readonly bool _mockHttpResult = true;

        private ResultsAndCertificationInternalApiClient _apiClient;

        public override void Given()
        {
            _apiClient = CreateApiClient(() => new MockHttpMessageHandler<bool>(_mockHttpResult, ApiConstants.UpdateNotification, HttpStatusCode.OK, JsonConvert.SerializeObject(_request)));
        }

        public async override Task When()
        {
            _result = await _apiClient.UpdateNotificationAsync(_request);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().Be(_mockHttpResult);
        }
    }
}