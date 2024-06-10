using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetPendingWithdrawalsDataFile
{
    public abstract class TestSetup : BaseTest<RegistrationLoader>
    {
        protected const long AoUkprn = 10009696;
        protected Guid BlobUniqueReference = new("f47d7a4e-9b8c-4a6f-8e4d-2e3b1a5c9f0d");

        protected IBlobStorageService BlobStorageService { get; private set; }
        protected Stream ActualResult;

        protected RegistrationLoader Loader;

        public override void Setup()
        {
            BlobStorageService = Substitute.For<IBlobStorageService>();

            Loader = new RegistrationLoader(
                Substitute.For<IMapper>(),
                Substitute.For<ILogger<RegistrationLoader>>(),
                Substitute.For<IResultsAndCertificationInternalApiClient>(),
                BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetPendingWithdrawalsDataFileAsync(AoUkprn, BlobUniqueReference);
        }
    }
}