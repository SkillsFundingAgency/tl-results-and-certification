using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelDetailsByPathwayId
{
    public class When_Called_With_TlevelConfirmedDetailsViewModel : TestSetup
    {
        private TLevelConfirmedDetailsViewModel _actualResult { get; set; }

        public override void Given()
        {
            CreateMapper();
            Loader = new TlevelLoader(InternalApiClient, Mapper);
            InternalApiClient.GetTlevelDetailsByPathwayIdAsync(Ukprn, Id).Returns(ApiClientResponse);
        }

        protected override void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(TlevelMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<TlevelPathwayDetails, TLevelConfirmedDetailsViewModel>(HttpContextAccessor) : null);
            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }

        public async override Task When()
        {
            _actualResult = await Loader.GetTlevelDetailsByPathwayIdAsync(Ukprn, Id);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).GetTlevelDetailsByPathwayIdAsync(Ukprn, Id);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.PathwayId.Should().Be(ApiClientResponse.PathwayId);
            _actualResult.IsValid.Should().BeTrue();
            _actualResult.PathwayDisplayName.Should().Be($"{ApiClientResponse.PathwayName}<br/>({ApiClientResponse.PathwayCode})");
            _actualResult.TlevelTitle.Should().Be(ApiClientResponse.TlevelTitle);

            var expectedSpecialisms = ApiClientResponse.Specialisms.Select(s => $"{s.Name}<br/>({s.Code})").ToList();
            _actualResult.Specialisms.Should().BeEquivalentTo(expectedSpecialisms);
        }
    }
}
