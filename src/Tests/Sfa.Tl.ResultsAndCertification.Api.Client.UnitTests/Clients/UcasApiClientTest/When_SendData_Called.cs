using FluentAssertions;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Api.Client.Clients;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Ucas;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Api.Client.UnitTests.Clients.UcasApiClientTest
{
    public class When_SendData_Called : BaseTest<UcasApiClient>
    {
        protected UcasTokenResponse _mockTokenHttpResult;
        private string _result;
        private UcasDataRequest _ucasDataRequest;
        protected UcasDataResponse _mockHttpResult;
        private ResultsAndCertificationConfiguration _configuration;
        private UcasApiClient _apiClient;

        public override void Setup()
        {
            _configuration = new ResultsAndCertificationConfiguration
            {
                UcasApiSettings = new UcasApiSettings { Uri = "https://transfer.ucasenvironments.com/", Username = "test", Password = "test", Version = 1, FolderId = "12345", GrantType = "pass" }
            };

            var fileData = Encoding.ASCII.GetBytes("Hello");
            var fileHash = BitConverter.ToString(SHA256.Create().ComputeHash(fileData)).Replace("-", "");
            _ucasDataRequest = new UcasDataRequest
            {
                FileName = $"{Guid.NewGuid()}.{Constants.FileExtensionTxt}",
                FileHash = fileHash,
                FileData = fileData
            };

            _mockTokenHttpResult = new UcasTokenResponse { AccessToken = Guid.NewGuid().ToString() };
            _mockHttpResult = new UcasDataResponse { Id = "12345" };
        }

        public override void Given()
        {
            string requestParameters = string.Format(ApiConstants.UcasTokenParameters, _configuration.UcasApiSettings.GrantType, _configuration.UcasApiSettings.Username, _configuration.UcasApiSettings.Password);
            var mockHttpHandler = new MockHttpMessageHandler<UcasTokenResponse>(_mockTokenHttpResult, $"{string.Format(ApiConstants.UcasBaseUri, _configuration.UcasApiSettings.Version)}{ApiConstants.UcasTokenUri}", HttpStatusCode.OK, requestParameters);

            var requestUri = $"{string.Format(ApiConstants.UcasBaseUri, _configuration.UcasApiSettings.Version)}{string.Format(ApiConstants.UcasFileUri, _configuration.UcasApiSettings.FolderId)}";
            var content = new MultipartFormDataContent
            {
                { new StringContent(ApiConstants.SHA256), ApiConstants.FormDataHashType },
                { new StringContent(_ucasDataRequest.FileHash), ApiConstants.FormDataHash },
                {
                    new StreamContent(new MemoryStream(_ucasDataRequest.FileData))
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("multipart/form-data")
                        }
                    },
                    ApiConstants.FormDataFile,
                    _ucasDataRequest.FileName
                }
            };
            mockHttpHandler.AddMultipartHttpResponses(_mockHttpResult, requestUri, HttpStatusCode.OK, content);

            HttpClient = new HttpClient(mockHttpHandler);
            _apiClient = new UcasApiClient(HttpClient, _configuration);
        }

        public async override Task When()
        {
            _result = await _apiClient.SendDataAsync(_ucasDataRequest);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();            
        }
    }
}
