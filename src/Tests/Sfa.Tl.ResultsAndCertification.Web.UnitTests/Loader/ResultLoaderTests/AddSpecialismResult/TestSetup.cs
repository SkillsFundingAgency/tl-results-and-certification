﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.AddSpecialismResult
{
    public abstract class TestSetup : BaseTest<ResultLoader>
    {
        protected readonly long AoUkprn = 12345678;
        protected readonly int ProfileId = 1;
        protected readonly ComponentType componentType = ComponentType.Core;
        protected ManageSpecialismResultViewModel ViewModel;

        protected IMapper Mapper;
        protected ILogger<ResultLoader> Logger;
        protected IBlobStorageService BlobStorageService;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected ResultLoader Loader;
        protected AddResultResponse ActualResult;

        protected IHttpContextAccessor HttpContextAccessor;
        protected readonly string Givenname = "test";
        protected readonly string Surname = "user";
        protected readonly string Email = "test.user@test.com";

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<ResultLoader>>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();

            HttpContextAccessor = Substitute.For<IHttpContextAccessor>();
            HttpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, Givenname),
                    new Claim(ClaimTypes.Surname, Surname),
                    new Claim(ClaimTypes.Email, Email)
                }))
            });

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(ResultMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<ManageSpecialismResultViewModel, AddResultRequest>(HttpContextAccessor) : null);
            });

            Mapper = new AutoMapper.Mapper(mapperConfig);
            Loader = new ResultLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.AddSpecialismResultAsync(AoUkprn, ViewModel);
        }
    }
}
