using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.AdminAddCoreAssessmentEntry
{
    public abstract class TestSetup : BaseTest<AdminDashboardLoader>
    {
        
        protected AdminReviewChangesCoreAssessmentViewModel ViewModel;
        protected IMapper Mapper;      
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected AdminDashboardLoader Loader;
        protected bool ActualResult;

        protected IHttpContextAccessor HttpContextAccessor;
        protected readonly string Givenname = "test";
        protected readonly string Surname = "user";
        protected readonly string Email = "test.user@test.com";

        public override void Setup()
        {
           
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
                c.AddMaps(typeof(AssessmentMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<AdminReviewChangesCoreAssessmentViewModel, ReviewAddCoreAssessmentRequest>(HttpContextAccessor) : null);
            });

            Mapper = new AutoMapper.Mapper(mapperConfig);
            Loader = new AdminDashboardLoader(InternalApiClient, Mapper);
        }

        public async override Task When()
        {
            ActualResult = await Loader.ProcessAddCoreAssessmentRequestAsync(ViewModel);
        }
    }
}
