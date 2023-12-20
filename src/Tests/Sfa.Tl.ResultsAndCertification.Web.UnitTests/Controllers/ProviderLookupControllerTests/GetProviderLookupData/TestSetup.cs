using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Controllers;
using Sfa.Tl.ResultsAndCertification.Web.Loader.Interfaces;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderLookupControllerTests.GetProviderLookupData
{
    public abstract class When_FindProviderAsync_Post_Action_Is_Called : BaseTest<ProviderController>
    {
        // DI Mocks
        protected IProviderLoader ProviderLoader;

        // input, output and other mock variables
        protected ProviderLookupController Controller;
        protected JsonResult Result;
        protected IHttpContextAccessor HttpContextAccessor;

        protected string ProviderName = "King Edward";

        public override void Setup()
        {
            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            ProviderLoader = Substitute.For<IProviderLoader>();
            Controller = new ProviderLookupController(ProviderLoader);
        }

        public async override Task When()
        {
            Result = await Controller.GetProviderLookupDataAsync(ProviderName);
        }
    }
}
