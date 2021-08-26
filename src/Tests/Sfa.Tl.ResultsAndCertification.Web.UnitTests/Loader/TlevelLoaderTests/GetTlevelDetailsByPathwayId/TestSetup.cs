using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System.Collections.Generic;
using System.Security.Claims;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelDetailsByPathwayId
{
    public abstract class TestSetup : BaseTest<TlevelLoader>
    {
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected TlevelLoader Loader;
        protected TLevelConfirmedDetailsViewModel ActualResult;
        protected readonly int Id = 9;
        protected readonly long Ukprn = 1024;
        protected TlevelPathwayDetails ApiClientResponse;
        protected TLevelConfirmedDetailsViewModel ExpectedResult;

        protected readonly int PathwayId = 1;
        protected readonly string PathwayName = "Pathway Name1";
        protected readonly string RouteName = "Route Name1";
        protected readonly bool ShowSomethingIsNotRight = true;
        protected readonly bool ShowQueriedInfo = false;
        protected List<SpecialismDetails> Specialisms;

        protected readonly string Givenname = "test";
        protected readonly string Surname = "user";
        protected readonly string Email = "test.user@test.com";

        // Dependencies
        protected IHttpContextAccessor HttpContextAccessor;

        public override void Setup()
        {
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

            Specialisms = new List<SpecialismDetails> {
                new SpecialismDetails { Name = "Civil Engineering", Code = "97865897" },
                new SpecialismDetails { Name = "Assisting teaching", Code = "7654321" }
            };

            ApiClientResponse = new TlevelPathwayDetails { PathwayId = 1, PathwayName = PathwayName, RouteName = RouteName, PathwayStatusId = 2, Specialisms = Specialisms };
            ExpectedResult = new TLevelConfirmedDetailsViewModel { PathwayId = 1, IsValid = ShowSomethingIsNotRight, Specialisms = new List<string> { "Civil Engineering<br/>(97865897)", "Assisting teaching<br/>(7654321)" } };

            CreateMapper();

            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
        }

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(TlevelMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<TlevelPathwayDetails, TLevelConfirmedDetailsViewModel>(HttpContextAccessor) :
                                null);
            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }               
    }
}
