using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessBulkRegistrationsAsync
{
    public class Then_ApiClient_Mapper_Is_Called : When_ProcessBulkRegistrationsAsync_Is_Called
    {
        [Fact]
        public void Then_ApiClient_Is_Called()
        {
            InternalApiClient.Received().ProcessBulkRegistrationsAsync(BulkRegistrationRequest);
        }

        [Fact]
        public void Then_Mapper_Is_Called()
        {
            Mapper.Received().Map<BulkRegistrationRequest>(UploadRegistrationsRequestViewModel);
        }
    }
}
