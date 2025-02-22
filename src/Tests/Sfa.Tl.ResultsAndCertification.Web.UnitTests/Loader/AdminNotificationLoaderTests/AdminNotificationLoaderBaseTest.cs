﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminNotificationLoaderTests
{
    public abstract class AdminNotificationLoaderBaseTest : BaseTest<AdminProviderLoader>
    {
        protected IResultsAndCertificationInternalApiClient ApiClient;
        protected AdminNotificationLoader Loader;

        public override void Setup()
        {
            ApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            Loader = new AdminNotificationLoader(ApiClient, CreateMapper());
        }

        private static AutoMapper.Mapper CreateMapper()
        {
            IHttpContextAccessor httpContextAccessor = Substitute.For<IHttpContextAccessor>();

            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "test"),
                    new Claim(ClaimTypes.Surname, "user"),
                    new Claim(ClaimTypes.Email, "test.user@test.com")
                }))
            });

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(AdminProviderLoader).Assembly);

                c.ConstructServicesUsing(type =>
                {
                    if (type.Equals(typeof(UserNameResolver<AdminEditNotificationViewModel, UpdateNotificationRequest>)))
                    {
                        return new UserNameResolver<AdminEditNotificationViewModel, UpdateNotificationRequest>(httpContextAccessor);
                    }
                    if (type.Equals(typeof(UserNameResolver<AdminAddNotificationViewModel, AddNotificationRequest>)))
                    {
                        return new UserNameResolver<AdminAddNotificationViewModel, AddNotificationRequest>(httpContextAccessor);
                    }
                    else
                    {
                        return null;
                    }
                });
            });

            return new AutoMapper.Mapper(mapperConfig);
        }
    }
}