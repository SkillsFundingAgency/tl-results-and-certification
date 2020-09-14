using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Services.BlobStorage.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.AddRegistration
{
    public abstract class TestSetup : BaseTest<RegistrationLoader>
    {
        protected readonly long AoUkprn = 12345678;
        protected readonly int ProviderUkprn = 987654321;
        protected IResultsAndCertificationInternalApiClient InternalApiClient;
        protected IMapper Mapper;
        protected ILogger<RegistrationLoader> Logger;
        protected RegistrationLoader Loader;
        protected IBlobStorageService BlobStorageService;
        protected bool ApiClientResponse;
        protected bool ActualResult;
        protected IHttpContextAccessor HttpContextAccessor;
        protected RegistrationViewModel RegistrationViewModel;

        protected readonly string Givenname = "test";
        protected readonly string Surname = "user";
        protected readonly string Email = "test.user@test.com";
        private string _coreCode = "12345678";

        public override void Setup()
        {
            ApiClientResponse = true;

            RegistrationViewModel = new RegistrationViewModel
            {
                Uln = new UlnViewModel { Uln = "1234567890" },
                LearnersName = new LearnersNameViewModel { Firstname = "First", Lastname = "Last" },
                DateofBirth = new DateofBirthViewModel { Day = "01", Month = "01", Year = "2020" },
                SelectProvider = new SelectProviderViewModel { SelectedProviderUkprn = "98765432", SelectedProviderDisplayName = "Barnsley College (98765432)" },
                SelectCore = new SelectCoreViewModel { SelectedCoreCode = _coreCode, SelectedCoreDisplayName = $"Education ({_coreCode})", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } },
                SpecialismQuestion = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true },
                SelectSpecialisms = new SelectSpecialismViewModel { PathwaySpecialisms = new PathwaySpecialismsViewModel { PathwayCode = _coreCode, PathwayName = "Education", Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Code = "7654321", Name = "Test Education", DisplayName = "Test Education (7654321)", IsSelected = true } } } },
                SelectAcademicYear = new SelectAcademicYearViewModel { SelectedAcademicYear = "2020" }
            };

            Logger = Substitute.For<ILogger<RegistrationLoader>>();
            BlobStorageService = Substitute.For<IBlobStorageService>();
            InternalApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            InternalApiClient.AddRegistrationAsync(Arg.Any<RegistrationRequest>()).Returns(ApiClientResponse);

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
                c.AddMaps(typeof(RegistrationMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<RegistrationViewModel, RegistrationRequest>(HttpContextAccessor) : null);
            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }

        public override void Given()
        {
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        public async override Task When()
        {
            ActualResult = await Loader.AddRegistrationAsync(AoUkprn, RegistrationViewModel);
        }
    }
}
