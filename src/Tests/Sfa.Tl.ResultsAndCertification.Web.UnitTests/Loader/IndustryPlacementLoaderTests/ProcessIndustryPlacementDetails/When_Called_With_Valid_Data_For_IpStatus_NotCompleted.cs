using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.ProcessIndustryPlacementDetails
{
    public class When_Called_With_Valid_Data_For_IpStatus_NotCompleted : TestSetup
    {
        private readonly bool _expectedApiResult = true;

        public override void Given()
        {
            CreateMapper();

            ProviderUkprn = 987654321;

            ViewModel = new IndustryPlacementViewModel
            {
                IpCompletion = new IpCompletionViewModel
                {
                    ProfileId = 1,
                    RegistrationPathwayId = 1,
                    PathwayId = 7,
                    IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted
                }
            };

            InternalApiClient.ProcessIndustryPlacementDetailsAsync(Arg.Is<IndustryPlacementRequest>(x =>
                                x.ProfileId == ViewModel.IpCompletion.ProfileId &&
                                x.RegistrationPathwayId == ViewModel.IpCompletion.RegistrationPathwayId &&
                                x.IndustryPlacementStatus == ViewModel.IpCompletion.IndustryPlacementStatus &&
                                x.ProviderUkprn == ProviderUkprn &&
                                x.PerformedBy.Equals($"{Givenname} {Surname}")
                                ))
                             .Returns(_expectedApiResult);

            Loader = new IndustryPlacementLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeTrue();
        }

        public override void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(IndustryPlacementMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<IpCompletionViewModel, IndustryPlacementRequest>(HttpContextAccessor) : null);
            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }
    }
}
