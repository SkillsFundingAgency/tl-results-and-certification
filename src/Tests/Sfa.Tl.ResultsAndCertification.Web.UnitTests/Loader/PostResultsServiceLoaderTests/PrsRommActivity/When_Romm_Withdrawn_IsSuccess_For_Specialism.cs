﻿using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.Mapper;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.PrsRommActivity
{
    public class When_Romm_Withdrawn_IsSuccess_For_Specialism : TestSetup
    {
        private PrsAddRommOutcomeViewModel _model;
        private readonly bool _expectedApiResult = true;

        public override void Given()
        {
            CreateMapper();

            _model = new PrsAddRommOutcomeViewModel
            {
                ProfileId = 1,
                AssessmentId = 2,
                ResultId = 3,
                ComponentType = ComponentType.Specialism
            };

            InternalApiClient.PrsActivityAsync(Arg.Is<PrsActivityRequest>(x =>
                                x.ProfileId == _model.ProfileId &&
                                x.AssessentId == _model.AssessmentId &&
                                x.ResultId == _model.ResultId &&
                                x.ComponentType == _model.ComponentType &&
                                x.PrsStatus == PrsStatus.Withdraw &&
                                x.AoUkprn == AoUkprn &&
                                x.PerformedBy == $"{Givenname} {Surname}"
                                ))
                             .Returns(_expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.PrsRommActivityAsync(AoUkprn, _model);
        }

        [Fact]
        public void Then_True_Returned()
        {
            ActualResult.Should().BeTrue();
        }

        public override void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(PostResultsServiceMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("UserNameResolver") ?
                                new UserNameResolver<PrsAddRommOutcomeViewModel, PrsActivityRequest>(HttpContextAccessor) : null);
            });
            Mapper = new AutoMapper.Mapper(mapperConfig);
        }
    }
}
