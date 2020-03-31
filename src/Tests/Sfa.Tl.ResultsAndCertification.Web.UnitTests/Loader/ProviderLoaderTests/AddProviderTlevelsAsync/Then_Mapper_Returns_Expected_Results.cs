using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.AddProviderTlevelsAsync
{
    public class Then_Mapper_Returns_Expected_Results : When_Called_Method_AddProviderTlevelsAsync
    {
        //private IHttpContextAccessor _httpContextAccessor;
        

        //public override void Given()
        //{
        //    //_httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        //    //_httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
        //    //{
        //    //    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        //    //    {
        //    //        new Claim(ClaimTypes.GivenName, _givename),
        //    //        new Claim(ClaimTypes.Surname, _surname),
        //    //        new Claim(ClaimTypes.Email, _email)
        //    //    }))
        //    //});

        //    //CreateMapper();

        //    //ProviderTlevelDetails = new List<ProviderTlevelDetails>
        //    //        {
        //    //            new ProviderTlevelDetails { TqAwardingOrganisationId = 1, ProviderId = 1, PathwayId = 1, CreatedBy = "test user" },
        //    //            new ProviderTlevelDetails { TqAwardingOrganisationId = 2, ProviderId = 1, PathwayId = 2, CreatedBy = "test user" }
        //    //        };

        //    //ProviderTlevelsViewModel = new ProviderTlevelsViewModel
        //    //{
        //    //    ProviderId = 1,
        //    //    DisplayName = "Test1",
        //    //    Ukprn = 1234589,
        //    //    Tlevels = new List<ProviderTlevelDetailsViewModel>
        //    //        {
        //    //            new ProviderTlevelDetailsViewModel { TqAwardingOrganisationId = 1, ProviderId = 1, PathwayId = 1, IsSelected = true },
        //    //            new ProviderTlevelDetailsViewModel { TqAwardingOrganisationId = 2, ProviderId = 1, PathwayId = 2, IsSelected = true }
        //    //        }
        //    //};
        //    ExpectedResult = true;
        //    InternalApiClient.AddProviderTlevelsAsync(ProviderTlevelDetails).Returns(true);
        //    Loader = new ProviderLoader(InternalApiClient, Mapper);
        //}

        [Fact]
        public void Then_Mapper_Has_Expected_Results()
        {
            var result = Mapper.Map<List<ProviderTlevelDetails>>(ProviderTlevelsViewModel.Tlevels);

            result.Count.Should().Be(2);

            result[0].TqAwardingOrganisationId.Should().Be(ProviderTlevelsViewModel.Tlevels[0].TqAwardingOrganisationId);
            result[0].ProviderId.Should().Be(ProviderTlevelsViewModel.Tlevels[0].ProviderId);
            result[0].PathwayId.Should().Be(ProviderTlevelsViewModel.Tlevels[0].PathwayId);
            result[0].CreatedBy.Should().Be($"{Givenname} {Surname}");

            result[1].TqAwardingOrganisationId.Should().Be(ProviderTlevelsViewModel.Tlevels[1].TqAwardingOrganisationId);
            result[1].ProviderId.Should().Be(ProviderTlevelsViewModel.Tlevels[1].ProviderId);
            result[1].PathwayId.Should().Be(ProviderTlevelsViewModel.Tlevels[1].PathwayId);
            result[1].CreatedBy.Should().Be($"{Givenname} {Surname}");
        }


        //protected void CreateMapper()
        //{
        //    var mapperConfig = new MapperConfiguration(c =>
        //    {
        //        c.AddMaps(typeof(ProviderMapper).Assembly);
        //        c.ConstructServicesUsing(type =>
        //                    type.Name.Contains("UserNameResolver") ?
        //                        new UserNameResolver<ProviderTlevelDetailsViewModel, ProviderTlevelDetails>(_httpContextAccessor) : null);
        //    });
        //    Mapper = new AutoMapper.Mapper(mapperConfig);
        //}
    }
}
